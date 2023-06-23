using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

using DarkUI.Controls;

namespace Keyboard_Inspector {
    class ChartArea: Control {
        ContextMenuStrip KeyMenu;

        public ChartArea() {
            AllowDrop = true;
            DoubleBuffered = true;
            ResizeRedraw = true;

            ForeColor = Color.FromArgb(65, 140, 240);

            KeyMenu = new DarkContextMenu();
            KeyMenu.SuspendLayout();

            KeyMenu.Items.AddRange(new ToolStripItem[] {
                new ToolStripMenuItem("Change &Color..."),
                new ToolStripMenuItem("&Reset Color"),
                new ToolStripSeparator(),
                new ToolStripMenuItem("&Hide Key"),
                new ToolStripMenuItem("Show &All Keys"),
                new ToolStripSeparator(),
                new ToolStripMenuItem("&Freeze"),
                new ToolStripMenuItem("Un&freeze")
            });

            KeyMenu.Items[0].Click += (_, __) => {
                if (!HasHistory) return;
                if (!(KeyMenu.Tag is int k)) return;

                KeyMenu.Tag = null;

                ColorDialog cd = new ColorDialog();
                cd.Color = Inputs[k].Color;

                if (cd.ShowDialog() == DialogResult.OK) {
                    Inputs[k].Color = cd.Color;
                    Invalidate();
                }
            };

            KeyMenu.Items[1].Click += (_, __) => {
                if (!HasHistory) return;
                if (!(KeyMenu.Tag is int k)) return;

                KeyMenu.Tag = null;

                Inputs[k].Color = Input.DefaultColor;
                Invalidate();
            };

            KeyMenu.Items[3].Click += (_, __) => {
                if (!HasHistory) return;
                if (!(KeyMenu.Tag is int k)) return;

                KeyMenu.Tag = null;

                Inputs[k].Visible = false;
                YMaxValue--;

                RefreshVisibleInputs();

                Invalidate();
            };

            KeyMenu.Items[4].Click += (_, __) => {
                if (!HasHistory) return;

                KeyMenu.Tag = null;

                for (int i = 0; i < Inputs.Count; i++)
                    Inputs[i].Visible = true;

                YMaxValue = Inputs.Count;

                RefreshVisibleInputs();

                Invalidate();
            };

            KeyMenu.Items[6].Click += (_, __) => {
                if (!HasHistory) return;

                KeyMenu.Tag = null;

                Frozen = true;

                Invalidate();
            };

            KeyMenu.Items[7].Click += (_, __) => {
                if (!HasHistory) return;

                KeyMenu.Tag = null;

                Frozen = false;

                Invalidate();
            };

            KeyMenu.ResumeLayout(false);
        }

        bool Suspended = false;
        public void SuspendPaint() => Suspended = true;
        public void ResumePaint() {
            Suspended = false;
            Invalidate();
        }
        
        public double Zoom { get; private set; } = 1;

        double _viewport = 0;
        public double Viewport {
            get => _viewport;
            private set {
                _viewport = value;
                if (_viewport < 0) _viewport = 0;
                else {
                    double t = 1 / Zoom;
                    if (_viewport + t > 1) _viewport = 1 - t;
                }
            }
        }

        public Func<int, double> IntervalGenerator = i => Math.Pow(2, i - 10);
        public double? ScopeDefaultX = null;

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

        void Scrolled(object sender, EventArgs e) {
            if (updatingScroll) return;

            Viewport = (double)ScrollBar.Value / ScrollBar.Maximum;
            Invalidate();
        }

        bool updatingScroll = false;

        void UpdateScroll() {
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

        void ScopeReset() {
            Zoom = ScopeDefaultX == null? 1 : (XMaxFactored / ScopeDefaultX.Value);
            Viewport = 0;

            Invalidate();
            UpdateScroll();
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

                YMaxValue = Math.Max(1, LineData.Max());
                ScopeReset();

            } else {
                kind = Kind.None;
                _xfactor = 1;
            }

            Invalidate();
        }

        class InputHolder {
            public readonly Input Input;
            public Color Color;
            public bool Visible;
            public List<Event> Events;
            public RectangleF TextRect, MenuRect, DragRect;

            public InputHolder(Input input) {
                Input = input;
                Color = Input.DefaultColor;
                Visible = true;
            }
        }

        string InputAsString(Input i, bool includeSource)
            => $"{(includeSource? $"[{KeyHistory.Sources[i.Source]}] " : "")}{i}";

