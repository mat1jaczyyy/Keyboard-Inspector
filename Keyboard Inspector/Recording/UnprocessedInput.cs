using System;
using System.Windows.Forms;

namespace Keyboard_Inspector {
    struct UnprocessedInput {
        public double Time;
        public Native.RawHID Raw;

        static readonly ulong[] hat_bm = new ulong[] { 0b0001, 0b0011, 0b0010, 0b0110, 0b0100, 0b1100, 0b1000, 0b1001 };

        void ProcessKeyboard() {
            var input = Raw.CopyMemoryTo<Native.RawKeyboard>();

            // https://stackoverflow.com/a/71885051
            if (input.MakeCode == Native.KEYBOARD_OVERRUN_MAKE_CODE) return;
            if (input.VKey >= 0xFF) return;

            ushort scanCode = input.MakeCode;
            Keys vkCode = (Keys)input.VKey;

            if (input.Flags.HasFlag(Native.RawKeyboardFlags.RI_KEY_E0))
                scanCode |= 0xE000;

            if (input.Flags.HasFlag(Native.RawKeyboardFlags.RI_KEY_E1))
                scanCode |= 0xE100;

            if (vkCode == Keys.ShiftKey || vkCode == Keys.ControlKey || vkCode == Keys.Menu)
                vkCode = (Keys)(ushort)Native.MapVirtualKey(scanCode, Native.MAPVK_VSC_TO_VK_EX);

            Recorder.RecordInput(Time, !input.Flags.HasFlag(Native.RawKeyboardFlags.RI_KEY_BREAK), new Input(vkCode.ToString(), input.Header.Device));
        }

        void ProcessMouse() {
            var input = Raw.CopyMemoryTo<Native.RawMouse>();

            if (input.ButtonFlags == 0) return;

            for (int i = 0; i < 10; i++) {
                if (((input.ButtonFlags >> i) & 1) == 1)
                    Recorder.RecordInput(Time, (i & 1) == 0, new Input(((MouseKeys)(i >> 1)).ToString(), input.Header.Device));
            }
        }

        void ProcessHID(ref ulong last) {
            if (!Native.GetRawInputDeviceInfo(Raw.Header.Device, Native.RawInputCommand.PreparsedData, out byte[] ppd)) return;

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

                    if (Native.HidP_GetUsages(Native.HIDP_REPORT_TYPE.HidP_Input, buttoncaps[0].UsagePage, 0, list, ref buttons, ppd, Raw.bRawData, Raw.dwSizeHid) != Native.NTSTATUS.HIDP_STATUS_SUCCESS) return;

                    for (int i = 0; i < buttons; i++)
                        inputs |= 1UL << (list[i] - buttoncaps[0].UsageMin);
                }

                foreach (var i in valuecaps) {
                    if (i.UsagePage == Native.HIDUsagePage.Generic && i.UsageMin == Native.HIDUsage.HatSwitch) {
                        if (Native.HidP_GetUsageValue(Native.HIDP_REPORT_TYPE.HidP_Input, i.UsagePage, 0, i.UsageMin, out int hat, ppd, Raw.bRawData, Raw.dwSizeHid) != Native.NTSTATUS.HIDP_STATUS_SUCCESS) return;

                        if (hat.InRangeII(i.LogicalMin, i.LogicalMax)) {
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
                            Recorder.RecordInput(Time, i == 1, new Input(((GamepadKeys)n).ToString(), Raw.Header.Device));
                    }

                    last = inputs;
                }

            } finally {
                Native.HidD_FreePreparsedData(ppd);
            }
        }

        public ulong Process(ulong last) {
            if (Raw.Header.Size == 0) Console.WriteLine($"dropped input? {Time}");

            if (Raw.Header.Type == Native.RawInputType.RIM_TYPE_KEYBOARD)
                ProcessKeyboard();

            else if (Raw.Header.Type == Native.RawInputType.RIM_TYPE_MOUSE)
                ProcessMouse();

            else if (Raw.Header.Type == Native.RawInputType.RIM_TYPE_HID)
                ProcessHID(ref last);

            return last;
        }
    }
}
