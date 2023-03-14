using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Keyboard_Inspector {
    static class Program {
        public static string[] Args = null;

        [STAThread]
        static void Main(string[] args) {
            Args = args;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }

        public static void ToBinary<T>(this ReadOnlyCollection<T> c, BinaryWriter bw) where T: IBinary {
            bw.Write(c.Count);
            
            foreach (var i in c)
                i.ToBinary(bw);
        }
    }
}
