using System.IO;

namespace Keyboard_Inspector {
    interface IBinary {
        void ToBinary(BinaryWriter bw);
    }

    class IBinaryException: IOException {
        public IBinaryException(string message): base(message) {}
    }
}
