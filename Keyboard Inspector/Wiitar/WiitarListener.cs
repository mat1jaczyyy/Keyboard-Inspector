using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Keyboard_Inspector {
    static partial class WiitarListener {
		static uint last = 0;

		static uint SwapBits(uint number, int i, int j) {
			bool bit1 = Convert.ToBoolean((number >> i) & 1);
			bool bit2 = Convert.ToBoolean((number >> j) & 1);

			if (bit1) number |= 1u << j;
			else number &= ~(1u << j);

			if (bit2) number |= 1u << i;
			else number &= ~(1u << i);

			return number;
		}

		public static bool Process(ref Message m) {
			if (m.Msg != WM_INPUT) return false;

			GetRawInputData(m.LParam, RawInputCommand.Input, null, out int size, Marshal.SizeOf(typeof(RawInputHeader)));
			byte[] input = new byte[size];

			if (GetRawInputData(m.LParam, RawInputCommand.Input, input, out size, Marshal.SizeOf(typeof(RawInputHeader)))) {
				uint inputs = 0;

				if (input.Length == 35) // Xplorer
					inputs = SwapBits(SwapBits(input[31], 2, 3), 6, 7) & 0b011011111u | (input[33] == 5? 0b000100000u : (input[33] == 1? 0b100000000u : 0u));

				else if (input.Length == 39) // Wiitar
					inputs = BitConverter.ToUInt16(input, 37);

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
