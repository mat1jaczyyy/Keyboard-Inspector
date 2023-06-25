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

        public static void InvokeIfRequired(this Form form, Action action) {
            if (form.InvokeRequired) form.Invoke(action);
            else action();
        }

        public static T InvokeIfRequired<T>(this Form form, Func<T> action) {
            if (form.InvokeRequired) return (T)form.Invoke(action);
            else return action();
        }

        public static bool InRangeII<T>(this T val, T min, T max) where T: IComparable<T>
            => min.CompareTo(val) <= 0 && val.CompareTo(max) <= 0;

        public static bool InRangeIE<T>(this T val, T min, T max) where T: IComparable<T>
            => min.CompareTo(val) <= 0 && val.CompareTo(max) < 0;

        public static T Clamp<T>(this T val, T min, T max) where T: IComparable<T> {
            if (val.CompareTo(min) < 0) return min;
            if (val.CompareTo(max) > 0) return max;
            return val;
        }

        public static bool ContainsIX(this RectangleF rect, Point point)
            => rect.Left <= point.X && point.X <= rect.Right &&
               rect.Top <= point.Y && point.Y < rect.Bottom;

        public static void ToBinary<T>(this List<T> l, BinaryWriter bw) where T: IBinary {
            bw.Write(l.Count);
            
            foreach (var i in l)
                i.ToBinary(bw);
        }

        public static void ToBinary<T>(this Dictionary<long, T> d, BinaryWriter bw) where T: IBinary {
            bw.Write(d.Count);

            foreach (var i in d) {
                bw.Write(i.Key);
                i.Value.ToBinary(bw);
            }
        }

        public static double Blend(this double val, double backVal, double amount) {
            if (amount <= 0) return backVal;
            if (amount >= 1) return val;
            return val * amount + backVal * (1 - amount);
        }

        public static Color Blend(this Color color, Color backColor, double amount) {
            byte r = (byte)(color.R * amount + backColor.R * (1 - amount));
            byte g = (byte)(color.G * amount + backColor.G * (1 - amount));
            byte b = (byte)(color.B * amount + backColor.B * (1 - amount));
            return Color.FromArgb(r, g, b);
        }

        public static Color WithAlpha(this Color color, byte alpha)
            => Color.FromArgb(alpha, color);

        public static void DrawShadowString(this Graphics g, string text, Font font, Brush textBrush, Brush shadowBrush, RectangleF rect, bool center = false) {
            StringFormat format = center? new StringFormat() { Alignment = StringAlignment.Center } : null;

            rect.Offset(-1, -1);
            g.DrawString(text, font, shadowBrush, rect, format);
            rect.Offset(2, 2);
            g.DrawString(text, font, shadowBrush, rect, format);
            rect.Offset(-1, -1);
            g.DrawString(text, font, textBrush, rect, format);
        }

        public static void AutoSeparators(this ToolStripItemCollection items) {
            bool has = false;

            foreach (ToolStripItem item in items) {
                if (item is ToolStripMenuItem) {
                    has |= item.Available;
                
                } else if (item is ToolStripSeparator) {
                    item.Available = has;
                    has = false;
                }
            }
        }
    }
}
