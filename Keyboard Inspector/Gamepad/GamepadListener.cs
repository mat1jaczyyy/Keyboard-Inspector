using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Keyboard_Inspector {
    static partial class GamepadListener {
		static uint last = 0;

		static void HandleInput(ref Message m) {
            if (!GetRawInputData(m.LParam, out RawInput input)) return;
            if (!GetRawInputDeviceInfo(input.Header.Device, RawInputCommand.PreparsedData, out byte[] ppd)) return;

            try {
                if (HidP_GetCaps(ppd, out HIDP_CAPS caps) != NTSTATUS.HIDP_STATUS_SUCCESS) return;

                var buttoncaps = new HIDP_BUTTON_CAPS[caps.NumberInputButtonCaps];
                if (HidP_GetButtonCaps(HIDP_REPORT_TYPE.HidP_Input, buttoncaps, ref caps.NumberInputButtonCaps, ppd) != NTSTATUS.HIDP_STATUS_SUCCESS) return;

                if (buttoncaps[0].UsagePage != HIDUsagePage.Button) return;

                uint buttons = (uint)(buttoncaps[0].UsageMax - buttoncaps[0].UsageMin + 1);
                ushort[] list = new ushort[buttons];

                if (HidP_GetUsages(HIDP_REPORT_TYPE.HidP_Input, buttoncaps[0].UsagePage, 0, list, ref buttons, ppd, input.HID.bRawData, input.HID.dwSizeHid) != NTSTATUS.HIDP_STATUS_SUCCESS) return;

                uint inputs = 0;

                for (int i = 0; i < buttons; i++)
                    inputs |= 1U << (list[i] - buttoncaps[0].UsageMin);

                if (last != inputs) {
                    for (int n = 0; n < 32; n++) {
                        uint i = (inputs >> n) & 1;
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

		public static void Start() => Register(RawInputDeviceFlags.InputSink);
        public static void Stop() => Register(RawInputDeviceFlags.Remove);

		static GamepadListener() {
			deviceList[0].UsagePage = HIDUsagePage.Generic;
			deviceList[0].Usage = HIDUsage.Joystick;
            deviceList[1].UsagePage = HIDUsagePage.Generic;
            deviceList[1].Usage = HIDUsage.Gamepad;
        }
    }
}
