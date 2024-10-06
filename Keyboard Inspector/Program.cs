using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

using DarkUI.Win32;

using FFTW.NET;

namespace Keyboard_Inspector {
    static class Program {
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

        public static List<InputInfo> FrozenInputs { get; private set; }
        public static Dictionary<long, Source> FrozenSources { get; private set; }

        public static bool IsFrozen { get; private set; } = false;

        public static void SetFreeze(bool value) {
            IsFrozen = value;

            bool canFreeze = value && !Result.IsEmpty(Result);

            FrozenInputs = canFreeze? Result.Inputs : null;
            FrozenSources = canFreeze? Result.Sources : null;
        }

        [STAThread]
        static void Main(string[] args) {
            Args = args;

            if (!Directory.Exists(Constants.DataDir))
                Directory.CreateDirectory(Constants.DataDir);

            DFT.Wisdom.Import(Constants.WisdomFile);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            ListenerWindow.Create();

            Application.AddMessageFilter(new ControlScrollFilter());
            Application.Run(new MainForm());
        }
    }
}
