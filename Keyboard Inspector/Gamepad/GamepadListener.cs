using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Keyboard_Inspector {
    static partial class GamepadListener {
        static readonly ulong[] hat_bm = new ulong[] { 0b0001, 0b0011, 0b0010, 0b0110, 0b0100, 0b1100, 0b1000, 0b1001 };

		static ulong last;

		static void HandleInput(ref Message m) {
            if (!GetRawInputData(m.LParam, out RawInput input)) return;
            if (!GetRawInputDeviceInfo(input.Header.Device, RawInputCommand.PreparsedData, out byte[] ppd)) return;

            try {
                if (HidP_GetCaps(ppd, out HIDP_CAPS caps) != NTSTATUS.HIDP_STATUS_SUCCESS) return;

                var buttoncaps = new HIDP_BUTTON_CAPS[caps.NumberInputButtonCaps];
                if (HidP_GetButtonCaps(HIDP_REPORT_TYPE.HidP_Input, buttoncaps, ref caps.NumberInputButtonCaps, ppd) != NTSTATUS.HIDP_STATUS_SUCCESS) return;

                var valuecaps = new HIDP_VALUE_CAPS[caps.NumberInputValueCaps];
                if (HidP_GetValueCaps(HIDP_REPORT_TYPE.HidP_Input, valuecaps, ref caps.NumberInputValueCaps, ppd) != NTSTATUS.HIDP_STATUS_SUCCESS) return;

                ulong inputs = 0;

                if (buttoncaps[0].UsagePage == HIDUsagePage.Button) {
                    uint buttons = (uint)(buttoncaps[0].UsageMax - buttoncaps[0].UsageMin + 1);
                    ushort[] list = new ushort[buttons];

                    if (HidP_GetUsages(HIDP_REPORT_TYPE.HidP_Input, buttoncaps[0].UsagePage, 0, list, ref buttons, ppd, input.HID.bRawData, input.HID.dwSizeHid) != NTSTATUS.HIDP_STATUS_SUCCESS) return;

                    for (int i = 0; i < buttons; i++)
                        inputs |= 1UL << (list[i] - buttoncaps[0].UsageMin);
                }

                foreach (var i in valuecaps) {
                    if (i.UsagePage == HIDUsagePage.Generic && (HIDUsage)i.UsageMin == HIDUsage.HatSwitch) {
                        if (HidP_GetUsageValue(HIDP_REPORT_TYPE.HidP_Input, i.UsagePage, 0, i.UsageMin, out int hat, ppd, input.HID.bRawData, input.HID.dwSizeHid) != NTSTATUS.HIDP_STATUS_SUCCESS) return;                        
                        
                        if (i.LogicalMin <= hat && hat <= i.LogicalMax) {
                            if (i.LogicalMax == 3) { // 4-way hat
                                inputs |= 1UL << (hat + 32);

                            } else if (i.LogicalMax == 7) { // 8-way hat
                                inputs |= hat_bm[hat] << 32;
                            }
                        }
                        break;
                    }
                }

                if (last != inputs) {
                    for (int n = 0; n < 36; n++) {
                        ulong i = (inputs >> n) & 1;
                        if (i != ((last >> n) & 1))
                            Recorder.RecordInput(i == 1, new GamepadInput((GamepadKeys)n));
                    }

                    last = inputs;
                }

            } finally {
                HidD_FreePreparsedData(ppd);
            }
        }

		public static bool Process(ref Message m) {
			if (m.Msg != WM_INPUT) return false;

			HandleInput(ref m);
			return true;
		}

		static RAWINPUTDEVICE[] deviceList = new RAWINPUTDEVICE[2];

	    static void Register(RawInputDeviceFlags flags) {
            for (int i = 0; i < deviceList.Length; i++) {
                deviceList[i].Flags = flags;
                deviceList[i].WindowHandle = flags == RawInputDeviceFlags.InputSink? MainForm.Instance.Handle : IntPtr.Zero;
            }
		    RegisterRawInputDevices(deviceList, deviceList.Length, Marshal.SizeOf(typeof(RAWINPUTDEVICE)));
	    }

		public static void Start() {
            last = 0;
            Register(RawInputDeviceFlags.InputSink);
        }
        public static void Stop() => Register(RawInputDeviceFlags.Remove);

		static GamepadListener() {
			deviceList[0].UsagePage = HIDUsagePage.Generic;
			deviceList[0].Usage = HIDUsage.Joystick;
            deviceList[1].UsagePage = HIDUsagePage.Generic;
            deviceList[1].Usage = HIDUsage.Gamepad;
        }
    }
}
