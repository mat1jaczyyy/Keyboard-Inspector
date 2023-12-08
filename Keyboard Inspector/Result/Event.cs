using System.IO;

namespace Keyboard_Inspector {
    class Event: IBinary {
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

        public void ToBinary(BinaryWriter bw) {
            bw.Write(Time);
            bw.Write(Pressed);
            Input.ToBinary(bw);
        }

        public static Event FromBinary(BinaryReader br, uint fileVersion) {
            return new Event(
                br.ReadDouble(),
                br.ReadBoolean(),
                Input.FromBinary(br, fileVersion)
            );
        }
    }
}
