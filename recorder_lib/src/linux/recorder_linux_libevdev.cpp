#include <recorder.h>
#include <libevdev/libevdev.h>
#include <boost/container/static_vector.hpp>
#include <boost/unordered/concurrent_flat_map.hpp>
#include <boost/iterator/transform_iterator.hpp>
#include <thread>
#include <string>
#include <string_view>
#include <utility>
#include <fcntl.h>
#include <glob.h>
#include <poll.h>
#include <unistd.h>
#include <sys/inotify.h>
#include <iostream>

#include "evdev_key_codes.h"

using namespace boost::container;
using namespace boost::unordered;
using namespace std::literals;

struct evdev_device
{
    libevdev* dev = nullptr;
    bool remove = false;
};

class recorder::impl
{
public:
    bool recording() const;
    void start(bool keyboard, bool mouse, bool gamepad);
    void stop();
    const recorder::device_map& devices() const;
    const recorder::input_map& inputs() const;

private:
    bool m_running = false;
    recorder::device_map m_devices;
    recorder::input_map m_inputs;

    using evdev_device_map = concurrent_flat_map<std::string, evdev_device>;
    evdev_device_map m_evdev_devices;
    void _init_scan_devices();
    void _init_poll(bool keyboard, bool mouse, bool gamepad);
    std::jthread m_device_scan_thread;
    std::jthread m_poll_thread;
};

bool is_keyboard(libevdev* device)
{
    return libevdev_has_event_code(device, EV_KEY, BTN_A);
}

bool is_mouse(libevdev* device)
{
    return libevdev_has_event_code(device, EV_KEY, BTN_LEFT);
}

bool is_gamepad(libevdev* device)
{
    // TODO: get a gamepad and implement this
    return false;
}

libevdev* evdev_open(const char* path)
{
    int fd = open(path, O_RDONLY | O_NONBLOCK);
    if (fd < 0)
    {
        return nullptr;
    }
    libevdev* device;
    if (libevdev_new_from_fd(fd, &device) != 0)
    {
        close(fd);
        return nullptr;
    }
    return device;
}

void evdev_close(libevdev* device)
{
    int fd = libevdev_get_fd(device);
    if (fd > 0)
        close(fd);
    libevdev_free(device);
}

void recorder::impl::_init_scan_devices()
{
    auto default_construct = [](const std::string& path) {
        return std::make_pair(path, evdev_device());
    };
    glob_t glob_result;
    if (glob("/dev/input/event*", 0, nullptr, &glob_result) == 0)
    {
        auto gl_pathc = glob_result.gl_pathc;
        auto gl_pathv = glob_result.gl_pathv;
        m_evdev_devices.insert(
            boost::make_transform_iterator(gl_pathv, default_construct),
            boost::make_transform_iterator(gl_pathv + gl_pathc, default_construct)
        );
    }
    m_device_scan_thread = std::jthread([&](const std::stop_token& stop) {
        int notify_fd = inotify_init1(IN_NONBLOCK);
        inotify_add_watch(notify_fd, "/dev/input", IN_CREATE | IN_DELETE);
        pollfd poll_struct{
            .fd = notify_fd,
            .events = POLLIN
        };
        while (!stop.stop_requested())
        {
            int result = poll(&poll_struct, 1, 100);
            if (result == 0)
                continue;
            constexpr int notify_event_count = 100;
            std::array<inotify_event, notify_event_count> notify_events;
            while (true)
            {
                auto len = read(
                    notify_fd,
                    reinterpret_cast<char*>(notify_events.data()),
                    sizeof(inotify_event) * notify_events.size()
                );
                if (len < 0)
                {
                    if (errno != EAGAIN)
                        throw std::runtime_error("An error occurred when checking for devices");
                    break;
                }
                static_vector<std::string, notify_event_count> added, removed;                
                for (auto i = notify_events.begin(); i != notify_events.begin() + len / sizeof(inotify_event); i++)
                {
                    std::string_view path(i->name);
                    if (path.find("event") == std::string::npos)
                        continue;
                    if (i->mask & IN_CREATE)
                    {
                        added.emplace_back("/dev/input/"s.append(path));
                    }
                    else if (i->mask & IN_DELETE)
                    {
                        removed.emplace_back("/dev/input/"s.append(path));
                    }
                }
                m_evdev_devices.insert(
                    boost::make_transform_iterator(added.begin(), default_construct),
                    boost::make_transform_iterator(added.end(), default_construct)
                );
                m_evdev_devices.visit(removed.begin(), removed.end(),
                    [](evdev_device_map::value_type& dev) {
                        dev.second.remove = true;
                    }
                );
            }
        }
        close(notify_fd);
    });
}

