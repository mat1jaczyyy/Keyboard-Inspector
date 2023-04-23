using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Keyboard_Inspector {
    static partial class Listener {
        static readonly ulong[] hat_bm = new ulong[] { 0b0001, 0b0011, 0b0010, 0b0110, 0b0100, 0b1100, 0b1000, 0b1001 };

		static ulong last;
        static double precise;

        static void HandleKeyboard(Native.RawKeyboard input) {
            Recorder.RecordInput(precise, (input.Flags & 1) == 0, new KeyInput((Keys)input.VKey));
        }

        static void HandleMouse(Native.RawMouse input) {
            if (input.ButtonFlags == 0) return;
            int a = 123;
        }

        static void HandleHID(Native.RawHID input) {
            if (!Native.GetRawInputDeviceInfo(input.Header.Device, Native.RawInputCommand.PreparsedData, out byte[] ppd)) return;

            try {
                if (Native.HidP_GetCaps(ppd, out Native.HIDP_CAPS caps) != Native.NTSTATUS.HIDP_STATUS_SUCCESS) return;

                var buttoncaps = new Native.HIDP_BUTTON_CAPS[caps.NumberInputButtonCaps];
                if (Native.HidP_GetButtonCaps(Native.HIDP_REPORT_TYPE.HidP_Input, buttoncaps, ref caps.NumberInputButtonCaps, ppd) != Native.NTSTATUS.HIDP_STATUS_SUCCESS) return;

                var valuecaps = new Native.HIDP_VALUE_CAPS[caps.NumberInputValueCaps];
                if (Native.HidP_GetValueCaps(Native.HIDP_REPORT_TYPE.HidP_Input, valuecaps, ref caps.NumberInputValueCaps, ppd) != Native.NTSTATUS.HIDP_STATUS_SUCCESS) return;

                ulong inputs = 0;

                if (buttoncaps[0].UsagePage == Native.HIDUsagePage.Button) {
                    uint buttons = (uint)(buttoncaps[0].UsageMax - buttoncaps[0].UsageMin + 1);
                    ushort[] list = new ushort[buttons];

                    if (Native.HidP_GetUsages(Native.HIDP_REPORT_TYPE.HidP_Input, buttoncaps[0].UsagePage, 0, list, ref buttons, ppd, input.bRawData, input.dwSizeHid) != Native.NTSTATUS.HIDP_STATUS_SUCCESS) return;

                    for (int i = 0; i < buttons; i++)
                        inputs |= 1UL << (list[i] - buttoncaps[0].UsageMin);
                }

                foreach (var i in valuecaps) {
                    if (i.UsagePage == Native.HIDUsagePage.Generic && i.UsageMin == Native.HIDUsage.HatSwitch) {
                        if (Native.HidP_GetUsageValue(Native.HIDP_REPORT_TYPE.HidP_Input, i.UsagePage, 0, i.UsageMin, out int hat, ppd, input.bRawData, input.dwSizeHid) != Native.NTSTATUS.HIDP_STATUS_SUCCESS) return;                        
                        
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
                            Recorder.RecordInput(precise, i == 1, new GamepadInput((GamepadKeys)n));
                    }

                    last = inputs;
                }

            } finally {
                Native.HidD_FreePreparsedData(ppd);
            }
        }

		public static void Process(ref Message m) {
            if (!Recorder.IsRecording) return;
            precise = Recorder.ElapsedPrecise;

            if (!Native.GetRawInputData(m.LParam, out Native.RawHID raw)) return;

            if (raw.Header.Type == Native.RawInputType.RIM_TYPE_KEYBOARD) {
                if (!Native.GetRawInputData(m.LParam, out Native.RawKeyboard rkb)) return;
                HandleKeyboard(rkb);

            } else if (raw.Header.Type == Native.RawInputType.RIM_TYPE_HID) {
                HandleHID(raw);

            } else if (raw.Header.Type == Native.RawInputType.RIM_TYPE_MOUSE) {
                if (!Native.GetRawInputData(m.LParam, out Native.RawMouse rm)) return;
                HandleMouse(rm);
            }
        }

		static Native.RAWINPUTDEVICE[] deviceList;

	    static void Register(bool enable) {
            for (int i = 0; i < deviceList.Length; i++) {
                deviceList[i].Flags = enable? Native.RawInputDeviceFlags.InputSink : Native.RawInputDeviceFlags.None;
                deviceList[i].WindowHandle = enable? MainForm.Instance.Handle : IntPtr.Zero;
            }
            Native.RegisterRawInputDevices(deviceList, deviceList.Length, Marshal.SizeOf(typeof(Native.RAWINPUTDEVICE)));
	    }

		public static void Start() {
            last = 0;
            Register(true);
        }
        public static void Stop() => Register(false);

		static Listener() {
            deviceList = new Native.RAWINPUTDEVICE[4];

            for (int i = 0; i < deviceList.Length; i++)
                deviceList[i].UsagePage = Native.HIDUsagePage.Generic;

            deviceList[0].Usage = Native.HIDUsage.Keyboard;
            deviceList[1].Usage = Native.HIDUsage.Mouse;
            deviceList[2].Usage = Native.HIDUsage.Joystick;
            deviceList[3].Usage = Native.HIDUsage.Gamepad;
        }
    }
}
