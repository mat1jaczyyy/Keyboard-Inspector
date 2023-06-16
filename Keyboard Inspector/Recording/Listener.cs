using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Keyboard_Inspector {
    enum GamepadKeys {
        Button1, Button2, Button3, Button4, Button5, Button6, Button7, Button8,
        Button9, Button10, Button11, Button12, Button13, Button14, Button15, Button16,
        Button17, Button18, Button19, Button20, Button21, Button22, Button23, Button24,
        Button25, Button26, Button27, Button28, Button29, Button30, Button31, Button32,
        HatN, HatE, HatS, HatW
    }

    enum MouseKeys {
        LeftClick, RightClick, MiddleClick, MouseBack, MouseForward
    }

    static partial class Listener {
        static readonly ulong[] hat_bm = new ulong[] { 0b0001, 0b0011, 0b0010, 0b0110, 0b0100, 0b1100, 0b1000, 0b1001 };

		static ulong last;
        static double precise;

        static void HandleKeyboard(Native.RawKeyboard input) {
            // todo fix vkey right shift?
            Recorder.RecordInput(precise, (input.Flags & 1) == 0, new Input(((Keys)input.VKey).ToString(), input.Header.Device));
        }

        static void HandleMouse(Native.RawMouse input) {
            if (input.ButtonFlags == 0) return;

            for (int i = 0; i < 10; i++) {
                if (((input.ButtonFlags >> i) & 1) == 1)
                    Recorder.RecordInput(precise, (i & 1) == 0, new Input(((MouseKeys)(i >> 1)).ToString(), input.Header.Device));
            }
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
                            int size = i.LogicalMax - i.LogicalMin + 1;
                            hat -= i.LogicalMin;

                            if (size == 4) { // 4-way hat
                                inputs |= 1UL << (hat + 32);

                            } else if (size == 8) { // 8-way hat
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
                            Recorder.RecordInput(precise, i == 1, new Input(((GamepadKeys)n).ToString(), input.Header.Device));
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
            int n = 0;

            if (enable) {
                if (MainForm.Instance.captureKeyboard.Checked) {
                    deviceList[n++].Usage = Native.HIDUsage.Keyboard;
                }

                if (MainForm.Instance.captureGamepad.Checked) {
                    deviceList[n++].Usage = Native.HIDUsage.Gamepad;
                    deviceList[n++].Usage = Native.HIDUsage.Joystick;
                }

                if (MainForm.Instance.captureMouse.Checked) {
                    deviceList[n++].Usage = Native.HIDUsage.Mouse;
                }

            } else n = deviceList.Length;

            for (int i = 0; i < n; i++) {
                deviceList[i].Flags = enable? Native.RawInputDeviceFlags.InputSink : Native.RawInputDeviceFlags.None;
                deviceList[i].WindowHandle = enable? MainForm.Instance.Handle : IntPtr.Zero;
            }

            Native.RegisterRawInputDevices(deviceList, n, Marshal.SizeOf(typeof(Native.RAWINPUTDEVICE)));
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
        }
    }
}
