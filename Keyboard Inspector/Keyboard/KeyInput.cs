using System.Drawing;
using System.Windows.Forms;
using System.Xml;

namespace Keyboard_Inspector {
    class KeyInput: Input<Keys> {
        public KeyInput(Keys k): base(k) {}

        public override string Source => "Keyboard";

        public override Color DefaultColor => Color.Gray;

        protected override string XMLName => "ki";

        public static KeyInput FromXMLDerived(XmlNode node) {
            node.Ensure("ki");

            return new KeyInput(node.InnerText.ToEnum<Keys>());
        }
    }
}
