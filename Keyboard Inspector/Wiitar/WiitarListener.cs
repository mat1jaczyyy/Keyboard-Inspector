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

			if (GetRawInputData(m.LParam, out RawInput input)) {
				GetRawInputDeviceInfo(input.Header.Device, RawInputCommand.PreparsedData, out byte[] ppd);

				HidP_GetCaps(ppd, out HIDP_CAPS caps);
                var buttoncaps = new HIDP_BUTTON_CAPS[caps.NumberInputButtonCaps];
                HidP_GetButtonCaps(HIDP_REPORT_TYPE.HidP_Input, buttoncaps, ref caps.NumberInputButtonCaps, ppd);

                if (buttoncaps[0].UsagePage == HIDUsagePage.Button) {
                    uint buttons = (uint)(buttoncaps[0].UsageMax - buttoncaps[0].UsageMin + 1);
					ushort[] list = new ushort[buttons];

					HidP_GetUsages(HIDP_REPORT_TYPE.HidP_Input, buttoncaps[0].UsagePage, 0, list, ref buttons, ppd, input.HID.bRawData, input.HID.dwSizeHid);

                    uint inputs = 0;

					for (int i = 0; i < buttons; i++)
						inputs |= 1U << (list[i] - buttoncaps[0].UsageMin);

                    for (uint i1 = last, i2 = inputs, n = 0; n < Enum.GetNames(typeof(WiitarKeys)).Length; i1 >>= 1, i2 >>= 1, n++) {
                        if ((i1 & 1) != (i2 & 1))
                            Recorder.RecordInput((i2 & 1) == 1, new WiitarInput((WiitarKeys)n));
                    }

                    last = inputs;
                }

                HidD_FreePreparsedData(ppd);
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
			deviceList[0].Usage = HIDUsage.Joystick;
			deviceList[0].WindowHandle = MainForm.Instance.Handle;
		}
    }
}
