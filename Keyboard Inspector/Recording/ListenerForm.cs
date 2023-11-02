using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

using DarkUI.Forms;

namespace Keyboard_Inspector {
    partial class ListenerForm: DarkForm {
        public static ListenerForm Instance { get; private set; }

        // hiding from Alt+TAB dialog
        protected override CreateParams CreateParams {
            get {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x80;
                return cp;
            }
        }

        Native.RAWINPUTDEVICE[] deviceList;

        public void RegisterRawInput(bool enable) {
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
                deviceList[i].WindowHandle = enable? Handle : IntPtr.Zero;
            }

            Native.RegisterRawInputDevices(deviceList, n, Marshal.SizeOf(typeof(Native.RAWINPUTDEVICE)));
        }

        public ListenerForm() {
            if (Instance != null) throw new Exception("Can't have more than one ListenerForm");
            Instance = this;

            InitializeComponent();

            deviceList = new Native.RAWINPUTDEVICE[4];

            for (int i = 0; i < deviceList.Length; i++)
                deviceList[i].UsagePage = Native.HIDUsagePage.Generic;
        }
    }
}
