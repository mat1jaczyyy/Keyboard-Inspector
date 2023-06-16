using System;
using System.Collections.Generic;
using System.IO;
using static Keyboard_Inspector.Native;
using System.Linq;
using System.Text;

namespace Keyboard_Inspector {
    class Source: IBinary {
        public readonly int Count;
        public readonly string Name;
        public readonly string DeviceInterface;

        public Source(int count, string name, string device_interface = "") {
            Count = count;
            Name = name;
            DeviceInterface = device_interface;
        }

        public static Source FromHandle(long handle, int count) {
            IntPtr hDevice = new IntPtr(handle);

            if (!GetRawInputDeviceInfo(hDevice, RawInputCommand.DeviceName, out byte[] interface_bytes)) return null;

            var device_interface = Encoding.UTF8.GetString(interface_bytes.TakeWhile(i => i != 0).ToArray());

            var productString = new StringBuilder(2000);

            var hFile = CreateFile(device_interface, 0, 3, IntPtr.Zero, 3, 0, IntPtr.Zero);
            if (hFile != IntPtr.Zero) {
                try {
                    if (!HidD_GetProductString(hFile, productString, productString.Capacity))
                        return null;

                } finally {
                    CloseHandle(hFile);
                }
            }

            return new Source(count, productString.ToString(), device_interface);
        }

        public override string ToString() => Name;

        public void ToBinary(BinaryWriter bw) {
            bw.Write(Count);
            bw.Write(Name);
            bw.Write(DeviceInterface);
        }

        public static Dictionary<long, Source> DictionaryFromBinary(BinaryReader br, uint fileVersion) {
            int n = br.ReadInt32();
            var ret = new Dictionary<long, Source>();

            for (int i = 0; i < n; i++) {
                ret.Add(br.ReadInt64(),
                    new Source(
                        br.ReadInt32(),
                        br.ReadString(),
                        br.ReadString()
                    )
                );
            }

            return ret;
        }
    }
}
