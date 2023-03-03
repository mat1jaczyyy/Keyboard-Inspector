using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace Keyboard_Inspector {
    enum WiitarKeys {
        Green = 0,
        Red = 1,
        Yellow = 2,
        Blue = 3,
        Orange = 4,
        Downstrum = 5,
        Start = 6,
        Select = 7,
        Upstrum = 8
    }

    class WiitarInput: Input<WiitarKeys> {
        static readonly Dictionary<WiitarKeys, Color> colors = new Dictionary<WiitarKeys, Color>() {
            {WiitarKeys.Green, Color.Green},
            {WiitarKeys.Red, Color.Red},
            {WiitarKeys.Yellow, Color.Gold},
            {WiitarKeys.Blue, Color.Blue},
            {WiitarKeys.Orange, Color.Orange},
            {WiitarKeys.Downstrum, Color.Black},
            {WiitarKeys.Start, Color.Gray},
            {WiitarKeys.Select, Color.Gray},
            {WiitarKeys.Upstrum, Color.Black}
        };

        public WiitarInput(WiitarKeys k): base(k) {}

        public override string Source => "Wiitar";

        public override Color DefaultColor => colors[Key];

        protected override char BinaryID => 'w';

        public static WiitarInput FromBinaryDerived(BinaryReader br) {
            return new WiitarInput((WiitarKeys)br.ReadInt32());
        }
    }
}
