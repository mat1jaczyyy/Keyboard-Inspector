using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

using FFTW.NET;

namespace Keyboard_Inspector {
    static class Program {
        public static readonly string DataDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Keyboard Inspector");
        public static readonly string WisdomFile = Path.Combine(DataDir, "wisdom");

        public static string[] Args = null;

        [STAThread]
        static void Main(string[] args) {
            Args = args;

            if (!Directory.Exists(DataDir))
                Directory.CreateDirectory(DataDir);

            DFT.Wisdom.Import(WisdomFile);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }

        public static void ToBinary<T>(this ReadOnlyCollection<T> c, BinaryWriter bw) where T: IBinary {
            bw.Write(c.Count);
            
            foreach (var i in c)
                i.ToBinary(bw);
        }

        public static int GetLastIndex(this Chart c)
            => (int)c.Series[0].Tag;

        public static DataPoint GetLast(this Chart c)
            => c.Series[0].Points[c.GetLastIndex()];
    }
}
