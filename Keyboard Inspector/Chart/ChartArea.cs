using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;

namespace Keyboard_Inspector {
    class ChartArea: Control {
        public ChartArea() {
            DoubleBuffered = true;
            ResizeRedraw = true;

            ForeColor = Color.FromArgb(65, 140, 240);

            Scope = new Scope(this, null);
        }

        bool Suspended = false;
        public void SuspendPaint() => Suspended = true;
        public void ResumePaint() {
            Suspended = false;
            Invalidate();
        }

        string _title = "";
        [Category("Appearance")]
        [Description("The title of the chart.")]
        [DefaultValue("")]
        public string Title {
            get => _title;
            set {
                _title = value;
                Invalidate();
            }
        }

        enum Kind {
            None, Line, KeyHistory
        }
        Kind kind = Kind.None;

        bool HasAny => kind != Kind.None;

        double[] LineData;
        bool HasData => LineData != null && LineData.Length > 0;

        double _xfactor = 1;
        double XFactor {
            get => _xfactor;
            set {
                _xfactor = value;
                Invalidate();
            }
        }

        public void LoadData(IEnumerable<double> data, double xfactor = 1) {
            if (data is null) LineData = null;
            else if (data is double[] arr) LineData = arr;
            else LineData = data.ToArray();

            HighlightPoint = -1;

            if (HasData) {
                kind = Kind.Line;
                _xfactor = xfactor;

                YMaxValue = LineData.Max();
                Scope.SetToDefault();

            } else {
                kind = Kind.None;
                _xfactor = 1;
            }

            Invalidate();
        }

        Result KeyHistory;
        List<Input> InputCollection;
        List<Event>[] PerInputEvents;
        Dictionary<Input, int> InputIndexes;

        public void LoadData(Result result) {
            KeyHistory = result;

            _xfactor = 1;
            HighlightPoint = -1;

            if (!Result.IsEmpty(KeyHistory)) {
                kind = Kind.KeyHistory;
                
                InputCollection = KeyHistory.Events.Select(i => i.Input).Distinct().ToList();

                InputIndexes = new Dictionary<Input, int>();
                PerInputEvents = new List<Event>[InputCollection.Count];

                for (int i = 0; i < InputCollection.Count; i++) {
                    InputIndexes[InputCollection[i]] = i;
                    PerInputEvents[i] = new List<Event>(KeyHistory.Events.Count);
                }

                for (int i = 0; i < KeyHistory.Events.Count; i++) {
                    var e = KeyHistory.Events[i];
                    PerInputEvents[InputIndexes[e.Input]].Add(e);
                }

                YMaxValue = InputCollection.Count;
                Scope.SetToDefault();

            } else {
                kind = Kind.None;
            }

            Invalidate();
        }

        double XMax {
            get {
                if (kind == Kind.Line) return LineData.Length - 1;
                else if (kind == Kind.KeyHistory) return KeyHistory.Time;
                return double.NaN;
            }
        }
        public double XMaxFactored {
            get {
                if (kind == Kind.Line) return XMax * XFactor;
                return XMax;
            }
        }
        double YMaxValue;
        bool ShouldFlipY => kind == Kind.Line;
        double FlipY(double y) => ShouldFlipY? YMaxValue - y : y;

        double GetX(int index) {
            if (kind == Kind.Line) return index;
            else if (kind == Kind.KeyHistory) return KeyHistory.Events[index].Time;
            return double.NaN;
        }

        double GetXFactored(int index) {
            if (kind == Kind.Line) return GetX(index) * XFactor;
            return GetX(index);
        }

        double GetY(int index) {
            if (kind == Kind.Line) return LineData[index];
            else if (kind == Kind.KeyHistory) return InputIndexes[KeyHistory.Events[index].Input] + 0.5;
            return double.NaN;
        }

        int BinarySearch(List<Event> events, double x, int ceil) {
            int start = 0;
            int end = events.Count - 1;

            if (events.Count == 0) return -1;

            if (x <= events[start].Time) return start;
            if (events[end].Time <= x) return end;

            while (start <= end) {
                int mid = (start + end) / 2;
                if (mid == 0) return mid + ceil;

                double left = events[mid - 1].Time;
                double right = events[mid].Time;

                if (left == x) return mid - 1;
                if (right == x) return mid;
                if (left < x && x < right) return mid + ceil - 1;

                if (right < x) start = mid + 1;
                else end = mid - 1;
            }
            
            return -1;
        }

