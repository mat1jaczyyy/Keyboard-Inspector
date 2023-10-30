using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

using DarkUI.Controls;

namespace Keyboard_Inspector {
    class Chart: Control {
        static Stopwatch clickTimer = new Stopwatch();

        static Chart() {
            clickTimer.Start();
        }

        ContextMenuStrip InputMenu;

        public Chart() {
            AllowDrop = true;
            DoubleBuffered = true;
            ResizeRedraw = true;

            ForeColor = Color.FromArgb(65, 140, 240);

            InputMenu = new DarkContextMenu();
            InputMenu.SuspendLayout();

            InputMenu.Items.AddRange(new ToolStripItem[] {
                new ToolStripMenuItem("Change &Color..."),
                new ToolStripMenuItem("&Reset Color"),
                new ToolStripSeparator(),
                new ToolStripMenuItem("&Hide Input"),
                new ToolStripMenuItem("Hide &Device"),
                new ToolStripSeparator(),
                new ToolStripMenuItem("&Sort by Device"),
                new ToolStripMenuItem("Show &All Inputs"),
                new ToolStripSeparator(),
                new ToolStripMenuItem("&Freeze"),
                new ToolStripMenuItem("Un&freeze")
            });

            // Change Color
            InputMenu.Items[0].Click += (_, __) => {
                if (!HasHistory) return;
                if (!(InputMenu.Tag is int k)) return;

                InputMenu.Tag = null;

                ColorDialog cd = new ColorDialog();
                cd.Color = KeyHistory.Inputs[k].Color;

                if (cd.ShowDialog() == DialogResult.OK) {
                    KeyHistory.Inputs[k].Color = cd.Color;
                    Invalidate();
                }
            };

            // Reset Color
            InputMenu.Items[1].Click += (_, __) => {
                if (!HasHistory) return;
                if (!(InputMenu.Tag is int k)) return;

                InputMenu.Tag = null;

                KeyHistory.Inputs[k].Color = Input.DefaultColor;
                Invalidate();
            };

            // Hide Input
            InputMenu.Items[3].Click += (_, __) => {
                if (!HasHistory) return;
                if (!(InputMenu.Tag is int k)) return;

                InputMenu.Tag = null;

                KeyHistory.Inputs[k].Visible = false;
                YMaxValue--;

                Invalidate();

                KeyHistory.Analysis.Analyze();
            };

            // Hide Device
            InputMenu.Items[4].Click += (_, __) => {
                if (!HasHistory) return;
                if (!(InputMenu.Tag is int k)) return;

                InputMenu.Tag = null;

                var source = KeyHistory.Inputs[k].Input.Source;
                
                foreach (var i in KeyHistory.Inputs)
                    if (i.Input.Source == source)
                        i.Visible = false;

                YMaxValue = KeyHistory.VisibleInputs().Count();
                
                Invalidate();

                KeyHistory.Analysis.Analyze();
            };

            // Sort by Device
            InputMenu.Items[6].Click += (_, __) => {
                if (!HasHistory) return;

                KeyHistory.Inputs.SortByKey(i => KeyHistory.Sources[i.Input.Source].Name);

                Invalidate();
            };

            // Show All Inputs
            InputMenu.Items[7].Click += (_, __) => {
                if (!HasHistory) return;

                InputMenu.Tag = null;

                foreach (var i in KeyHistory.Inputs)
                    i.Visible = true;

                YMaxValue = KeyHistory.Inputs.Count;

                Invalidate();

                KeyHistory.Analysis.Analyze();
            };

            // Freeze
            InputMenu.Items[9].Click += (_, __) => {
                if (!HasHistory) return;

                InputMenu.Tag = null;

                Program.Freeze();

                Invalidate();
            };

            // Unfreeze
            InputMenu.Items[10].Click += (_, __) => {
                if (!HasHistory) return;

                InputMenu.Tag = null;

                Program.Unfreeze();

                Invalidate();
            };

            InputMenu.ResumeLayout(false);
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

        void ScopeReset() {
            Zoom = ScopeDefaultX == null? 1 : (XMaxFactored / ScopeDefaultX.Value);
            Viewport = 0;

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

                YMaxValue = Math.Max(1, LineData.Max());
                ScopeReset();

            } else {
                kind = Kind.None;
                _xfactor = 1;
            }

            Invalidate();
        }

        string InputAsString(Input i, bool includeSource)
            => $"{(includeSource? $"[{KeyHistory.Sources[i.Source]}] " : "")}{i}";

