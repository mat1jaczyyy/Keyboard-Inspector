using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keyboard_Inspector {
    interface IBinary {
        void ToBinary(BinaryWriter bw);
    }
}
