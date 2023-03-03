using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Keyboard_Inspector {
    class KeyInput: Input<Keys> {
        public KeyInput(Keys k): base(k) {}

        public override string Source => "Keyboard";

        public override Color DefaultColor => Color.Gray;

        protected override char BinaryID => 'k';

        public static KeyInput FromBinaryDerived(BinaryReader br) {
            return new KeyInput((Keys)br.ReadInt32());
        }
    }
}