        Result KeyHistory;
        bool HasHistory => !Result.IsEmpty(KeyHistory);

        public void LoadData(Result result) {
            KeyHistory = result;

            _xfactor = 1;
            HighlightPoint = -1;

            if (HasHistory) {
                kind = Kind.KeyHistory;

                YMaxValue = KeyHistory.VisibleInputs().Count();
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
        public double MaxZoom {
            get {
                if (kind == Kind.Line) return XMax * 0.66;
                else if (kind == Kind.KeyHistory) return XMax * 1000 * 0.66;
                return 1;
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
            else if (kind == Kind.KeyHistory) return KeyHistory.VisibleIndex(KeyHistory.Events[index].Input) + 0.5; // TODO: perf?
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

        bool ZoomChange(Point pt, double scrolls, Units u = null) {
            if (scrolls == 0) return false;

            u = u?? GetUnits();

            var x = (pt.X - u.Chart.X) / u.Chart.Width;
            double s = 1 / Zoom;

            double change = Math.Pow(1.05, scrolls);
            Zoom *= change;

            if (Zoom <= 1) {
                Zoom = 1;
                Viewport = 0;

            } else {
                if (Zoom > MaxZoom) {
                    Zoom = MaxZoom;
                    change = Zoom * s;
                }

                Viewport += x * (1 - 1 / change) * s;
            }

            Invalidate();
            return true;
        }

        protected override void OnMouseWheel(MouseEventArgs e) {
            base.OnMouseWheel(e);

            if (!HasAny) return;

            if (CapturedScrollBar != ScrollBarComponent.None) return;
            if (Captured && CapturedDirection == PanDirection.Vertical) return;

            Units u = GetUnits();
            if (!u.Chart.ContainsIX(e.Location) && !u.XAxis.ContainsIX(e.Location)) return;

            if (ZoomChange(e.Location, e.Delta / 120.0, u) && Captured) {
                CapturedOffset = new Point(0, 0);
                CapturedViewport = Viewport;
            }

            FindHighlightPoint(e.Location);
        }

        protected override void OnMouseLeave(EventArgs e) {
            base.OnMouseLeave(e);

            HighlightPoint = -1;
            Hovering = ScrollBarComponent.None;
        }

        bool IntersectYText(Point pt, bool menu, out int result, Units u = null) {
            u = u?? GetUnits();

            result = -1;

            for (int i = 0, v = 0; i < KeyHistory.Inputs.Count; i++) {
                if (!KeyHistory.Inputs[i].Visible) continue;

                if ((menu? u.KeyMenu : u.KeyDrag)[v].Contains(pt)) {
                    result = i;
                    return true;
                }
                v++;
            }

            return false;
        }

        bool IntersectForMenu(Point pt, out int result, Units u = null)
            => IntersectYText(pt, true, out result, u);
        
        bool IntersectForDrag(Point pt, out int result, Units u = null) {
            if (KeyHistory.Inputs.Count <= 1) {
                result = -1;
                return false;
            }

            return IntersectYText(pt, false, out result, u);
        }

        enum ScrollBarComponent {
            None, ScrollBar, NotchLeft, NotchRight
        }

        ScrollBarComponent _hovering = ScrollBarComponent.None;
        ScrollBarComponent Hovering {
            get => _hovering;
            set {
                if (_hovering != value) {
                    _hovering = value;
                    Invalidate();
                }
            }
        }

        RectangleF GetScrollBarComponent(ScrollBarComponent x, Units u = null) {
            u = u?? GetUnits();

            switch (x) {
                case ScrollBarComponent.ScrollBar: return u.ScrollBar;
                case ScrollBarComponent.NotchLeft: return u.NotchLeft;
                case ScrollBarComponent.NotchRight: return u.NotchRight;
            }

            return RectangleF.Empty;
        }

        Point CapturedCursorTick(Point pt) {
            Cursor.Position = PointToScreen(CapturedPoint);

            if (pt != CapturedPoint) {
                Program.CursorVisible = false;
                HighlightPoint = -1;
            }

            Point offset = new Point(CapturedPoint.X - pt.X, CapturedPoint.Y - pt.Y);
            CapturedOffset.X += offset.X;
            CapturedOffset.Y += offset.Y;

            return offset;
        }

        void PanAction(Point pt, Units u = null) {
            u = u?? GetUnits();

            Point offset = CapturedCursorTick(pt);

            if (CapturedDirection == PanDirection.None) {
                if (Zoom == 1) {
                    CapturedDirection = PanDirection.Vertical;

                } else {
                    if (CapturedOffset.X * CapturedOffset.X + CapturedOffset.Y * CapturedOffset.Y < 9) return;

                    double ang = Math.Atan2(Math.Abs(offset.Y), Math.Abs(offset.X));
                    if (ang <= Math.PI / 6) CapturedDirection = PanDirection.Horizontal;
                    else if (ang >= Math.PI / 3) CapturedDirection = PanDirection.Vertical;

                    if (CapturedDirection != PanDirection.None) {
                        CapturedOffset = new Point(0, 0);
                        CapturedOffset.X += offset.X;
                        CapturedOffset.Y += offset.Y;
                    }
                }
            }

            if (CapturedDirection == PanDirection.Horizontal) {
                if (offset.X != 0) {
                    Viewport = CapturedViewport + (CapturedOffset.X / u.Chart.Width / Zoom);
                    Invalidate();
                }

            } else if (CapturedDirection == PanDirection.Vertical) {
                if (offset.Y != 0) {
                    Zoom = CapturedZoom;
                    Viewport = CapturedViewport;
                    ZoomChange(CapturedPoint, CapturedOffset.Y / 10.0, u);
                }
            }
        }

        // https://www.desmos.com/calculator/qtg073idnp
        static class Cramp {
            const double s = 0.0004;
            static double a, m, n, z;

            public static void Refresh(double crampedZoom, double maxZoom) {
                a = 1 / crampedZoom;
                m = 1 / maxZoom;
                n = Math.Pow(Math.E, -1 / (a - m));
                z = a - Math.Log(s, n);
            }

            static double f(double x) => Math.Pow(n, a - x) * (a - m) + m;

            public static double Calc(double x) {
                if (x < z) return Math.Max(m, s * (x - z) + f(z));
                if (x < a) return f(x);
                return x;
            }

            public static double Inv(double y) {
                double fz = f(z);
                if (y < fz) return (y - fz) / s + z;
                if (y < a) return a - Math.Log((y - m) / (a - m), n); // f^-1
                return y;
            }
        }

        double NotchAction(double dist, Units u = null) {
            Cramp.Refresh(u.CrampedZoom, MaxZoom);
            return Cramp.Calc(1 / CapturedZoomWithCramp + dist);
        }

        void ScrollAction(Point pt, Units u = null) {
            u = u?? GetUnits();

            CapturedCursorTick(pt);

            double ScrollBarDistance() => -CapturedOffset.X / u.ScrollBarArea.Width;

            if (CapturedScrollBar == ScrollBarComponent.ScrollBar) {
                Viewport = CapturedViewport + ScrollBarDistance();

            } else if (CapturedScrollBar == ScrollBarComponent.NotchLeft) {
                Program.CursorVisible = false;
                double rightEdge = CapturedViewport + 1 / CapturedZoom;
                double newInvZoom = NotchAction(-ScrollBarDistance(), u).Clamp(0, rightEdge);
                Zoom = 1 / newInvZoom;
                Viewport = rightEdge - newInvZoom;

            } else if (CapturedScrollBar == ScrollBarComponent.NotchRight) {
                Program.CursorVisible = false;
                Zoom = 1 / NotchAction(ScrollBarDistance(), u).Clamp(0, 1 - Viewport);
            }

            Invalidate();
        }

        protected override void OnMouseMove(MouseEventArgs e) {
            /* Double Click Region */ {
                Point dist = new Point(e.X - lastDown.X, e.Y - lastDown.Y);
                if (Math.Sqrt(dist.X * dist.X + dist.Y * dist.Y) > SystemInformation.DoubleClickSize.Width / 2.0)
                    clicks = 0;
            }

            base.OnMouseMove(e);

            if (!HasAny) return;

            Units u = GetUnits();
            bool canHighlight = false;

            ScrollBarComponent setHovering = ScrollBarComponent.None;

            if (Captured) {
                PanAction(e.Location, u);

            } else if (CapturedScrollBar != ScrollBarComponent.None) {
                ScrollAction(e.Location, u);

            } else if (u.NotchLeft.Contains(e.Location)) {
                setHovering = ScrollBarComponent.NotchLeft;
            
            } else if (u.NotchRight.Contains(e.Location)) {
                setHovering = ScrollBarComponent.NotchRight;

            } else if (u.ScrollBar.Contains(e.Location)) {
                setHovering = ScrollBarComponent.ScrollBar;

            } else canHighlight = true;
            
            if (canHighlight && (u.Title.ContainsIX(e.Location) || u.Chart.ContainsIX(e.Location) || u.XAxis.ContainsIX(e.Location))) {
                FindHighlightPoint(e.Location);

            } else HighlightPoint = -1;

            Hovering = setHovering;
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

            var visible = HasHistory? KeyHistory.VisibleInputSet() : null;

            for (int i = first; i <= last; i++) {
                if (visible?.Contains(KeyHistory.Events[i].Input) == false) continue;

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
            HandleMouseClick(e);
        }

        protected override void OnMouseClick(MouseEventArgs e) {
            base.OnMouseClick(e);
            HandleMouseClick(e);
        }

        void HandleMouseClick(MouseEventArgs e) {
            if (!HasAny) return;

            if (AnyCaptured) return;

            if (e.Button == MouseButtons.Left) {
                Units u = GetUnits();

                if (clicks == 2 && u.Chart.ContainsIX(e.Location)) {
                    Spotlight?.Invoke(Parent, EventArgs.Empty);
                    clicks = 0;
                    return;
                }

                int jump = 0;
                if (u.ScrollBarJumpLeft.Contains(e.Location) && u.ScrollBarJumpLeft.Contains(lastDown)) jump = -1;
                else if (u.ScrollBarJumpRight.Contains(e.Location) && u.ScrollBarJumpRight.Contains(lastDown)) jump = 1;
                else return;

                Viewport += jump * (1 / Zoom);
                Invalidate();
                return;
            }

            if (!HasHistory) return;

            if (e.Button == MouseButtons.Right) {
                Units u = GetUnits();

                bool intersects = IntersectForMenu(e.Location, out int k, u);
                if (intersects) InputMenu.Tag = k;

                bool multipleInputs = KeyHistory.VisibleInputs().Count() > 1;

                InputMenu.Items[0].Available = intersects;
                InputMenu.Items[1].Available = intersects && KeyHistory.Inputs[k].Color != Input.DefaultColor;

                InputMenu.Items[3].Available = intersects && multipleInputs;
                InputMenu.Items[4].Available = intersects && multipleInputs && u.MultipleSources;

                InputMenu.Items[6].Available = multipleInputs && u.MultipleSources;
                InputMenu.Items[7].Available = KeyHistory.Inputs.Any(i => !i.Visible);

                InputMenu.Items[9].Available = !Program.IsFrozen;
                InputMenu.Items[10].Available = Program.IsFrozen;

                InputMenu.Items.AutoSeparators();

                Cursor = Cursors.Default;
                InputMenu.Show(this, e.Location);
            }
        }

        enum PanDirection {
            None, Horizontal, Vertical
        }

        bool Captured;
        Point CapturedOffset;
        Point CapturedPoint;
        double CapturedXValue;
        PanDirection CapturedDirection;
        double CapturedZoom;
        double CapturedZoomWithCramp;
        double CapturedViewport;
        ScrollBarComponent CapturedScrollBar;

        bool AnyCaptured => Captured || CapturedScrollBar != ScrollBarComponent.None;

        int clicks = 0;
        long lastClick = long.MinValue;
        Point lastDown;

        protected override void OnMouseDown(MouseEventArgs e) {
            /* Double Click Timing */ {
                long click = clickTimer.ElapsedMilliseconds;
                if (click - SystemInformation.DoubleClickTime > lastClick)
                    clicks = 0;

                clicks++;
                lastClick = click;
                lastDown = e.Location;
            }

            base.OnMouseDown(e);

            if (!HasAny) return;
            if (e.Button != MouseButtons.Left) return;

            Units u = GetUnits();

            if (HasHistory && IntersectForDrag(e.Location, out int k, u)) {
                DoDragDrop(k, DragDropEffects.Move);
                return;
            }

            if (u.Chart.ContainsIX(e.Location) || u.XAxis.ContainsIX(e.Location)) {
                // Mouse Capture is done automatically by WinForms
                // Just need a flag so we know it's because user is panning
                Captured = true;
                CapturedOffset = new Point(0, 0);
                CapturedPoint = e.Location;
                CapturedXValue = (e.X - u.Chart.X) / u.XUnit + u.Min;
                CapturedDirection = PanDirection.None;
                CapturedZoom = Zoom;
                CapturedViewport = Viewport;

            } else if (u.ScrollBar.Contains(e.Location)) {
                CapturedScrollBar = Hovering;
                Hovering = ScrollBarComponent.None;
                CapturedOffset = new Point(0, 0);
                CapturedPoint = e.Location;
                CapturedXValue = e.X - GetScrollBarComponent(CapturedScrollBar, u).X;
                CapturedZoom = Zoom;
                Cramp.Refresh(u.CrampedZoom, MaxZoom);
                CapturedZoomWithCramp = 1 / Cramp.Inv(1 / Zoom);
                CapturedViewport = Viewport;
            }
        }

        void StopUIAction(Units u = null) {
            if (Captured) {
                Captured = false;
                Invalidate();
            }

            if (CapturedScrollBar != ScrollBarComponent.None) {
                CapturedPoint.X = (int)(GetScrollBarComponent(CapturedScrollBar, u).X + CapturedXValue);
                Cursor.Position = PointToScreen(CapturedPoint);

                CapturedScrollBar = ScrollBarComponent.None;
                Invalidate();
            }

            Program.CursorVisible = true;
        }

        protected override void OnMouseUp(MouseEventArgs e) {
            base.OnMouseUp(e);

            if (e.Button != MouseButtons.Left) return;

            StopUIAction();
        }

        protected override void OnMouseCaptureChanged(EventArgs e) {
            base.OnMouseCaptureChanged(e);

            if (!Capture)
                StopUIAction();
        }

        bool ValidateDrag(DragEventArgs e, out int result) {
            result = -1;
            if (!e.Data.GetDataPresent("System.Int32")) return false;

            result = (int)e.Data.GetData("System.Int32");
            return result.InRangeIE(0, KeyHistory.Inputs.Count) && KeyHistory.Inputs[result].Visible;
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
                KeyHistory.Inputs.Move(f, k);

                Invalidate();
            }
        }

        class Units {
            public RectangleF Title, Chart, XAxis, YAxis, ScrollBarArea, ScrollBar, ScrollBarJumpLeft, ScrollBarJumpRight, NotchLeft, NotchRight;
            public RectangleF[] KeyDrag, KeyMenu, KeyText;
            public double TextHeight, Space, XUnit, YUnit, Min, Max, CrampedZoom;
            public bool MultipleSources;
        }

        const int MinScrollBarWidth = 20;

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

                u.ScrollBarArea = new RectangleF();
                u.ScrollBarArea.X = u.Title.X + 11.5f;
                u.ScrollBarArea.Width = u.Title.Width - 23;
                u.ScrollBarArea.Height = 17;
                u.ScrollBarArea.Y = ClientRectangle.Bottom - u.ScrollBarArea.Height;

                u.CrampedZoom = (double)u.ScrollBarArea.Width / MinScrollBarWidth;
                float scrollRight = u.ScrollBarArea.Right;

                u.ScrollBar = new RectangleF();
                u.ScrollBar.Y = u.ScrollBarArea.Y + 2.5f;
                u.ScrollBar.Height = u.ScrollBarArea.Height - 8;
                u.ScrollBar.Width = (float)(u.ScrollBarArea.Width / Zoom);

                if (u.ScrollBar.Width < MinScrollBarWidth) {
                    u.ScrollBarArea.Width -= MinScrollBarWidth - u.ScrollBar.Width;
                    u.ScrollBar.Width = MinScrollBarWidth;
                }

                u.ScrollBar.Width = (float)Math.Round(u.ScrollBar.Width);
                u.ScrollBar.X = (float)(u.ScrollBarArea.X + Math.Round(u.ScrollBarArea.Width * Viewport));

                u.ScrollBarJumpLeft = new RectangleF(
                    u.ScrollBarArea.X,
                    u.ScrollBar.Y,
                    u.ScrollBar.X - u.ScrollBarArea.X,
                    u.ScrollBar.Height
                );

                u.ScrollBarJumpRight = new RectangleF(
                    u.ScrollBar.Right,
                    u.ScrollBar.Y,
                    scrollRight - u.ScrollBar.Right,
                    u.ScrollBar.Height
                );

                u.NotchLeft = new RectangleF(
                    u.ScrollBar.X,
                    u.ScrollBar.Y,
                    6,
                    u.ScrollBar.Height
                );

                u.NotchRight = new RectangleF(
                    u.ScrollBar.Right - 6,
                    u.ScrollBar.Y,
                    6,
                    u.ScrollBar.Height
                );

                u.XAxis = new RectangleF();
                u.XAxis.Height = (float)u.TextHeight + 10;
                u.XAxis.Y = u.ScrollBarArea.Y - u.XAxis.Height;

                u.YAxis = new RectangleF();
                u.YAxis.X = ClientRectangle.X + 20;
                u.YAxis.Y = u.Title.Bottom;
                u.YAxis.Height = u.XAxis.Y - u.Title.Bottom;
            
                if (kind == Kind.KeyHistory) {
                    var visible = KeyHistory.VisibleInputs().ToList();
                    
                    u.MultipleSources = visible.Select(i => i.Input.Source).Distinct().Count() > 1;

                    u.KeyDrag = new RectangleF[visible.Count];
                    u.KeyMenu = new RectangleF[visible.Count];
                    u.KeyText = new RectangleF[visible.Count];

                    for (int i = 0; i < visible.Count; i++)
                        u.KeyText[i].Size = gfx.MeasureString(InputAsString(visible[i].Input, u.MultipleSources), Font);

                    u.YAxis.Width = (float)Math.Ceiling(u.KeyText.Max(i => i.Width));
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
                    
                    for (int i = 0; i < u.KeyText.Length; i++) {
                        u.KeyDrag[i] = new RectangleF(
                            (float)(u.YAxis.X - compensation),
                            (float)(u.Chart.Y + i * u.YUnit),
                            (float)clickWidth,
                            (float)u.YUnit
                        );

                        u.KeyMenu[i] = u.KeyDrag[i];
                        u.KeyMenu[i].Width += u.Chart.Width;

                        u.KeyText[i].X = u.YAxis.Right - u.KeyText[i].Width;
                        u.KeyText[i].Y = u.KeyMenu[i].Y + (float)((u.YUnit - u.KeyText[i].Height) / 2);
                    }
                }
            }

            return u;
        }

        // TODO Remove unused
        class ColorSet {
            public Color LineColor;
            public Brush TextBrush, BackBrush, ShadowBrush, ShadowTextBrush, FrozenBrush, ScrollBarBrush, ScrollBarHoverBrush, ScrollBarCapturedBrush;
            public LinearGradientBrush PointBrush;
            public Pen GradientPen, XPen, YPen, CapturedPen;
        }

        ColorSet GetColorSet(Units u) {
            ColorSet cs = new ColorSet();

            Color TextColor, BGColor, PointColor, CapturedColor;
            LinearGradientBrush GradientBrush;

            TextColor = Color.FromArgb(160, 160, 160);
            BGColor = BackColor.Blend(Color.Black, 0.9);
            cs.LineColor = ForeColor;
            PointColor = ForeColor.Blend(Color.White, 0.6);
            CapturedColor = Color.FromArgb(158, 177, 195);

            cs.TextBrush = new SolidBrush(TextColor);
            cs.BackBrush = new SolidBrush(BGColor);

            var blend = new ColorBlend();
            blend.Positions = new float[] { 0, 0.5f, 1 };

            u.Chart.Inflate(1, 1);
            GradientBrush = new LinearGradientBrush(u.Chart, Color.Transparent, Color.Transparent, LinearGradientMode.Vertical);
            cs.PointBrush = new LinearGradientBrush(u.Chart, Color.Transparent, Color.Transparent, LinearGradientMode.Vertical);
            u.Chart.Inflate(-1, -1);

            blend.Colors = new Color[] { cs.LineColor, cs.LineColor, ForeColor.Blend(BGColor, 0.5) };
            GradientBrush.InterpolationColors = blend;

            blend.Colors = new Color[] { PointColor, PointColor, cs.LineColor };
            cs.PointBrush.InterpolationColors = blend;

            cs.ShadowBrush = new SolidBrush(BGColor.WithAlpha(200));
            cs.ShadowTextBrush = new SolidBrush(BackColor.Blend(Color.Black, 0.75).WithAlpha(224));
            cs.FrozenBrush = new SolidBrush(TextColor.Blend(Color.SkyBlue, 0.45));
            cs.ScrollBarBrush = new SolidBrush(Color.FromArgb(92, 92, 92));
            cs.ScrollBarHoverBrush = new SolidBrush(Color.FromArgb(122, 128, 132));
            cs.ScrollBarCapturedBrush = new SolidBrush(CapturedColor);

            cs.GradientPen = new Pen(GradientBrush);
            cs.XPen = new Pen(Color.FromArgb(20, 20, 20));
            cs.YPen = new Pen(Color.FromArgb(44, 44, 44));
            cs.CapturedPen = new Pen(CapturedColor.WithAlpha(150));

            return cs;
        }

        protected override void OnPaint(PaintEventArgs e) {
            if (Suspended) return;

            base.OnPaint(e);

            if (!HasAny) return;

            Units u = GetUnits();
            ColorSet cs = GetColorSet(u);

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
                PointColor = _ => KeyHistory.GetInfo(KeyHistory.Events[_].Input).Color;
                
                var blend = new ColorBlend();
                blend.Positions = new float[] { 0f, 0.1f, 0.3f, 1 };

                var height = u.YUnit - 2;
                RectangleF bar = new RectangleF(u.Chart.X, 0, u.Chart.Width, 0);

                var visible = KeyHistory.VisibleInputs().ToList();

                for (int i = 0; i < visible.Count; i++) {
                    e.Graphics.DrawShadowString(
                        InputAsString(visible[i].Input, u.MultipleSources),
                        Font,
                        Program.IsFrozen? cs.FrozenBrush : cs.TextBrush,
                        cs.ShadowTextBrush,
                        u.KeyText[i]
                    );

                    double y = u.Chart.Y + i * u.YUnit;

                    bar.Height = Math.Max(1, (float)(Math.Floor(y + height) - (y = Math.Floor(y))));
                    bar.Y = (float)(y + 0.5);

                    LinearGradientBrush brush = new LinearGradientBrush(bar, Color.Transparent, Color.Transparent, LinearGradientMode.Vertical);

                    var color = Color.FromArgb(250, visible[i].Color);
                    var topColor = Color.FromArgb(240, color.Blend(Color.Black, 0.98));
                    var bottomColor = Color.FromArgb(215, color.Blend(Color.Black, 0.95));

                    blend.Colors = new Color[] { topColor, color, color, bottomColor };
                    brush.InterpolationColors = blend;

                    var events = visible[i].Events;
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

            /* Captured */
            if (Captured && CapturedDirection != PanDirection.None) {
                float x = CapturedPoint.X;
                
                if (CapturedDirection == PanDirection.Vertical)
                    x = (float)Math.Round((CapturedXValue - u.Min) * u.XUnit + u.Chart.X);

                if (x.InRangeII(u.Chart.X, u.Chart.Right))
                    e.Graphics.DrawLine(cs.CapturedPen, x, u.Chart.Y, x, u.Chart.Bottom);
            }

            /* ScrollBar */
            {
                e.Graphics.FillRectangle(cs.BackBrush, u.ScrollBarJumpLeft);
                e.Graphics.FillRectangle(cs.BackBrush, u.ScrollBarJumpRight);

                bool GetScrollBarBrush(ScrollBarComponent component, out Brush brush) {
                    brush = null;

                    if (Hovering == component) brush = cs.ScrollBarHoverBrush;
                    if (CapturedScrollBar == component) brush = cs.ScrollBarCapturedBrush;

                    return brush != null;
                }

                e.Graphics.FillRectangle(cs.ScrollBarBrush, u.ScrollBar);

                Brush b;
                if (GetScrollBarBrush(ScrollBarComponent.ScrollBar, out b))
                    e.Graphics.FillRectangle(b, u.ScrollBar);

                if (GetScrollBarBrush(ScrollBarComponent.NotchLeft, out b))
                    e.Graphics.FillRectangle(b, u.NotchLeft);

                if (GetScrollBarBrush(ScrollBarComponent.NotchRight, out b))
                    e.Graphics.FillRectangle(b, u.NotchRight);

                if (Hovering != ScrollBarComponent.None || CapturedScrollBar != ScrollBarComponent.None) {
                    RectangleF notch = new RectangleF();
                    notch.Y = u.ScrollBar.Y + 1.6f;
                    notch.Width = 1.4f;
                    notch.Height = u.ScrollBar.Height - 3.2f;

                    notch.X = u.ScrollBar.X + 1.8f;
                    e.Graphics.FillRectangle(cs.BackBrush, notch);

                    notch.X = u.ScrollBar.Right - 1.8f - notch.Width;
                    e.Graphics.FillRectangle(cs.BackBrush, notch);
                }
            }
        }
    }
}
