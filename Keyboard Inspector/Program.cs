using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

using DarkUI.Win32;

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
            Application.AddMessageFilter(new ControlScrollFilter());
            Application.Run(new MainForm());
        }

        public static void ToBinary<T>(this List<T> l, BinaryWriter bw) where T: IBinary {
            bw.Write(l.Count);
            
            foreach (var i in l)
                i.ToBinary(bw);
        }

        public static Color Blend(this Color color, Color backColor, double amount) {
            byte r = (byte)(color.R * amount + backColor.R * (1 - amount));
            byte g = (byte)(color.G * amount + backColor.G * (1 - amount));
            byte b = (byte)(color.B * amount + backColor.B * (1 - amount));
            return Color.FromArgb(r, g, b);
        }

        public static void DrawShadowString(this Graphics g, string text, Font font, Brush textBrush, Brush shadowBrush, RectangleF rect, bool center = false) {
            StringFormat format = center? new StringFormat() { Alignment = StringAlignment.Center } : null;

            rect.Offset(-1, -1);
            g.DrawString(text, font, shadowBrush, rect, format);
            rect.Offset(2, 2);
            g.DrawString(text, font, shadowBrush, rect, format);
            rect.Offset(-1, -1);
            g.DrawString(text, font, textBrush, rect, format);
        }
    }
}