        int GetPointLeft(double x) {
            if (kind == Kind.Line) return (int)Math.Floor(x);
            else if (kind == Kind.KeyHistory) return BinarySearch(KeyHistory.Events, x, 0);
            return -1;
        }

        int GetPointRight(double x) {
            if (kind == Kind.Line) return (int)Math.Ceiling(x);
            else if (kind == Kind.KeyHistory) return BinarySearch(KeyHistory.Events, x, 1);
            return -1;
        }

        public readonly Scope Scope;

        protected override void OnMouseWheel(MouseEventArgs e) {
            base.OnMouseWheel(e);

            if (!HasAny) return;

            Units u = GetUnits();

            if (!Scope.ApplyWheel(e.Delta, (e.X - u.Chart.X) / u.Chart.Width)) return;

            HandleMouseMove(e, true);
        }

        int _highlight = -1;
        int HighlightPoint {
            get => _highlight;
            set {
                if (_highlight == value) return;
                _highlight = value;
                if (HasAny) Invalidate();
            }
        }
        const double hoverRadius = 29;

        protected override void OnMouseLeave(EventArgs e) {
            base.OnMouseLeave(e);

            HighlightPoint = -1;
        }

        protected override void OnMouseMove(MouseEventArgs e) {
            base.OnMouseMove(e);

            if (!HasAny) return;

            HandleMouseMove(e, false);
        }

        void HandleMouseMove(MouseEventArgs e, bool remake) {
            Units u = GetUnits();

            double x = (e.X - u.Chart.X) / u.XUnit + u.Min;
            double y = FlipY((e.Y - u.Chart.Y) / u.YUnit);

            // Get X-axis size of hover radius
            double r = hoverRadius / u.XUnit;

            double aspect = u.YUnit / u.XUnit;

            // Discard values that are too far away to match
            int first = GetPointRight(Math.Max(u.Min, x - r));
            int last = GetPointLeft(Math.Min(u.Max, x + r));

            double closest = r * r;
            int win = -1;

            for (int i = first; i <= last; i++) {
                var x2 = GetX(i) - x;
                x2 *= x2;
                var y2 = (GetY(i) - y) * aspect;
                y2 *= y2;
                var d = x2 + y2;

                if (d < closest) {
                    closest = d;
                    win = i;
                }
            }

            HighlightPoint = win;

            if (HighlightPoint == win && remake)
                Invalidate();
        }

        public event EventHandler Spotlight;

        protected override void OnMouseDoubleClick(MouseEventArgs e) {
            base.OnMouseDoubleClick(e);

            if (!HasAny) return;
            if (e.Button != MouseButtons.Left) return;

            Spotlight?.Invoke(Parent, EventArgs.Empty);
        }

        class Units {
            public RectangleF Title, Chart, XAxis, YAxis;
            public double TextHeight, Space, XUnit, YUnit, Min, Max;
            public bool MultipleSources;
            public SizeF[] InputTextSizes;
        }

        Units GetUnits() {
            Units u = new Units();

            using (Graphics gfx = Graphics.FromHwnd(IntPtr.Zero)) {
                u.TextHeight = Math.Ceiling(gfx.MeasureString("Text", Font).Height);

                u.Title = new RectangleF();
                u.Title.Width = ClientRectangle.Width;

                if (!string.IsNullOrWhiteSpace(Title))
                    u.Title.Height = (float)u.TextHeight + 3;

                u.XAxis = new RectangleF();
                u.XAxis.Height = (float)u.TextHeight + 10;
                u.XAxis.Y = ClientRectangle.Y + ClientRectangle.Height - u.XAxis.Height;

                u.YAxis = new RectangleF();
                u.YAxis.X = 20;
                u.YAxis.Y = ClientRectangle.Y + u.Title.Height;
                u.YAxis.Height = ClientRectangle.Height - u.Title.Height - u.XAxis.Height;
            
                if (kind == Kind.KeyHistory) {
                    u.MultipleSources = InputCollection.Select(i => i.Source).Distinct().Count() > 1;
                    u.InputTextSizes = InputCollection.Select(i => gfx.MeasureString(i.ToString(u.MultipleSources), Font)).ToArray();
                    u.YAxis.Width = (float)Math.Ceiling(u.InputTextSizes.Max(i => i.Width));
                }

                u.XAxis.X = u.YAxis.X + u.YAxis.Width;
                if (u.YAxis.Width > 0)
                    u.XAxis.X += 4;

                u.XAxis.Width = ClientRectangle.Width - u.XAxis.X - 20;

                u.Chart = new RectangleF(
                    u.XAxis.X,
                    u.YAxis.Y,
                    u.XAxis.Width,
                    u.YAxis.Height
                );

                u.Space = XMax / Scope.Zoom;
                u.XUnit = u.Chart.Width / u.Space;
                u.YUnit = u.Chart.Height / YMaxValue;

                u.Min = Scope.Viewport * XMax;
                u.Max = Math.Min(XMax, u.Space + u.Min);
            }

            return u;
        }

