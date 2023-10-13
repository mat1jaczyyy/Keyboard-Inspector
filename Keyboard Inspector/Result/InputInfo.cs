using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace Keyboard_Inspector {
    class InputInfo: IBinary {
        public readonly Input Input;
        public Color Color;
        public bool Visible;

        public List<Event> Events = new List<Event>();

        public InputInfo(Input input, Color color, bool visible) {
            Input = input;
            Color = color;
            Visible = visible;
        }

        public InputInfo(Input input)
        : this(input, Input.DefaultColor, true) {}

        public void ToBinary(BinaryWriter bw) {
            Input.ToBinary(bw);
            bw.Write(Color.ToArgb());
            bw.Write(Visible);
        }

        public static InputInfo FromBinary(BinaryReader br, uint fileVersion) {
            return new InputInfo(
                Input.FromBinary(br, fileVersion),
                Color.FromArgb(br.ReadInt32()),
                br.ReadBoolean()
            );
        }
    }
}