void recorder::impl::_init_poll(bool keyboard, bool mouse, bool gamepad)
{
    m_poll_thread = std::jthread([&](const std::stop_token& stop) {
        std::vector<libevdev*> devices;
        while (!stop.stop_requested())
        {
            devices.clear();
            // I'm not sure if this is safe (modify elements inside erase_if). Let's see
            m_evdev_devices.erase_if([&](evdev_device_map::value_type& val) {
                auto& [path, dev] = val;
                if (dev.remove && dev.dev)
                {
                    evdev_close(dev.dev);
                    return true;
                }
                // New device, try to open it
                if (!dev.dev)
                {
                    dev.dev = evdev_open(path.data());
                    if (!dev.dev)
                        return true;
                    // disable all events, and only enable the one we need
                    libevdev_disable_event_type(dev.dev, EV_KEY);
                    libevdev_disable_event_type(dev.dev, EV_REL);
                    libevdev_disable_event_type(dev.dev, EV_ABS);
                    libevdev_disable_event_type(dev.dev, EV_MSC);
                    libevdev_disable_event_type(dev.dev, EV_SW);
                    libevdev_disable_event_type(dev.dev, EV_LED);
                    libevdev_disable_event_type(dev.dev, EV_SND);
                    libevdev_disable_event_type(dev.dev, EV_REP);
                    libevdev_disable_event_type(dev.dev, EV_FF);
                    libevdev_disable_event_type(dev.dev, EV_PWR);
                    libevdev_disable_event_type(dev.dev, EV_FF_STATUS);
                    if (keyboard)
                    {
                        for (auto code: code_keyboard)
                            libevdev_enable_event_code(dev.dev, EV_KEY, code, nullptr);
                    }
                    if (mouse)
                    {
                        for (auto code: code_mouse)
                            libevdev_enable_event_code(dev.dev, EV_KEY, code, nullptr);
                    }
                    if (gamepad)
                    {
                        // TODO: fill in gamepad codes
                    }
                }
                devices.push_back(dev.dev);
                return false;
            });
            // poll all the devices
            for (auto device: devices)
            {
                input_event ev;
                int rc, flags = LIBEVDEV_READ_FLAG_NORMAL;
                while (true)
                {
                    rc = libevdev_next_event(device, flags, &ev);
                    if (rc == LIBEVDEV_READ_STATUS_SUCCESS)
                    {
                        if (ev.type != EV_KEY || ev.value == 2)
                            continue;
                        std::string id(libevdev_get_phys(device));
                        std::uint16_t vid = libevdev_get_id_vendor(device);
                        std::uint16_t pid = libevdev_get_id_product(device);
                        m_devices.try_emplace(id, vid, pid, ""s);
                        m_inputs[id].emplace_back(
                            ev.input_event_sec * 1000000ULL + ev.input_event_usec,
                            ev.value,
                            ev.code
                        );
                        continue;
                    }
                    else if (rc == LIBEVDEV_READ_STATUS_SYNC)
                    {
                        flags = LIBEVDEV_READ_FLAG_SYNC;
                        continue;
                    }
                    else if (rc == -EAGAIN)
                    {
                        flags = LIBEVDEV_READ_FLAG_NORMAL;
                    }
                    break;
                }
            }
        }
    });
}

void recorder::impl::start(bool keyboard, bool mouse, bool gamepad)
{
    if (m_running)
        throw std::runtime_error("The recorder is already running");
    m_running = true;
    _init_scan_devices();
    _init_poll(keyboard, mouse, gamepad);
}

void recorder::impl::stop()
{
    if (!m_running)
        return;
    m_device_scan_thread.request_stop();
    m_poll_thread.request_stop();
    m_device_scan_thread.join();
    m_poll_thread.join();
    m_evdev_devices.visit_all([](evdev_device_map::value_type& entry) {
        if (entry.second.dev)
            evdev_close(entry.second.dev);
    });
    m_evdev_devices.clear();
    m_running = false;
}

bool recorder::impl::recording() const
{
    return m_running;
}

const recorder::device_map& recorder::impl::devices() const
{
    return m_devices;
}

const recorder::input_map& recorder::impl::inputs() const
{
    return m_inputs;
}

// A bunch of boilerplate for pImpl
recorder::recorder(): p_impl(new recorder::impl)
{
}
recorder::~recorder()
{
    stop();
}
recorder::recorder(recorder&&) = default;
recorder& recorder::operator=(recorder&&) = default;

bool recorder::recording() const
{
    return p_impl->recording();
}
void recorder::start(bool keyboard, bool mouse, bool gamepad)
{
    p_impl->start(keyboard, mouse, gamepad);
}
void recorder::stop()
{
    p_impl->stop();
}
const recorder::device_map& recorder::devices() const
{
    return p_impl->devices();
}
const recorder::input_map& recorder::inputs() const
{
    return p_impl->inputs();
}