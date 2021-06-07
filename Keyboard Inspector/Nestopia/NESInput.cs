using System.Drawing;
using System.Windows.Forms;

namespace Keyboard_Inspector {
    enum NESKeys {
        A = 0,
        B = 1,
        Select = 2,
        Start = 3,
        Up = 4,
        Down = 5,
        Left = 6,
        Right = 7
    }

    class NESInput: Input<NESKeys> {
        public NESInput(NESKeys k): base(k) {}

        public override string Source => "NES";

        public override Color DefaultColor => Color.Red;
    }
}
