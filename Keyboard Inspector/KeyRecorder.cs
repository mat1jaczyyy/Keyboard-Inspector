using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Keyboard_Inspector {
    static class KeyRecorder {
        delegate IntPtr HookFunc(int nCode, IntPtr wParam, IntPtr lParam);

        const int WH_KEYBOARD_LL = 13;
        static IntPtr _hookID = IntPtr.Zero;

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern IntPtr SetWindowsHookEx(int idHook, HookFunc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern IntPtr GetModuleHandle(string lpModuleName);

        static HookFunc hook = Hook;

        static Stopwatch time;
        static List<KeyEvent> keys;

        static double ElapsedPrecise(this Stopwatch sw) => (double)sw.ElapsedTicks / Stopwatch.Frequency;

        static IntPtr Hook(int nCode, IntPtr wParam, IntPtr lParam) {
            if (nCode >= 0)
                keys.Add(new KeyEvent(time.ElapsedPrecise(), (int)wParam, Marshal.ReadInt32(lParam)));

            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }

        static IntPtr SetHook(HookFunc func) {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule) {
                return SetWindowsHookEx(WH_KEYBOARD_LL, func, GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        public static void StartRecording() {
            if (IsRecording)
                StopRecording();

            time?.Stop();
            time = new Stopwatch();
            time.Start();

            keys = new List<KeyEvent>();

            _hookID = SetHook(hook);
        }

        public static double StopRecording() {
            if (!IsRecording) return 0;

            UnhookWindowsHookEx(_hookID);
            _hookID = IntPtr.Zero;

            time.Stop();

            return time.ElapsedPrecise();
        }

        public static bool IsRecording => _hookID != IntPtr.Zero;
        public static IReadOnlyList<KeyEvent> Events => keys;
        public static double Time => time?.ElapsedPrecise()?? 0;
    }
}
