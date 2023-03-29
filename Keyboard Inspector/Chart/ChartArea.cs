using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

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

        double[] Data;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Bindable(false)]
        [Browsable(false)]
        public bool HasData => Data != null && Data.Length > 0;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Bindable(false)]
        [Browsable(false)]
        public int LastIndex => Data.Length - 1;
        
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Bindable(false)]
        [Browsable(false)]

        public double XMaxValue => LastIndex * XFactor;
        
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Bindable(false)]
        [Browsable(false)]
        public double YMaxValue { get; private set; }

        double FlipY(double y) => YMaxValue - y;

        double _xfactor = 1;
        [Category("Data")]
        [Description("The factor with which the X-axis is scaled/multiplied.")]
        [DefaultValue(1)]
        public double XFactor {
            get => _xfactor;
            set {
                _xfactor = value;
                Invalidate();
            }
        }

        public void LoadData(IEnumerable<double> data, double xfactor = 1) {
            if (data is null) Data = null;
            else if (data is double[] arr) Data = arr;
            else Data = data.ToArray();

            _xfactor = xfactor;
            HighlightPoint = -1;

            if (HasData) {
                YMaxValue = Data.Max();
                Scope.SetToDefault();
            }

            Invalidate();
        }

        string _title = "";
        [Category("Appearance")]
        [Description("The title of the chart.")]
        [DefaultValue(1)]
        public string Title {
            get => _title;
            set {
                _title = value;
                Invalidate();
            }
        }

        struct Units {
            public RectangleF Title, Chart, XAxis;
            public double TextHeight, Space, XUnit, YUnit, Min, Max;
        }

        Units GetUnits() {
            Units u = new Units();

            u.TextHeight = Math.Ceiling(Graphics.FromHwnd(IntPtr.Zero).MeasureString("Text", Font).Height);

            u.Title = new RectangleF();
            u.Title.Width = ClientRectangle.Width;

            if (!string.IsNullOrWhiteSpace(Title))
                u.Title.Height = (float)u.TextHeight + 3;

            u.XAxis = new RectangleF();
            u.XAxis.Width = ClientRectangle.Width;
            u.XAxis.Height = (float)u.TextHeight + 10;
            u.XAxis.Inflate(-20, 0);
            u.XAxis.Y = ClientRectangle.Y + ClientRectangle.Height - u.XAxis.Height;

            u.Chart = new RectangleF(
                u.XAxis.X,
                ClientRectangle.Y + u.Title.Height,
                u.XAxis.Width,
                ClientRectangle.Height - u.Title.Height - u.XAxis.Height
            );

            u.Space = LastIndex / Scope.Zoom;
            u.XUnit = u.Chart.Width / u.Space;
            u.YUnit = u.Chart.Height / YMaxValue;
            
            u.Min = Scope.Viewport * LastIndex;
            u.Max = Math.Min(LastIndex, u.Space + u.Min);

            return u;
        }

        public readonly Scope Scope;

        protected override void OnMouseWheel(MouseEventArgs e) {
            base.OnMouseWheel(e);

            if (!HasData) return;

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
                if (HasData) Invalidate();
            }
        }
        const double hoverRadius = 29;

        protected override void OnMouseLeave(EventArgs e) {
            base.OnMouseLeave(e);

            HighlightPoint = -1;
        }

        protected override void OnMouseMove(MouseEventArgs e) {
            base.OnMouseMove(e);

            if (!HasData) return;

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
            int first = (int)Math.Ceiling(Math.Max(u.Min, x - r));
            int last = (int)Math.Floor(Math.Min(u.Max, x + r));

            double closest = r * r;
            int win = -1;

            for (int i = first; i <= last; i++) {
                var x2 = i - x;
                x2 *= x2;
                var y2 = (Data[i] - y) * aspect;
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

            if (!HasData) return;
            if (e.Button != MouseButtons.Left) return;

            Spotlight?.Invoke(Parent, EventArgs.Empty);
        }

        protected override void OnPaint(PaintEventArgs e) {
            if (Suspended) return;

            base.OnPaint(e);

            if (!HasData) return;

            Units u = GetUnits();

            Console.WriteLine(ClientRectangle);

            PointF ToPointF(int index) => new PointF(
                (float)((index - u.Min) * u.XUnit - 0.5) + u.Chart.X,
                (float)(FlipY(Data[index]) * u.YUnit - 0.5) + u.Chart.Y
            );

            Color textColor = Color.FromArgb(160, 160, 160);
            Color backColor = Color.FromArgb((byte)(BackColor.R * 0.9), (byte)(BackColor.G * 0.9), (byte)(BackColor.B * 0.9));
            Color lineColor = ForeColor.Blend(ForeColor.Blend(backColor, 0.9), 0.9);
            Color lowColor = ForeColor.Blend(backColor, 0.5);
            Color lowTransparentColor = Color.FromArgb(128, ForeColor);
            Color pointColor = ForeColor.Blend(Color.White, 0.6);

            Brush textBrush = new SolidBrush(textColor);
            Brush backBrush = new SolidBrush(backColor);
            Brush lineBrush = new SolidBrush(lineColor);
            Brush lowBrush = new SolidBrush(lowColor);
            Brush lowTransparentBrush = new SolidBrush(lowTransparentColor);

            u.Chart.Inflate(1, 1);
            LinearGradientBrush gradientBrush = new LinearGradientBrush(u.Chart, Color.Transparent, Color.Transparent, LinearGradientMode.Vertical);
            LinearGradientBrush pointBrush = new LinearGradientBrush(u.Chart, Color.Transparent, Color.Transparent, LinearGradientMode.Vertical);
            u.Chart.Inflate(-1, -1);

            var blend = new ColorBlend();
            blend.Positions = new float[] { 0, 0.5f, 1 };
            blend.Colors = new Color[] { lineColor, lineColor, lowColor };
            gradientBrush.InterpolationColors = blend;

            blend.Colors = new Color[] { pointColor, pointColor, lineColor };
            pointBrush.InterpolationColors = blend;

            Brush shadowBrush = new SolidBrush(Color.FromArgb(200, backColor));
            Brush shadowTextBrush = new SolidBrush(Color.FromArgb(224, (byte)(BackColor.R * 0.75), (byte)(BackColor.G * 0.75), (byte)(BackColor.B * 0.75)));

            Pen linePen = new Pen(lineBrush);
            Pen gradientPen = new Pen(gradientBrush);
            
            Pen xPen = new Pen(Color.FromArgb(20, 20, 20));
            Pen yPen = new Pen(Color.FromArgb(48, 48, 48));

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            /* Title, background, YAxis */
            {
                if (!string.IsNullOrWhiteSpace(Title))
                    e.Graphics.DrawShadowString(Title, Font, textBrush, shadowTextBrush, u.Title, true);

                e.Graphics.FillRectangle(backBrush, u.Chart);

                for (int i = 0; i < 6; i++) {
                    float y = (float)Math.Round(u.Chart.Y + u.Chart.Height * i / 5);
                    e.Graphics.DrawLine(yPen, u.Chart.X, y, u.Chart.X + u.Chart.Width, y);
                }
            }

            /* XAxis */
            {
                double interval = Scope.GetInterval(XMaxValue, u.Chart.Width, out double px, out double pos);
                double offset = (float)Math.Round(pos * px);
                int posCeil = (int)Math.Ceiling(pos);

                var textRect = new RectangleF(0, u.XAxis.Y + 7, (float)px, (float)u.TextHeight);

                float GetS() => (float)(Math.Round(posCeil * px) - offset);

                for (float s = GetS(); s <= u.Chart.Width + 0.5; posCeil++, s = GetS()) {
                    s = (float)Math.Round(s + u.XAxis.X);

                    e.Graphics.DrawLine(xPen, s, u.Chart.Y, s, u.XAxis.Y + 4);
                    textRect.X = s - textRect.Width / 2;
                    e.Graphics.DrawShadowString($"{interval * posCeil:0.###}", Font, textBrush, shadowTextBrush, textRect, true);
                }
            }

            /* Line chart */
            {
                int first = (int)Math.Floor(u.Min);
                int last = (int)Math.Ceiling(u.Max);

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
                        e.Graphics.DrawLine(gradientPen, new PointF(prev.X, yMin), new PointF(point.X, yMax));
                        useYLine = false;

                    } else {
                        e.Graphics.DrawLine(gradientPen, prev, point);

                        if (u.XUnit > 5) {
                            if (i == 1 && HighlightPoint != 0)
                                e.Graphics.FillEllipse(pointBrush, prev.X - ptSize / 2, prev.Y - ptSize / 2, ptSize, ptSize);

                            if (first + i != HighlightPoint)
                                e.Graphics.FillEllipse(pointBrush, point.X - ptSize / 2, point.Y - ptSize / 2, ptSize, ptSize);
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

            /* Highlight */
            {
                if (HighlightPoint != -1) {
                    var point = ToPointF(HighlightPoint);
                    var marker = new RectangleF(point.X - 3, point.Y - 3, 6, 6);

                    e.Graphics.FillEllipse(lowTransparentBrush, marker);
                    e.Graphics.DrawEllipse(linePen, marker);

                    point = new PointF((float)Math.Round(point.X), (float)Math.Round(point.Y));

                    string text = $"({HighlightPoint * XFactor:0.###}, {Data[HighlightPoint]:0.###})";
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

                    e.Graphics.FillRectangle(shadowBrush, shadowRect);
                    e.Graphics.DrawShadowString(text, Font, textBrush, shadowTextBrush, textRect, true);
                }
            }
        }
    }
}
