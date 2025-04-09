#include <recorder.h>
#include <chrono>
#include <iostream>
#include <thread>

using namespace std::literals;

extern "C" int portable_getch();

int main(int argc, char const *argv[])
{
    recorder rec;
    rec.start();
    std::cout << "Keep spamming, press Esc to end...\n";
    while (portable_getch() != 27 /* ESC */)
    {
    }
    rec.stop();
    auto& inputs = rec.inputs();
    std::cout << "Recorded " << inputs.size() << " devices\n";
    for (auto& [device_id, events]: inputs)
    {
        std::cout << "- Device " << device_id << '\n';
        std::cout << "  - Recorded " << events.size() << " events\n";
    }
    return 0;
}