        // TODO Remove unused
        class ColorSet {
            public Color TextColor, BackColor, LineColor, LowColor, LowTransparentColor, PointColor;
            public Brush TextBrush, BackBrush, LineBrush, LowBrush, LowTransparentBrush, ShadowBrush, ShadowTextBrush;
            public LinearGradientBrush GradientBrush, PointBrush;
            public Pen LinePen, GradientPen, XPen, YPen;
        }

        ColorSet GetDrawing(Units u) {
            ColorSet cs = new ColorSet();

            cs.TextColor = Color.FromArgb(160, 160, 160);
            cs.BackColor = Color.FromArgb((byte)(BackColor.R * 0.9), (byte)(BackColor.G * 0.9), (byte)(BackColor.B * 0.9));
            cs.LineColor = ForeColor.Blend(ForeColor.Blend(cs.BackColor, 0.9), 0.9);
            cs.LowColor = ForeColor.Blend(cs.BackColor, 0.5);
            cs.LowTransparentColor = Color.FromArgb(128, ForeColor);
            cs.PointColor = ForeColor.Blend(Color.White, 0.6);

            cs.TextBrush = new SolidBrush(cs.TextColor);
            cs.BackBrush = new SolidBrush(cs.BackColor);
            cs.LineBrush = new SolidBrush(cs.LineColor);
            cs.LowBrush = new SolidBrush(cs.LowColor);
            cs.LowTransparentBrush = new SolidBrush(cs.LowTransparentColor);

            u.Chart.Inflate(1, 1);
            cs.GradientBrush = new LinearGradientBrush(u.Chart, Color.Transparent, Color.Transparent, LinearGradientMode.Vertical);
            cs.PointBrush = new LinearGradientBrush(u.Chart, Color.Transparent, Color.Transparent, LinearGradientMode.Vertical);
            u.Chart.Inflate(-1, -1);

            var blend = new ColorBlend();
            blend.Positions = new float[] { 0, 0.5f, 1 };
            blend.Colors = new Color[] { cs.LineColor, cs.LineColor, cs.LowColor };
            cs.GradientBrush.InterpolationColors = blend;

            blend.Colors = new Color[] { cs.PointColor, cs.PointColor, cs.LineColor };
            cs.PointBrush.InterpolationColors = blend;

            cs.ShadowBrush = new SolidBrush(Color.FromArgb(200, cs.BackColor));
            cs.ShadowTextBrush = new SolidBrush(Color.FromArgb(224, (byte)(BackColor.R * 0.75), (byte)(BackColor.G * 0.75), (byte)(BackColor.B * 0.75)));

            cs.LinePen = new Pen(cs.LineBrush);
            cs.GradientPen = new Pen(cs.GradientBrush);

            cs.XPen = new Pen(Color.FromArgb(20, 20, 20));
            cs.YPen = new Pen(Color.FromArgb(44, 44, 44));

            return cs;
        }

