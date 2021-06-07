using System.Drawing;
using System.Windows.Forms;

namespace Keyboard_Inspector {
    class KeyInput: Input<Keys> {
        public KeyInput(Keys k): base(k) {}

        public override string Source => "Keyboard";

        public override Color DefaultColor => Color.Gray;
    }
}
