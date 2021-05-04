using System.Collections.ObjectModel;

namespace Keyboard_Inspector {
    public class Result {
        public double Time;

        public ReadOnlyCollection<KeyEvent> Events;

        public Result() {}

        public Result(double time, ReadOnlyCollection<KeyEvent> events) {
            Time = time;
            Events = events;
        }
    }
}
