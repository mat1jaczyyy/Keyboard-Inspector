using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Keyboard_Inspector {
    static class Utility {
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

        public static void Move<T>(this List<T> list, int from, int to) {
            var i = list[from];
            list.RemoveAt(from);
            list.Insert(to, i);
        }

        public static void SortByKey<T, K>(this List<T> list, Func<T, K> selector) {
            var sorted = list.OrderBy(selector).ToList();
            list.Clear();
            list.AddRange(sorted);
        }

        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> collection)
            => new HashSet<T>(collection);

        public static void ToBinary<T>(this List<T> l, BinaryWriter bw) where T: IBinary {
            bw.Write(l.Count);

            foreach (var i in l)
                i.ToBinary(bw);
        }

        public static List<T> ListFromBinary<T>(BinaryReader br, uint fileVersion, Func<BinaryReader, uint, T> fromBinary) where T: IBinary {
            int n = br.ReadInt32();
            var ret = new List<T>(n);

            for (int i = 0; i < n; i++) {
                ret.Add(fromBinary(br, fileVersion));
            }

            return ret;
        }

        public static void ToBinary<T>(this Dictionary<long, T> d, BinaryWriter bw) where T: IBinary {
            bw.Write(d.Count);

            foreach (var i in d) {
                bw.Write(i.Key);
                i.Value.ToBinary(bw);
            }
        }

        public static Dictionary<long, T> DictionaryFromBinary<T>(BinaryReader br, uint fileVersion, Func<BinaryReader, uint, T> fromBinary) where T: IBinary {
            int n = br.ReadInt32();
            var ret = new Dictionary<long, T>();

            for (int i = 0; i < n; i++) {
                ret.Add(
                    br.ReadInt64(),
                    fromBinary(br, fileVersion)
                );
            }

            return ret;
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
