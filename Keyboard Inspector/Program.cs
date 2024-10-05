using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

using DarkUI.Win32;

using FFTW.NET;

namespace Keyboard_Inspector {
    static class Program {
        public static readonly string DataDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Keyboard Inspector");
        public static readonly string WisdomFile = Path.Combine(DataDir, "wisdom");

        public static string[] Args = null;

        static bool _cursorVisible = true;
        public static bool CursorVisible {
            get => _cursorVisible;
            set {
                if (_cursorVisible == value) return;

                if (value)
                    Cursor.Show();
                else
                    Cursor.Hide();

                _cursorVisible = value;
            }
        }

        public static Result Result;

        public static List<InputInfo> Frozen { get; private set; }
        public static bool IsFrozen => Frozen != null;

        public static void Freeze() {
            Frozen = Result.Inputs;
        }

        public static void Unfreeze() {
            Frozen = null;
        }

        [STAThread]
        static void Main(string[] args) {
            Args = args;

            if (!Directory.Exists(DataDir))
                Directory.CreateDirectory(DataDir);

            DFT.Wisdom.Import(WisdomFile);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            ListenerWindow.Create();

            Application.AddMessageFilter(new ControlScrollFilter());

            Application.Idle += OnIdle;

            Application.Run(new MainForm());
        }

        static void OnIdle(object sender, EventArgs e) {
            while (!PeekMessage(out _, IntPtr.Zero, 0, 0, 0)) {
                if (!Recorder.IsRecording) return;
                MainForm.Instance.RefreshResult();
            }
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool PeekMessage(out Message msg, IntPtr hWnd, uint messageFilterMin, uint messageFilterMax, uint flags);
    }
}
