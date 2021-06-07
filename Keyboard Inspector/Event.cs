namespace Keyboard_Inspector {
    class Event {
        public readonly double Time;
        public readonly bool Pressed;
        public readonly Input Input;

        public Event(double time, bool pressed, Input input) {
            Time = time;
            Pressed = pressed;
            Input = input;
        }

        public override string ToString()
            => $"{Time:0.00000} {(Pressed? "DN" : "UP")} {Input}";
    }
}
