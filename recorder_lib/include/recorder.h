#pragma once

#include <memory>
#include <cstdint>
#include <string>
#include <vector>
#include <unordered_set>
#include <unordered_map>
#include <utility>

struct device {
    std::uint16_t vid;
    std::uint16_t pid;
    std::string name;
    device(std::uint16_t vid, std::uint16_t pid, const std::string& name):
        vid(vid), pid(pid), name(name)
    {
    }
};

struct input {
    std::uint64_t timestamp;
    bool pressed;
    std::uint64_t code;
    input(std::uint64_t timestamp, bool pressed, std::uint64_t code):
        timestamp(timestamp), pressed(pressed), code(code)
    {
    }
};

class recorder {
public:
    using device_map = std::unordered_map<std::string, device>;
    using input_map = std::unordered_map<std::string, std::vector<input>>;

    recorder();
    ~recorder();
    recorder(recorder&&);
    recorder& operator=(recorder&&);
    recorder(const recorder&) = delete;
    recorder& operator=(const recorder&) = delete;

    bool recording() const;
    void start(bool keyboard = true, bool mouse = false, bool gamepad = false);
    void stop();
    const device_map& devices() const;
    const input_map& inputs() const;
private:
    class impl;
    std::unique_ptr<impl> p_impl;
};
