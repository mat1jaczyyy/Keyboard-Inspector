using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Keyboard_Inspector {
    static partial class WiitarListener {
		static uint last = 0;

		public static bool Process(ref Message m) {
			if (m.Msg != WM_INPUT) return false;

			GetRawInputData(m.LParam, RawInputCommand.Input, null, out int size, Marshal.SizeOf(typeof(RawInputHeader)));
			byte[] input = new byte[size];

			if (GetRawInputData(m.LParam, RawInputCommand.Input, input, out size, Marshal.SizeOf(typeof(RawInputHeader)))) {
				uint inputs = BitConverter.ToUInt16(input, 37);

				for (uint i1 = last, i2 = inputs, n = 0; n < Enum.GetNames(typeof(WiitarKeys)).Length; i1 >>= 1, i2 >>= 1, n++) {
					if ((i1 & 1) != (i2 & 1))
						Recorder.RecordInput((i2 & 1) == 1, new WiitarInput((WiitarKeys)n));
				}

				last = inputs;
			}

			return true;
		}

		static RAWINPUTDEVICE[] deviceList = new RAWINPUTDEVICE[1];

		static void Register(RawInputDeviceFlags flags) {
			deviceList[0].Flags = flags;
			RegisterRawInputDevices(deviceList, deviceList.Length, Marshal.SizeOf(typeof(RAWINPUTDEVICE)));
		}

		public static void Start() => Register(RawInputDeviceFlags.InputSink);
        public static void Stop() => Register(RawInputDeviceFlags.Remove);

		static WiitarListener() {
			deviceList[0].UsagePage = HIDUsagePage.Generic;
			deviceList[0].Usage = HIDUsage.Gamepad;
			deviceList[0].WindowHandle = MainForm.Instance.Handle;
		}
    }
}
