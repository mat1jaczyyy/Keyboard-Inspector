using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keyboard_Inspector {
    class Analysis: IBinary {
        const int pLow = 20;
        const int pHigh = 128000;

        int _precision = 4000;
        public int Precision {
            get => _precision;
            set => _precision = Math.Min(pHigh, Math.Max(0, value));
        }
        public bool PrecisionValid => Precision >= pLow;

        int _hps = 0;
        public int HPS {
            get => _hps;
            set => _hps = Math.Min(9, Math.Max(0, value));
        }

        public bool LowCut = true;

        public void ToBinary(BinaryWriter bw) {
            bw.Write(Precision);
            bw.Write(HPS);
            bw.Write(LowCut);
        }

        public static Analysis FromBinary(BinaryReader br, uint fileVersion) {
            var ret = new Analysis();

            ret.Precision = br.ReadInt32();
            ret.HPS = br.ReadInt32();
            ret.LowCut = br.ReadBoolean();

            return ret;
        }
    }
}
