using System;
using System.Windows.Forms.DataVisualization.Charting;
using DarkUI.Controls;

namespace Keyboard_Inspector {
    class Scope {
        public double Zoom { get; private set; }
        public double Viewport { get; private set; }
        readonly Func<int, double> IntervalGenerator;
        public string BaseTitle = null;
        public string SecondaryTitle = null;
        public Action<Scope> Default = i => i.Reset();
        public Chart Chart;

        public void SetToDefault() => Default(this);

        public void Reset() {
            Zoom = 1;
            Viewport = 0;
        }

        public Scope(Func<int, double> intervalGenerator) {
            Reset();
            IntervalGenerator = intervalGenerator?? (i => Math.Pow(2, i - 10));
        }

        public void ScrollBar(DarkScrollBar scroll)
            => Viewport = (double)scroll.Value / scroll.Maximum;

        public bool ApplyWheel(double delta, double x) {
            if (x < 0 || 1 <= x) return false;

            double s = 1 / Zoom;

            double change = Math.Pow(1.05, delta / 120.0);
            Zoom *= change;

            if (Zoom <= 1) {
                Zoom = 1;
                Viewport = 0;

            } else {
                if (Zoom > 200000) {
                    Zoom = 200000;
                    change = Zoom * s;
                }

                double v = x * (1 - 1 / change);
                Viewport += v * s;

                if (Viewport < 0) Viewport = 0;
                else {
                    double t = 1 / Zoom;
                    if (Viewport + t > 1) Viewport = 1 - t;
                }
            }

            return true;
        }

        public double GetInterval(double xMax, double areaWidth, out double px) {
            int incIndex = 4;
            double interval;

            for (;;) {
                interval = IntervalGenerator(incIndex);

                px = interval / xMax * areaWidth * Zoom;

                if (px < 30) incIndex++;
                else if (px >= 90 && incIndex > 0) incIndex--;
                else break;
            }

            return interval;
        }

        public void SetBetween(double lo, double hi, double max) {
            Zoom = max / (hi - lo);
            Viewport = lo / max;
        }
    }
}
