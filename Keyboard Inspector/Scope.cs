using System;
using System.Windows.Forms;

using DarkUI.Controls;

namespace Keyboard_Inspector {
    class Scope {
        public double Zoom { get; private set; }
        public double Viewport { get; private set; }
        public Func<int, double> IntervalGenerator = i => Math.Pow(2, i - 10);
        public Action<Scope> Default = i => i.Reset();
        public Control Control { get; private set; }

        DarkScrollBar _scroll;
        public DarkScrollBar ScrollBar {
            get => _scroll;
            set {
                if (_scroll != null)
                    _scroll.ValueChanged -= Scrolled;

                _scroll = value;

                if (_scroll != null)
                    _scroll.ValueChanged += Scrolled;

                UpdateScroll();
            }
        }

        public Scope(Control control, DarkScrollBar scroll) {
            Control = control;
            ScrollBar = scroll;

            Reset();
        }

        public Scope(Control control, DarkScrollBar scroll, Func<int, double> intervalGenerator) : this(control, scroll)
            => IntervalGenerator = intervalGenerator ?? (i => Math.Pow(2, i - 10));

        void Scrolled(object sender, EventArgs e) {
            if (updatingScroll) return;

            Viewport = (double)ScrollBar.Value / ScrollBar.Maximum;
            Control?.Invalidate();
        }

        bool updatingScroll = false;

        public void UpdateScroll() {
            if (ScrollBar == null) return;

            try {
                updatingScroll = true;
                ScrollBar.ViewSize = (int)(ScrollBar.Maximum / Zoom);

                if (ScrollBar.ViewSize == ScrollBar.Maximum)
                    ScrollBar.ViewSize--;

                ScrollBar.Value = (int)(ScrollBar.Maximum * Viewport);

            } finally {
                updatingScroll = false;
            }
        }

        public void SetToDefault() => Default(this);

        public void Reset() {
            Zoom = 1;
            Viewport = 0;

            Control?.Invalidate();
            UpdateScroll();
        }

        public void SetBetween(double lo, double hi, double max) {
            Zoom = max / (hi - lo);
            Viewport = lo / max;

            Control?.Invalidate();
            UpdateScroll();
        }

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

            Control?.Invalidate();
            UpdateScroll();

            return true;
        }

        public double GetInterval(double xMax, double areaWidth, out double px, out double pos) {
            int incIndex = 0;
            double interval;

            for (;;) {
                interval = IntervalGenerator(incIndex);

                px = interval / xMax * areaWidth * Zoom;

                if (px < 30) incIndex++;
                else if (px >= 90 && incIndex > 0) incIndex--;
                else break;
            }

            pos = Viewport * xMax / interval;

            return interval;
        }
    }
}