        protected override void OnPaint(PaintEventArgs e) {
            if (Suspended) return;

            base.OnPaint(e);

            if (!HasAny) return;

            Units u = GetUnits();
            ColorSet cs = GetDrawing(u);

            PointF ToPointF(int index) => new PointF(
                (float)((GetX(index) - u.Min) * u.XUnit - 0.5) + u.Chart.X,
                (float)(FlipY(GetY(index)) * u.YUnit - 0.5) + u.Chart.Y
            );

            Func<int, Color> PointColor = _ => Color.White;

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            /* Title, background, YAxis */
            {
                if (!string.IsNullOrWhiteSpace(Title))
                    e.Graphics.DrawShadowString(Title, Font, cs.TextBrush, cs.ShadowTextBrush, u.Title, true);

                e.Graphics.FillRectangle(cs.BackBrush, u.Chart);

                double start = 0;
                double cutoff = 6;
                double divisor = 5;

                if (kind == Kind.KeyHistory) {
                    start = 0.5; cutoff = divisor = YMaxValue;
                }

                for (double i = start; i < cutoff; i++) {
                    float y = (float)Math.Round(u.Chart.Y + u.Chart.Height * i / divisor);
                    e.Graphics.DrawLine(cs.YPen, u.Chart.X, y, u.Chart.X + u.Chart.Width, y);
                }
            }

            /* XAxis */
            {
                double interval = Scope.GetInterval(XMaxFactored, u.Chart.Width, kind == Kind.Line? 30 : 40, out double px, out double pos);
                double offset = (float)Math.Round(pos * px);
                int posCeil = (int)Math.Ceiling(pos);

                var textRect = new RectangleF(0, u.XAxis.Y + 7, (float)px, (float)u.TextHeight);

                float GetS() => (float)(Math.Round(posCeil * px) - offset);

                for (float s = GetS(); s <= u.Chart.Width + 0.5; posCeil++, s = GetS()) {
                    s = (float)Math.Round(s + u.XAxis.X);

                    e.Graphics.DrawLine(cs.XPen, s, u.Chart.Y, s, u.XAxis.Y + 4);
                    textRect.X = s - textRect.Width / 2;
                    e.Graphics.DrawShadowString($"{interval * posCeil:0.###}", Font, cs.TextBrush, cs.ShadowTextBrush, textRect, true);
                }
            }

            /* Line chart */
            if (kind == Kind.Line) {
                //PointColor = _ => cs.LineColor;

                int first = GetPointLeft(u.Min);
                int last = GetPointRight(u.Max);

                int count = last - first + 1;
                PointF prev = ToPointF(first);

                // https://www.desmos.com/calculator/tc7y1fwscw
                float ptSize = (float)(1.8 / (1 + Math.Pow(Math.E, 9 - u.XUnit)));

                bool useYLine = false;
                float yMin = prev.Y;
                float yMax = prev.Y;

                // https://www.desmos.com/calculator/2bm2zdu40p
                float cutoff = (float)(5 - 3 * Math.Sqrt(1 - Math.Pow(u.XUnit - 1, 4))) / 5;
                float extraDiff = 0;

                var clip = e.Graphics.Clip;
                e.Graphics.Clip = new Region(new RectangleF(u.Chart.X - 0.5f, u.Chart.Y - 0.5f, u.Chart.Width + 1, u.Chart.Height + 1));

                for (int i = 1; i < count; i++) {
                    PointF point = ToPointF(first + i);

                    float diff = extraDiff + point.X - prev.X;

                    if (point.Y < yMin) yMin = point.Y;
                    if (point.Y > yMax) yMax = point.Y;

                    if (diff < cutoff) {
                        useYLine = true;
                        continue;
                    }

                    extraDiff = diff % cutoff;

                    if (useYLine) {
                        e.Graphics.DrawLine(cs.GradientPen, new PointF(prev.X, yMin), new PointF(point.X, yMax));
                        useYLine = false;

                    } else {
                        e.Graphics.DrawLine(cs.GradientPen, prev, point);

                        if (u.XUnit > 5) {
                            if (i == 1 && HighlightPoint != 0)
                                e.Graphics.FillEllipse(cs.PointBrush, prev.X - ptSize / 2, prev.Y - ptSize / 2, ptSize, ptSize);

                            if (first + i != HighlightPoint)
                                e.Graphics.FillEllipse(cs.PointBrush, point.X - ptSize / 2, point.Y - ptSize / 2, ptSize, ptSize);
                        }
                    }

                    if (!useYLine) {
                        yMin = prev.Y;
                        yMax = prev.Y;
                    }
                    prev = point;
                }

                e.Graphics.Clip = clip;
            }

            /* KeyHistory chart */
            if (kind == Kind.KeyHistory) {
                //PointColor = _ => KeyHistory.Events[_].Input.DefaultColor;

                e.Graphics.SmoothingMode = SmoothingMode.HighSpeed;
                
                var blend = new ColorBlend();
                blend.Positions = new float[] { 0f, 0.1f, 0.3f, 1 };

                RectangleF bar = new RectangleF(u.Chart.X, 0, u.Chart.Width, (float)(u.YUnit - 2));

                for (int k = 0; k < InputCollection.Count; k++) {
                    var stringRect = new RectangleF(
                        u.YAxis.X + u.YAxis.Width - u.InputTextSizes[k].Width,
                        (float)(u.Chart.Y + (k + 0.5) * u.YUnit - u.InputTextSizes[k].Height / 2),
                        u.InputTextSizes[k].Width,
                        u.InputTextSizes[k].Height
                    );

                    e.Graphics.DrawShadowString(InputCollection[k].ToString(u.MultipleSources), Font, cs.TextBrush, cs.ShadowTextBrush, stringRect);

                    bar.Y = (float)(u.Chart.Y + k * u.YUnit + 1);
                    LinearGradientBrush brush = new LinearGradientBrush(bar, Color.Transparent, Color.Transparent, LinearGradientMode.Vertical);

                    var color = Color.FromArgb(250, InputCollection[k].DefaultColor);
                    var topColor = Color.FromArgb(240, color.Blend(Color.Black, 0.98));
                    var bottomColor = Color.FromArgb(215, color.Blend(Color.Black, 0.95));

                    blend.Colors = new Color[] { topColor, color, color, bottomColor };
                    brush.InterpolationColors = blend;

                    var events = PerInputEvents[k];

                    int left = BinarySearch(PerInputEvents[k], u.Min, 1);
                    int right = BinarySearch(PerInputEvents[k], u.Max, 0) + 1;

                    void drawBar(double start, double end) {
                        if (start <= u.Min) start = u.Min;
                        if (start >= u.Max) return;
                        if (end <= u.Min) return;
                        if (end >= u.Max) end = u.Max;

                        bar.X = (float)(u.Chart.X + (start - u.Min) * u.XUnit);
                        bar.Width = (float)((end - start) * u.XUnit);

                        e.Graphics.FillRectangle(brush, bar);
                    }

                    int ev = left;
                    if (!events[ev].Pressed) {
                        drawBar(0, events[ev].Time);
                        ev++;
                    }

                    for (; ev < right; ev += 2) {
                        drawBar(
                            events[ev].Time,
                            ev + 1 < right? events[ev + 1].Time : KeyHistory.Time
                        );
                    }
                }

                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            }

            /* Highlight */
            if (HighlightPoint != -1) {
                var point = ToPointF(HighlightPoint);
                var marker = new RectangleF(point.X - 3, point.Y - 3, 6, 6);

                Color color = PointColor(HighlightPoint);

                e.Graphics.FillEllipse(new SolidBrush(Color.FromArgb(128, color)), marker);
                e.Graphics.DrawEllipse(new Pen(color), marker);

                point = new PointF((float)Math.Round(point.X), (float)Math.Round(point.Y));

                string text = $"({GetXFactored(HighlightPoint):0.###}, {GetY(HighlightPoint):0.###})";
                var textSize = e.Graphics.MeasureString(text, Font);
                var textRect = new RectangleF(point.X + 9, point.Y - 1 - (float)Math.Ceiling(textSize.Height), textSize.Width, textSize.Height);

                if (!u.Chart.Contains(textRect)) {
                    float dx = (float)Math.Round((point.X - textRect.X) * 2 - Math.Ceiling(textRect.Width)) + 2;
                    textRect.X += dx;

                    if (!u.Chart.Contains(textRect)) {
                        float dy = (float)Math.Round((point.Y - textRect.Y) * 2 - Math.Ceiling(textRect.Height)) + 3;
                        textRect.Y += dy;

                        if (!u.Chart.Contains(textRect)) {
                            textRect.X -= dx;

                            if (!u.Chart.Contains(textRect))
                                textRect.Y -= dy;
                        }
                    }
                }

                var shadowRect = new RectangleF(textRect.X - 1.75f, textRect.Y - 3, textRect.Width + 2, textRect.Height + 2.5f);

                e.Graphics.FillRectangle(cs.ShadowBrush, shadowRect);
                e.Graphics.DrawShadowString(text, Font, cs.TextBrush, cs.ShadowTextBrush, textRect, true);
            }
        }
    }
}
