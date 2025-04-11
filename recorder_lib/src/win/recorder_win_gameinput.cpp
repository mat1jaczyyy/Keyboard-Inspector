#include <recorder.h>
#include <gameinput.h>
#include <wil/win32_result_macros.h>
#include <boost/container/static_vector.hpp>
#include <algorithm>
#include <chrono>
#include <thread>
#include <stop_token>

using namespace GameInput::v1;
using namespace std::chrono;
using namespace std::literals;
using KeyStateArray = boost::container::static_vector<GameInputKeyState, 50>;

bool sort_by_scancode(const GameInputKeyState& a, const GameInputKeyState& b)
{
    return a.scanCode < b.scanCode;
}

class recorder::impl
{
public:
    bool recording() const;
    void start(bool keyboard, bool mouse, bool gamepad);
    void stop();
    const recorder::device_map& devices() const;
    const recorder::input_map& inputs() const;

private:
    recorder::device_map m_devices;
    recorder::input_map m_inputs;

    std::unordered_map<std::string, KeyStateArray> m_key_states;

    bool _gameinput_poll(GameInputKind kind);
    void _update_key_states(
        const std::string& id, std::uint64_t timestamp, const KeyStateArray& state
    );
    bool m_running = false;
    std::jthread m_poll_thread;
    IGameInput *m_gameinput;
    std::uint64_t m_timestamp_ref;
};

void recorder::impl::_update_key_states(
    const std::string& id, std::uint64_t timestamp, const KeyStateArray& state
)
{
    auto& inputs = m_inputs[id];
    auto& state_prev = m_key_states[id];
    KeyStateArray pressed;
    KeyStateArray released;
    std::set_difference(
        state_prev.begin(), state_prev.end(),
        state.begin(), state.end(),
        std::back_inserter(released),
        sort_by_scancode
    );
    std::set_difference(
        state.begin(), state.end(),
        state_prev.begin(), state_prev.end(),
        std::back_inserter(pressed),
        sort_by_scancode
    );
    for (auto& i : pressed)
    {
        inputs.emplace_back(timestamp, true, i.scanCode);
    }
    for (auto& i : released)
    {
        inputs.emplace_back(timestamp, false, i.scanCode);
    }
    m_key_states[id] = state;
}

bool recorder::impl::_gameinput_poll(GameInputKind kind)
{
    IGameInputReading *reading = nullptr;
    IGameInputDevice *device = nullptr;
    const GameInputDeviceInfo *device_info = nullptr;

    auto result = m_gameinput->GetCurrentReading(kind, nullptr, &reading);
    if (FAILED(result))
        return false;

    reading->GetDevice(&device);
    device->GetDeviceInfo(&device_info);

    std::string pnp = device_info->pnpPath;
    auto vid = device_info->vendorId;
    auto pid = device_info->productId;        
    m_devices.try_emplace(pnp, vid, pid, ""s);

    auto timestamp = reading->GetTimestamp();
    auto input_kind = reading->GetInputKind();
    if (input_kind & GameInputKind::GameInputKindKeyboard)
    {
        KeyStateArray state;
        auto state_len = reading->GetKeyCount();
        state.resize(state_len);
        reading->GetKeyState(state_len, state.data());
        std::sort(state.begin(), state.end(), sort_by_scancode);
        _update_key_states(pnp, timestamp, state);
    }

    device->Release();
    reading->Release();
    return true;
}

void recorder::impl::start(bool keyboard, bool mouse, bool gamepad)
{
    if (m_running)
        throw std::runtime_error("The recorder is already running");
    m_running = true;
    THROW_IF_FAILED_MSG(GameInputCreate(&m_gameinput), "Failed to initialize GameInput");
    m_gameinput->SetFocusPolicy(GameInputDefaultFocusPolicy);
    GameInputKind kind = GameInputKind::GameInputKindUnknown;
    if (keyboard)
        kind |= GameInputKind::GameInputKindKeyboard;
    if (mouse)
        kind |= GameInputKind::GameInputKindMouse;
    if (gamepad)
        kind |= GameInputKind::GameInputKindGamepad;
    m_timestamp_ref = m_gameinput->GetCurrentTimestamp();
    m_devices.clear();
    m_inputs.clear();
    m_key_states.clear();
    m_poll_thread = std::jthread([=](std::stop_token stop) {
        while (!stop.stop_requested())
        {
            if (!_gameinput_poll(kind))
                std::this_thread::yield();
        }
    });
}

void recorder::impl::stop()
{
    if (!m_running)
        return;
    m_poll_thread.request_stop();
    m_poll_thread.join();
    if (m_gameinput)
    {
        m_gameinput->Release();
        m_gameinput = nullptr;
    }
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