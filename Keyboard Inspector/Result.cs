using System.Collections.ObjectModel;

namespace Keyboard_Inspector {
    class Result {
        public double Time;

        public ReadOnlyCollection<Event> Events;

        public Result() {}

        public Result(double time, ReadOnlyCollection<Event> events) {
            Time = time;
            Events = events;
        }
    }
}