        Result KeyHistory;
        bool HasHistory => !Result.IsEmpty(KeyHistory);

        List<InputHolder> Inputs;
        List<InputHolder> VisibleInputs;
        
        bool Frozen;
        bool MultipleSources;

        void RefreshVisibleInputs() {
            VisibleInputs = Inputs.Where(x => x.Visible).ToList();
            MultipleSources = VisibleInputs.Select(i => i.Input.Source).Distinct().Count() > 1;
        }

        Dictionary<Input, int> InputIndexes;

        public void LoadData(Result result) {
            KeyHistory = result;

            _xfactor = 1;
            HighlightPoint = -1;

            if (HasHistory) {
                kind = Kind.KeyHistory;
                
                var newInputs = KeyHistory.Events.Select(i => i.Input).Distinct().Select(i => new InputHolder(i)).ToList();

                if (Frozen) {
                    Inputs.RemoveAll(i => !i.Visible);
                    YMaxValue = Inputs.Count;

                    foreach (var i in newInputs) {
                        if (Inputs.Any(j => j.Input == i.Input)) continue;

                        i.Visible = false;
                        Inputs.Add(i);
                    }

                } else {
                    Inputs = newInputs;

                    Source best = KeyHistory.GetBestSource(0.8);
                    if (best != null) {
                        foreach (var i in Inputs) {
                            i.Visible = KeyHistory.Sources[i.Input.Source] == best;
                        }
                        YMaxValue = Inputs.Count(i => i.Visible);

                    } else {
                        YMaxValue = Inputs.Count;
                    }

                }

                RefreshVisibleInputs();

                InputIndexes = new Dictionary<Input, int>();

                for (int i = 0; i < Inputs.Count; i++) {
                    InputIndexes[Inputs[i].Input] = i;
                    Inputs[i].Events = new List<Event>(KeyHistory.Events.Count);
                }

                for (int i = 0; i < KeyHistory.Events.Count; i++) {
                    var e = KeyHistory.Events[i];
                    Inputs[InputIndexes[e.Input]].Events.Add(e);
                }

                ScopeReset();

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

        double GetValue(int index) {
            if (kind == Kind.KeyHistory) return Convert.ToInt32(KeyHistory.Events[index].Pressed);
            return GetY(index);
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

        protected override void OnMouseWheel(MouseEventArgs e) {
            base.OnMouseWheel(e);

            if (!HasAny) return;

            Units u = GetUnits();
            var x = (e.X - u.Chart.X) / u.Chart.Width;

            if (!x.InRangeIE(0, 1)) return;

            double s = 1 / Zoom;

            double change = Math.Pow(1.05, e.Delta / 120.0);
            Zoom *= change;

            if (Zoom <= 1) {
                Zoom = 1;
                Viewport = 0;

            } else {
                if (Zoom > 200000) {
                    Zoom = 200000;
                    change = Zoom * s;
                }

                Viewport += x * (1 - 1 / change) * s;
            }

            if (Capture) {
                CapturedOffset = 0;
                CapturedViewport = Viewport;
            }

            UpdateScroll();
            FindHighlightPoint(e.Location);
            Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e) {
            base.OnMouseLeave(e);

            HighlightPoint = -1;
        }

        bool IntersectYText(Point pt, bool menu, out int result) {
            result = -1;

            for (int k = 0; k < Inputs.Count; k++) {
                if (!Inputs[k].Visible) continue;

                if ((menu? Inputs[k].MenuRect : Inputs[k].DragRect).Contains(pt)) {
                    result = k;
                    return true;
                }
            }

            return false;
        }

        bool IntersectForMenu(Point pt, out int result)
            => IntersectYText(pt, true, out result);
        
        bool IntersectForDrag(Point pt, out int result) {
            if (Inputs.Count <= 1) {
                result = -1;
                return false;
            }

            return IntersectYText(pt, false, out result);
        }

        protected override void OnMouseMove(MouseEventArgs e) {
            base.OnMouseMove(e);

            if (!HasAny) return;

            if (Captured) {
                Cursor.Position = PointToScreen(CapturedPoint);

                if (e.Location != CapturedPoint) {
                    Program.CursorVisible = false;
                    HighlightPoint = -1;
                }

                if (e.X != CapturedPoint.X) {
                    CapturedOffset += CapturedPoint.X - e.X;
                    Units u = GetUnits();

                    Viewport = CapturedViewport + (CapturedOffset / u.Chart.Width / Zoom);

                    UpdateScroll();
                    Invalidate();
                }
            
            } else FindHighlightPoint(e.Location);
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

        void FindHighlightPoint(Point p) {
            Units u = GetUnits();

            double x = (p.X - u.Chart.X) / u.XUnit + u.Min;
            double y = FlipY((p.Y - u.Chart.Y) / u.YUnit);

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
        }

        public event EventHandler Spotlight;

        protected override void OnMouseDoubleClick(MouseEventArgs e) {
            base.OnMouseDoubleClick(e);

            if (!HasAny) return;
            if (e.Button != MouseButtons.Left) return;

            Spotlight?.Invoke(Parent, EventArgs.Empty);
        }

        protected override void OnMouseClick(MouseEventArgs e) {
            base.OnMouseClick(e);

            if (!HasHistory) return;
            if (e.Button != MouseButtons.Right) return;

            bool intersects = IntersectForMenu(e.Location, out int k);
            if (intersects) KeyMenu.Tag = k;

            KeyMenu.Items[0].Available = intersects;
            KeyMenu.Items[1].Available = intersects && Inputs[k].Color != Input.DefaultColor;

            KeyMenu.Items[3].Available = intersects && VisibleInputs.Count > 1;
            KeyMenu.Items[4].Available = Inputs.Any(i => !i.Visible);

            KeyMenu.Items[6].Available = !Frozen;
            KeyMenu.Items[7].Available = Frozen;

            KeyMenu.Items.AutoSeparators();

            Cursor = Cursors.Default;
            KeyMenu.Show(this, e.Location);
        }

        bool Captured;
        int CapturedOffset;
        Point CapturedPoint;
        double CapturedViewport;

        protected override void OnMouseDown(MouseEventArgs e) {
            base.OnMouseDown(e);

            if (!HasAny) return;
            if (e.Button != MouseButtons.Left) return;

            if (HasHistory && IntersectForDrag(e.Location, out int k)) {
                DoDragDrop(k, DragDropEffects.Move);
                return;
            }
            
            Units u = GetUnits();

            if (u.Chart.Contains(e.Location) || u.XAxis.Contains(e.Location)) {
                // Mouse Capture is done automatically by WinForms
                // Just need a flag so we know it's because user is panning
                Captured = true;
                CapturedOffset = 0;
                CapturedPoint = e.Location;
                CapturedViewport = Viewport;
            }
        }

        protected override void OnMouseUp(MouseEventArgs e) {
            base.OnMouseUp(e);

            if (e.Button != MouseButtons.Left) return;

            Program.CursorVisible = true;
            Captured = false;
        }

        bool ValidateDrag(DragEventArgs e, out int result) {
            result = -1;
            if (!e.Data.GetDataPresent("System.Int32")) return false;

            result = (int)e.Data.GetData("System.Int32");
            return result.InRangeIE(0, Inputs.Count) && Inputs[result].Visible;
        }

        protected override void OnDragOver(DragEventArgs e) {
            base.OnDragOver(e);

            e.Effect = DragDropEffects.None;

            if (!HasHistory)
            if (!ValidateDrag(e, out _)) return;

            if (IntersectForDrag(PointToClient(new Point(e.X, e.Y)), out _))
                e.Effect = DragDropEffects.Move;
        }

        protected override void OnDragDrop(DragEventArgs e) {
            base.OnDragOver(e);

            if (!HasHistory) return;
            if (!ValidateDrag(e, out int f)) return;

            if (IntersectForDrag(PointToClient(new Point(e.X, e.Y)), out int k)) {
                var i = Inputs[f];
                Inputs.RemoveAt(f);
                Inputs.Insert(k, i);

                RefreshVisibleInputs();

                Invalidate();
            }
        }

        class Units {
            public RectangleF Title, Chart, XAxis, YAxis;
            public double TextHeight, Space, XUnit, YUnit, Min, Max;
        }

        Units GetUnits() {
            Units u = new Units();

            using (Graphics gfx = Graphics.FromHwnd(IntPtr.Zero)) {
                u.TextHeight = Math.Ceiling(gfx.MeasureString("Text", Font).Height);

                u.Title = new RectangleF();
                u.Title.X = ClientRectangle.X;
                u.Title.Y = ClientRectangle.Y;
                u.Title.Width = ClientRectangle.Width;

                if (!string.IsNullOrWhiteSpace(Title))
                    u.Title.Height = (float)u.TextHeight + 3;

                u.XAxis = new RectangleF();
                u.XAxis.Height = (float)u.TextHeight + 10;
                u.XAxis.Y = ClientRectangle.Bottom - u.XAxis.Height;

                u.YAxis = new RectangleF();
                u.YAxis.X = ClientRectangle.X + 20;
                u.YAxis.Y = u.Title.Bottom;
                u.YAxis.Height = ClientRectangle.Height - u.Title.Height - u.XAxis.Height;
            
                if (kind == Kind.KeyHistory) {
                    foreach (var i in VisibleInputs)
                        i.TextRect.Size = gfx.MeasureString(InputAsString(i.Input, MultipleSources), Font);

                    u.YAxis.Width = (float)Math.Ceiling(VisibleInputs.Max(i => i.TextRect.Width));
                }

                u.XAxis.X = u.YAxis.Right;
                if (u.YAxis.Width > 0)
                    u.XAxis.X += 4;

                u.XAxis.Width = ClientRectangle.Width - u.XAxis.X - 20;

                u.Chart = new RectangleF(
                    u.XAxis.X,
                    u.YAxis.Y,
                    u.XAxis.Width,
                    u.YAxis.Height
                );

                u.Space = XMax / Zoom;
                u.XUnit = u.Chart.Width / u.Space;
                u.YUnit = u.Chart.Height / YMaxValue;

                u.Min = Viewport * XMax;
                u.Max = Math.Min(XMax, u.Space + u.Min);

                if (kind == Kind.KeyHistory) {
                    double clickWidth = u.YAxis.Width;
                    double compensation = 0;

                    if (clickWidth < 25) {
                        compensation = 25 - clickWidth;
                        clickWidth = 25;
                    }

                    for (int k = 0; k < VisibleInputs.Count; k++) {
                        var i = VisibleInputs[k];

                        i.DragRect = new RectangleF(
                            (float)(u.YAxis.X - compensation),
                            (float)(u.Chart.Y + k * u.YUnit),
                            (float)clickWidth,
                            (float)u.YUnit
                        );

                        i.MenuRect = i.DragRect;
                        i.MenuRect.Width += u.Chart.Width;

                        i.TextRect.X = u.YAxis.Right - i.TextRect.Width;
                        i.TextRect.Y = i.MenuRect.Y + (float)((u.YUnit - i.TextRect.Height) / 2);
                    }
                }
            }

            return u;
        }

        // TODO Remove unused
        class ColorSet {
            public Color TextColor, BackColor, LineColor, LowColor, LowTransparentColor, PointColor;
            public Brush TextBrush, BackBrush, LineBrush, LowBrush, LowTransparentBrush, ShadowBrush, ShadowTextBrush, FrozenBrush;
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
            cs.FrozenBrush = new SolidBrush(cs.TextColor.Blend(Color.SkyBlue, 0.45));

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
                    e.Graphics.DrawLine(cs.YPen, u.Chart.X, y, u.Chart.Right, y);
                }
            }

            /* XAxis */
            {
                int incIndex = 0;
                double factorToPx = u.Chart.Width * Zoom / XMaxFactored;
                double lowest = (kind == Kind.Line? 30 : 40) / factorToPx;
                double interval;

                do {
                    interval = IntervalGenerator(incIndex++);
                } while (interval < lowest);

                double px = interval * factorToPx;
                double offset = (float)Math.Round(Viewport * u.Chart.Width * Zoom);
                int pos = (int)Math.Ceiling(Viewport * XMaxFactored / interval) - 1;

                var textRect = new RectangleF(0, u.XAxis.Y + 7, 0, (float)u.TextHeight);
                float textLowS = (float)(u.XAxis.X - px / 6);
                float textHighS = (float)(u.XAxis.Right + px / 6);

                float GetS() => (float)(Math.Round(pos * px) - offset) + u.Chart.X;

                for (float s = GetS();; pos++, s = GetS()) {
                    if (s.InRangeII(u.Chart.X, u.Chart.Right)) {
                        float sr = (float)Math.Round(s);
                        e.Graphics.DrawLine(cs.XPen, sr, u.Chart.Y - 0.5f, sr, u.XAxis.Y + 4.5f);
                    }
                    
                    string text = $"{interval * pos:0.###}";
                    textRect.Width = e.Graphics.MeasureString(text, Font).Width;

                    if (s.InRangeII(textLowS, textHighS)) {
                        textRect.X = (float)Math.Round(s.Clamp(u.XAxis.X, u.XAxis.Right) - textRect.Width / 2);
                        e.Graphics.DrawShadowString(text, Font, cs.TextBrush, cs.ShadowTextBrush, textRect, true);

                    } else if (s >= u.XAxis.Right)
                        break;
                }
            }

            /* Line chart */
            if (kind == Kind.Line) {
                PointColor = _ => cs.LineColor;

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
                        e.Graphics.DrawLine(cs.GradientPen,
                            new PointF( prev.X, prev.Y > point.Y? yMax : yMin),
                            new PointF(point.X, prev.Y > point.Y? yMin : yMax)
                        );
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
                PointColor = _ => Inputs[InputIndexes[KeyHistory.Events[_].Input]].Color; // it's weird, but good enough
                
                var blend = new ColorBlend();
                blend.Positions = new float[] { 0f, 0.1f, 0.3f, 1 };

                var height = u.YUnit - 2;
                RectangleF bar = new RectangleF(u.Chart.X, 0, u.Chart.Width, 0);

                for (int k = 0; k < VisibleInputs.Count; k++) {
                    var i = VisibleInputs[k];
                    e.Graphics.DrawShadowString(InputAsString(i.Input, MultipleSources), Font, Frozen? cs.FrozenBrush : cs.TextBrush, cs.ShadowTextBrush, i.TextRect);

                    double y = u.Chart.Y + k * u.YUnit;

                    bar.Height = Math.Max(1, (float)(Math.Floor(y + height) - (y = Math.Floor(y))));
                    bar.Y = (float)(y + 0.5);

                    LinearGradientBrush brush = new LinearGradientBrush(bar, Color.Transparent, Color.Transparent, LinearGradientMode.Vertical);

                    var color = Color.FromArgb(250, i.Color);
                    var topColor = Color.FromArgb(240, color.Blend(Color.Black, 0.98));
                    var bottomColor = Color.FromArgb(215, color.Blend(Color.Black, 0.95));

                    blend.Colors = new Color[] { topColor, color, color, bottomColor };
                    brush.InterpolationColors = blend;

                    var events = i.Events;
                    if (!events.Any()) continue;

                    int left = BinarySearch(events, u.Min, 1);
                    int right = BinarySearch(events, u.Max, 0) + 1;

                    var points = new List<double>() { double.NegativeInfinity };
                    var rounded = new List<double>();

                    void makePoint(double start, double end) {
                        if (start <= u.Min) start = u.Min;
                        if (start >= u.Max) return;
                        if (end <= u.Min) return;
                        if (end >= u.Max) end = u.Max;

                        start = u.Chart.X + (start - u.Min) * u.XUnit;
                        end = u.Chart.X + (end - u.Min) * u.XUnit;

                        points.Add(start);
                        points.Add(end);

                        rounded.Add(Math.Floor(start) + 0.5);
                        rounded.Add(Math.Floor(end) + 0.5);
                    }

                    int ev = left;
                    if (!events[ev].Pressed) {
                        makePoint(0, events[ev].Time);
                        ev++;
                    }

                    for (; ev < right; ev += 2) {
                        makePoint(
                            events[ev].Time,
                            ev + 1 < right? events[ev + 1].Time : KeyHistory.Time
                        );
                    }

                    points.Add(double.PositiveInfinity);

                    for (int p = 1; p < points.Count - 1; p++) {
                        double dist = Math.Min(points[p] - points[p - 1], points[p + 1] - points[p]);
                        points[p] = rounded[p - 1].Blend(points[p], (dist - 2) / 3);

                        if (p % 2 == 0) {
                            bar.X = (float)points[p - 1];
                            bar.Width = (float)(points[p] - points[p - 1]);

                            e.Graphics.FillRectangle(brush, bar);
                        }
                    }
                }
            }

            /* Highlight */
            if (HighlightPoint != -1) {
                var point = ToPointF(HighlightPoint);
                var marker = new RectangleF(point.X - 3, point.Y - 3, 6, 6);

                Color color = PointColor(HighlightPoint).Blend(Color.White, 0.16);

                e.Graphics.FillEllipse(new SolidBrush(Color.FromArgb(128, color)), marker);
                e.Graphics.DrawEllipse(new Pen(color), marker);

                point = new PointF((float)Math.Round(point.X), (float)Math.Round(point.Y));

                string text = $"({GetXFactored(HighlightPoint):0.###}, {GetValue(HighlightPoint):0.###})";
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
