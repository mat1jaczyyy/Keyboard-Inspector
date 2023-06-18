using System;
using System.Collections.Generic;
using System.IO;
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

        static bool CfgMgrName(string interface_name, out string name, out string instance_name) {
            name = null;
            instance_name = null;

            uint type;
            int size = 0;

            int ret = Native.CM_Get_Device_Interface_Property(
                interface_name,
                ref Native.DEVPKEY_Device_InstanceId,
                out type,
                null, ref size, 0
            );
            if (ret != Native.CR_BUFFER_SMALL || type != Native.DEVPROP_TYPE_STRING) return false;

            StringBuilder buffer = new StringBuilder(size);

            ret = Native.CM_Get_Device_Interface_Property(
                interface_name,
                ref Native.DEVPKEY_Device_InstanceId,
                out type,
                buffer, ref size, 0
            );
            if (ret != Native.CR_SUCCESS || type != Native.DEVPROP_TYPE_STRING) return false;

            instance_name = buffer.ToString();

            ret = Native.CM_Locate_DevNode(out int devInst, instance_name, Native.Locate_DevNode_Flags.CM_LOCATE_DEVNODE_PHANTOM);
            if (ret != 0) return false;

            size = 0;

            ret = Native.CM_Get_DevNode_Property(
                devInst,
                ref Native.DEVPKEY_Device_BusTypeGuid,
                out type,
                null, ref size, 0
            );
            if (ret != Native.CR_BUFFER_SMALL || type != Native.DEVPROP_TYPE_GUID) return false;

            Guid guid = new Guid();

            ret = Native.CM_Get_DevNode_Property(
                devInst,
                ref Native.DEVPKEY_Device_BusTypeGuid,
                out type,
                ref guid, ref size, 0
            );
            if (ret != Native.CR_SUCCESS || type != Native.DEVPROP_TYPE_GUID) return false;

            if (guid == Native.GUID_BUS_TYPE_HID) return false;

            size = 0;

            ret = Native.CM_Get_DevNode_Property(
                devInst,
                ref Native.DEVPKEY_NAME,
                out type,
                null, ref size, 0
            );
            if (ret != Native.CR_BUFFER_SMALL || type != Native.DEVPROP_TYPE_STRING) return false;

            buffer = new StringBuilder(size);

            ret = Native.CM_Get_DevNode_Property(
                devInst,
                ref Native.DEVPKEY_NAME,
                out type,
                buffer, ref size, 0
            );
            if (ret != Native.CR_SUCCESS || type != Native.DEVPROP_TYPE_STRING) return false;

            name = buffer.ToString();
            return true;
        }

        static bool HIDProductName(string interface_name, out string name) {
            var productString = new StringBuilder(2000);
            name = null;

            var hFile = Native.CreateFile(interface_name, 0, 3, IntPtr.Zero, 3, 0, IntPtr.Zero);
            if (hFile != IntPtr.Zero) {
                try {
                    if (!Native.HidD_GetProductString(hFile, productString, productString.Capacity))
                        return false;

                } finally {
                    Native.CloseHandle(hFile);
                }
            }

            name = productString.ToString();
            return true;
        }

        public static Source FromHandle(long handle, int count) {
            string name = "Unknown device";
            string interface_name = $"hDevice=0x{handle:X08}";

            if (Native.GetRawInputDeviceInfo(new IntPtr(handle), Native.RawInputCommand.DeviceName, out byte[] interface_bytes)) {
                interface_name = Encoding.UTF8.GetString(interface_bytes.TakeWhile(i => i != 0).ToArray());
            
                if (!CfgMgrName(interface_name, out name, out string instance_name) &&
                    !HIDProductName(interface_name, out name)
                ) {
                    name = instance_name?? interface_name;
                }
            }

            return new Source(count, name, interface_name);
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
