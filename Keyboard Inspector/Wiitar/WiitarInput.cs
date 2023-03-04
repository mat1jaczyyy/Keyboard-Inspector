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
            {WiitarKeys.Green, Color.FromArgb(0, 142, 0)},
            {WiitarKeys.Red, Color.FromArgb(176, 0, 0)},
            {WiitarKeys.Yellow, Color.Gold},
            {WiitarKeys.Blue, Color.FromArgb(25, 90, 196)},
            {WiitarKeys.Orange, Color.Orange},
            {WiitarKeys.Downstrum, Color.FromArgb(15, 15, 15)},
            {WiitarKeys.Start, Color.DarkGray},
            {WiitarKeys.Select, Color.DarkGray},
            {WiitarKeys.Upstrum, Color.FromArgb(15, 15, 15)}
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
