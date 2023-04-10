using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Keyboard_Inspector {
    static partial class KeyListener {
        const int MASK = 0xFFFB;

        static HOOKFUNC hook = Hook;

        static IntPtr Hook(int nCode, IntPtr wParam, IntPtr lParam) {
            if (nCode >= 0)
                Recorder.RecordInput(((int)wParam & MASK) == WM_KEYDOWN, new KeyInput((Keys)Marshal.ReadInt32(lParam)));

            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }

        static IntPtr _hookID = IntPtr.Zero;

        public static void Start() {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule) {
                _hookID = SetWindowsHookEx(WH_KEYBOARD_LL, hook, GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        public static void Stop() {
            UnhookWindowsHookEx(_hookID);
            _hookID = IntPtr.Zero;
        }
    }
}
