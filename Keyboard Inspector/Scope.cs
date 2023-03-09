using System;

using DarkUI.Controls;

namespace Keyboard_Inspector {
    class Scope {
        public double Zoom { get; private set; }
        public double Viewport { get; private set; }

        public void Reset() {
            Zoom = 1;
            Viewport = 0;
        }

        public Scope(double zoom = 1, double viewport = 0) {
            Zoom = zoom;
            Viewport = viewport;
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
    }
}
