using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace Keyboard_Inspector {
    class ListenerWindow {
        public static ListenerWindow Instance { get; private set; }

        public static void Create() {
            if (Instance != null) throw new Exception("Can't have more than one ListenerWindow");
            Instance = new ListenerWindow();
        }

        Native.RAWINPUTDEVICE[] deviceList;
        ulong last = 0;
        BlockingCollection<UnprocessedInput> queue = new BlockingCollection<UnprocessedInput>();

        const string CLASS_NAME = "ListenerWindow";
        Native.WndProc WndProc;
        IntPtr hWnd;

        ListenerWindow() {
            deviceList = new Native.RAWINPUTDEVICE[4];

            for (int i = 0; i < deviceList.Length; i++)
                deviceList[i].UsagePage = Native.HIDUsagePage.Generic;

            Task.Run(() => {
                foreach (var input in queue.GetConsumingEnumerable()) {
                    last = input.Process(last);
                }
            });

            IntPtr hInstance = Marshal.GetHINSTANCE(typeof(ListenerWindow).Module);
            WndProc = WindowProc;

            Native.WNDCLASSEX wc = Native.WNDCLASSEX.Build();
            wc.lpfnWndProc = Marshal.GetFunctionPointerForDelegate(WndProc);
            wc.hInstance = hInstance;
            wc.lpszClassName = CLASS_NAME;

            Native.RegisterClassEx(ref wc);

            var thread = new Thread(() => {
                // .NET API doesn't expose this
                var nativeThread = Native.GetCurrentThread();
                Native.SetThreadPriority(nativeThread, 15); // THREAD_PRIORITY_TIME_CRITICAL

                hWnd = Native.CreateWindowEx(
                    0, CLASS_NAME, "Keyboard Inspector Listener", 0,
                    Native.CW_USEDEFAULT, Native.CW_USEDEFAULT, Native.CW_USEDEFAULT, Native.CW_USEDEFAULT,
                    IntPtr.Zero, IntPtr.Zero, hInstance, IntPtr.Zero
                );

                if (hWnd == IntPtr.Zero) {
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                }

                Native.MSG msg;
                while (Native.GetMessage(out msg, IntPtr.Zero, 0, 0)) {
                    Native.TranslateMessage(ref msg);
                    Native.DispatchMessage(ref msg);
                }

                throw new Exception("ListenerWindow WM_QUIT");
            });
            thread.Start();
        }

        IntPtr WindowProc(IntPtr hWnd, uint msg, UIntPtr wParam, IntPtr lParam) {
            if (msg == Native.WM_INPUT && Recorder.IsRecording) {
                double time = Recorder.ElapsedPrecise;

                if (Native.GetRawInputData(lParam, out Native.RawHID raw)) {
                    queue.Add(new UnprocessedInput() {
                        Time = time,
                        Raw = raw
                    });
                }

                return IntPtr.Zero;
            }

            return Native.DefWindowProc(hWnd, msg, wParam, lParam);
        }

        void RegisterRawInput(bool enable) {
            int n = 0;

            if (enable) {
                last = 0;

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
                deviceList[i].WindowHandle = enable? hWnd : IntPtr.Zero;
            }

            Native.RegisterRawInputDevices(deviceList, n, Marshal.SizeOf(typeof(Native.RAWINPUTDEVICE)));
        }

        public void Start() => RegisterRawInput(true);
        public void Stop() => RegisterRawInput(false);
    }
}
