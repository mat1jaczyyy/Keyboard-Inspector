using System;
using System.Drawing;
using System.Linq;

namespace Keyboard_Inspector {
    class LinearGradient {
        Tuple<double, Color>[] colors;

        public LinearGradient(params Tuple<double, Color>[] colors) {
            this.colors = colors.OrderBy(c => c.Item1).ToArray();
        }

        public Color Get(double x) {
            if (x <= colors.First().Item1) return colors.First().Item2;
            if (x >= colors.Last().Item1) return colors.Last().Item2;

            Tuple<double, Color> lower = null, upper = null;

            for (int i = 0; i < colors.Length - 1; i++) {
                if (x >= colors[i].Item1 && x <= colors[i + 1].Item1) {
                    lower = colors[i];
                    upper = colors[i + 1];
                    break;
                }
            }

            if (lower == null || upper == null) return Color.Black;

            double range = upper.Item1 - lower.Item1;
            double t = (x - lower.Item1) / range;

            return upper.Item2.Blend(lower.Item2, t);
        }
    }
}
