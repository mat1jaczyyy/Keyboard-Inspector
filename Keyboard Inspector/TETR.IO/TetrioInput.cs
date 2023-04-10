using System.Drawing;
using System.IO;

namespace Keyboard_Inspector {
    enum TetrioKeys {
        moveLeft = 0,
        moveRight = 1,
        softDrop = 2,
        hardDrop = 3,
        rotateCCW = 4,
        rotateCW = 5,
        rotate180 = 6,
        hold = 7
    };

    class TetrioInput: Input<TetrioKeys> {
        public TetrioInput(TetrioKeys k): base(k) {}

        public override string Source => "TETR.IO Replay";

        public override Color DefaultColor => Key == TetrioKeys.hardDrop? Color.Goldenrod : Color.DarkGray;

        protected override char BinaryID => 't';

        public static TetrioInput FromBinaryDerived(BinaryReader br, uint fileVersion) {
            return new TetrioInput((TetrioKeys)br.ReadInt32());
        }
    }
}
