using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

using DarkUI.Win32;

using FFTW.NET;

namespace Keyboard_Inspector {
    static class Program {
        public static readonly string DataDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Keyboard Inspector");
        public static readonly string WisdomFile = Path.Combine(DataDir, "wisdom");

        public static string[] Args = null;

        class InputMessageFilter: IMessageFilter {
            const int WM_INPUT = 0x00FF;

            public bool PreFilterMessage(ref Message m) {
                if (m.Msg == WM_INPUT) {
                    Listener.Process(ref m);
                    return true;
                }

                return false;
            }
        }

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

        public static List<InputInfo> FrozenInputs { get; private set; }
        public static Dictionary<long, Source> FrozenSources { get; private set; }

        public static bool IsFrozen => FrozenInputs != null;

        public static void Freeze() {
            FrozenInputs = Result.Inputs;
            FrozenSources = Result.Sources;
        }

        public static void Unfreeze() {
            FrozenInputs = null;
            FrozenSources = null;
        }

        [STAThread]
        static void Main(string[] args) {
            Args = args;

            if (!Directory.Exists(DataDir))
                Directory.CreateDirectory(DataDir);

            DFT.Wisdom.Import(WisdomFile);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.AddMessageFilter(new InputMessageFilter());
            Application.AddMessageFilter(new ControlScrollFilter());

            Application.Run(new MainForm());
        }
    }
}
