using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Windows.Forms;
using Control = System.Windows.Forms.Control;
using System.Windows.Forms.DataVisualization.Charting;

using DarkUI.Controls;
using DarkUI.Forms;

using MathNet.Numerics;
using MathNet.Numerics.IntegralTransforms;
using MathNet.Numerics.Statistics;

namespace Keyboard_Inspector {
    partial class MainForm: DarkForm {
        public static MainForm Instance { get; private set; }

        // WiitarListener needs to grab WM_INPUT from Form...
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        protected override void WndProc(ref Message m) {
            if (WiitarListener.Process(ref m)) return;

            base.WndProc(ref m);
        }

        public MainForm() {
            if (Instance != null) throw new Exception("Can't have more than one MainForm");
            Instance = this;

            InitializeComponent();

            screen.AllowDrop = true;

            key.DropDown.Closing += (s, e) => e.Cancel = e.CloseReason == ToolStripDropDownCloseReason.ItemClicked;

            InitFileFormats();
        }

        int elapsed;
        Result result = null;
        bool hasResult => result?.Events.Any() == true;
        Scope scope = new Scope();
        double areaWidth, areaX;
        List<Input> inputs;
        List<RectangleF> textRects = new List<RectangleF>();
        Dictionary<Input, Color> colors = new Dictionary<Input, Color>();

        void processResult() {
            if (!hasResult) return;

            if (freeze.Checked && inputs?.Any() != true)
                freeze.Checked = false;

            if (!freeze.Checked) {
                inputs = result.Events.Select(i => i.Input).Distinct().ToList();
                colors.Clear();
            }
        }

        void resultLoaded() {
            string title = result?.GetTitle();
            Text = (string.IsNullOrWhiteSpace(title)? "" : $"{title} - ") + "Keyboard Inspector";

            key.Enabled = recording.Enabled = open.Enabled = !Recorder.IsRecording;
            save.Enabled = split.Visible = hasResult;

            Redraw();
            UpdateScroll(scroll, scope);

            labelN.Text = result?.Events.Count.ToString();

            CreateCharts();
        }

        void rec_Click(object sender, EventArgs e) {
            status.TextAlign = ContentAlignment.TopRight;

            if (!Recorder.IsRecording) {
                rec.Text = "Stop Recording";
                status.Text = "Recording... 00:00:00";
                split.Enabled = false; 

                result = null;

                Recorder.StartRecording();

                scope.Reset();

                elapsed = -1;
                t_Tick(sender, e);
                t.Enabled = true;

            } else {
                rec.Text = "Start Recording";
                status.Text = "";
                split.Enabled = true;

                result = Recorder.StopRecording();

                processResult();

                t.Enabled = false;
            }

            resultLoaded();
        }

        void t_Tick(object sender, EventArgs e) {
            status.TextAlign = ContentAlignment.TopRight;
            status.Text = $"Recording... {TimeSpan.FromSeconds(++elapsed):hh\\:mm\\:ss}";
        }

        void screen_MouseWheel(object sender, MouseEventArgs e) {
            if (result == null) return;
            if (!scope.ApplyWheel(e.Delta, (int)(e.X - areaX) / areaWidth)) return;
            
            Redraw();
            UpdateScroll(scroll, scope);
        }

        private void scroll_Scroll(object sender, ScrollValueEventArgs e) {
            if (result == null || updatingScroll) return;

            scope.ScrollBar(scroll);

            Redraw();
        }

        new const int Margin = 5;
        const int BarMargin = 1;

        double GetUnitIncrement(int index) {
            if (index <= 3) return Math.Pow(2, index) / 1000;
            if (index <= 12) {
                double ret = Math.Pow(10, (index / 3) - 3);
                if (index % 3 == 1) ret *= 2;
                if (index % 3 == 2) ret *= 5;
                return ret;
            }
            if (index <= 15) return Math.Pow(2, index - 14) * 60;
            if (index <= 17) return Math.Pow(2, index - 16) * 300;
            return Math.Pow(2, index - 18) * 1800;
        }

        void Redraw() {
            if (screen.Width <= 0 || screen.Height <= 0) return;

            Bitmap img = new Bitmap(screen.Width, screen.Height);

            if (result != null && inputs?.Any() == true) {
                textRects.Clear();

                Font font = status.Font;
                Brush textBrush = new SolidBrush(Color.FromArgb(160, 160, 160));
                Pen penMajor = new Pen(Color.FromArgb(20, 20, 20));
                Pen penMinor = new Pen(Color.FromArgb(48, 48, 48));

                bool multipleSources = inputs.Select(i => i.Source).Distinct().Count() > 1;

                using (Graphics gfx = Graphics.FromImage(img)) {
                    gfx.FillRectangle(new SolidBrush(screen.BackColor), 0, 0, screen.Width, screen.Height);

                    SizeF[] textSize = inputs.Select(i => gfx.MeasureString(i.ToString(multipleSources), status.Font)).ToArray();
                    float textWidth = textSize.Max(i => i.Width);
                    float textHeight = textSize[0].Height;
                    areaX = 2 * Margin + textWidth;
                    areaWidth = screen.Width - Margin - areaX;

                    float keyHeight = (float)(screen.Height - 3 * Margin - textHeight) / inputs.Count;

                    for (int k = 0; k < inputs.Count; k++) {
                        RectangleF stringRect = new RectangleF(
                            new PointF(
                                Margin + textWidth - textSize[k].Width,
                                Margin + keyHeight * k + (keyHeight - textHeight) / 2
                            ),
                            textSize[k]
                        );

                        textRects.Add(new RectangleF(
                            new PointF(
                                Margin + textWidth - textSize[k].Width,
                                Margin + keyHeight * k
                            ),
                            new SizeF(
                                textSize[k].Width,
                                keyHeight
                            )
                        ));

                        gfx.DrawString(
                            inputs[k].ToString(multipleSources), font, textBrush,
                            stringRect.X, stringRect.Y
                        );

                        gfx.DrawLine(
                            penMinor,
                            2 * Margin + textWidth,
                            Margin + keyHeight * (k + 0.5f),
                            screen.Width - Margin,
                            Margin + keyHeight * (k + 0.5f)
                        );
                    }

                    int incIndex = 8; // 0.5s
                    double increment;
                    double px;

                    for (;;) {
                        increment = GetUnitIncrement(incIndex);

                        px = increment / result.Time * areaWidth * scope.Zoom;

                        if (px < 30) incIndex++;
                        else if (px >= 90 && incIndex > 0) incIndex--;
                        else break;
                    }

                    double pos = scope.Viewport * result.Time / increment;

                    int posCeil = (int)Math.Ceiling(pos);

                    for (float s = (float)((posCeil - pos) * px); s < areaWidth; s = (float)((++posCeil - pos) * px)) {
                        gfx.DrawLine(
                            penMajor,
                            2 * Margin + textWidth + s,
                            Margin,
                            2 * Margin + textWidth + s,
                            screen.Height - 2 * Margin - textHeight
                        );

                        string t = (increment * posCeil).ToString("0.###");

                        gfx.DrawString(
                            t, font, textBrush,
                            2 * Margin + textWidth + s - gfx.MeasureString(t, font).Width / 2,
                            screen.Height - Margin - textHeight
                        );
                    }

                    for (int k = 0; k < inputs.Count; k++) {
                        Event[] events = result.Events.Where(i => i.Input == inputs[k]).ToArray();
                        Brush brush = new SolidBrush(colors.ContainsKey(inputs[k])? colors[inputs[k]] : inputs[k].DefaultColor);

                        if (events.Any()) {
                            void drawBar(double start, double end) {
                                start = (start - scope.Viewport * result.Time) * scope.Zoom / result.Time;
                                end = (end - scope.Viewport * result.Time) * scope.Zoom / result.Time;

                                if (start <= 0) start = 0;
                                if (start >= 1) return;
                                if (end <= 0) return;
                                if (end >= 1) end = 1;

                                gfx.FillRectangle(
                                    brush,
                                    2 * Margin + textWidth + (float)(start * areaWidth),
                                    Margin + keyHeight * k + BarMargin,
                                    (float)((end - start) * areaWidth),
                                    keyHeight - 2 * BarMargin
                                );
                            }

                            int e = 0;
                            if (!events[0].Pressed) {
                                e = 1;
                                drawBar(0, events[0].Time);
                            }

                            while (e < events.Length) {
                                int f;
                                for (f = e + 1; f < events.Length && events[f].Pressed; f++);

                                drawBar(
                                    events[e].Time,
                                    f < events.Length ? events[f].Time : result.Time
                                );

                                e = f + 1;
                            }
                        }
                    }

                    gfx.Flush();
                }
            }

            screen.Image = img;
            Update();
        }

        bool IntersectKey(Point pt, out int result) {
            result = -1;

            for (int i = 0; i < textRects.Count; i++) {
                if (textRects[i].Contains(pt)) {
                    result = i;
                    return true;
                }
            }

            return false;
        }

        void screen_MouseMove(object sender, MouseEventArgs e) {
            if (result == null) return;

            screen.Cursor = IntersectKey(e.Location, out _)
                ? Cursors.NoMoveVert
                : Cursors.Default;
        }

        void screen_MouseClick(object sender, MouseEventArgs e) {
            if (result == null || e.Button != MouseButtons.Right) return;

            if (IntersectKey(e.Location, out int i)) {
                screen.Cursor = Cursors.Default;

                keymenu.Tag = i;
                keymenu.Show(screen.PointToScreen(e.Location));
            }
        }

        void color_Click(object sender, EventArgs e) {
            if (!(keymenu.Tag is int i)) return;

            ColorDialog cd = new ColorDialog();
            cd.Color = colors.ContainsKey(inputs[i])? colors[inputs[i]] : inputs[i].DefaultColor;

            if (cd.ShowDialog() == DialogResult.OK) {
                colors[inputs[i]] = cd.Color;
                Redraw();
            }
        }

        void hide_Click(object sender, EventArgs e) {
            if (!(keymenu.Tag is int i)) return;

            inputs.RemoveAt(i);
            Redraw();
        }

        void unhide_Click(object sender, EventArgs e) {
            if (sender is ToolStripMenuItem item && item.GetCurrentParent() is ToolStripDropDownMenu menu)
                menu.Close();

            if (result == null)
                return;

            IEnumerable<Input> unhidden = result.Events.Select(i => i.Input);

            if (freeze.Checked)
                unhidden = inputs.Concat(unhidden);

            inputs = unhidden.Distinct().ToList();

            Redraw();
        }

        void screen_MouseDown(object sender, MouseEventArgs e) {
            if (result == null || e.Button != MouseButtons.Left) return;

            if (IntersectKey(e.Location, out int i))
                screen.DoDragDrop(i, DragDropEffects.Move);
        }

        bool ValidateDrag(DragEventArgs e, out int result) {
            result = -1;
            if (!e.Data.GetDataPresent("System.Int32")) return false;

            result = (int)e.Data.GetData("System.Int32");
            return !(result < 0 || textRects.Count <= result);
        }

        void screen_DragOver(object sender, DragEventArgs e) {
            e.Effect = DragDropEffects.None;

            if (result == null || !ValidateDrag(e, out _)) return;

            if (IntersectKey(screen.PointToClient(new Point(e.X, e.Y)), out _))
                e.Effect = DragDropEffects.Move;
        }

        void screen_DragDrop(object sender, DragEventArgs e) {
            if (result == null || !ValidateDrag(e, out int d)) return;

            if (IntersectKey(screen.PointToClient(new Point(e.X, e.Y)), out int i)) {
                Input k = inputs[d];
                inputs.RemoveAt(d);
                inputs.Insert(i, k);

                Redraw();
            }
        }

        private void screen_SizeChanged(object sender, EventArgs e)
            => Redraw();

        bool EstimatePeak(IEnumerable<double> data, out int result) {
            double max = double.MaxValue;
            bool search = false;
            result = -1;
            
            int i = 0;
            foreach (var v in data) {
                if (search) {
                    if (v > max) {
                        max = v;
                        result = i;
                    }
                } else {
                    if (v > max) search = true;
                    max = v;
                }
                i++;
            }
            
            return result != -1;
        }

        void CalcDelta()
            => deltaCache = result?.Events
                ?.Skip(1)
                ?.Select((x, i) => x.Time - result.Events[i].Time)
                ?.ToList();

        IEnumerable<int> GetDiffs()
            => deltaCache.Select(i => (int)Math.Round(i * precision));

        IEnumerable<int> GetCompound() {
            if (result == null) yield break;

            for (int i = 0; i < result.Events.Count - 1; i++) {
                for (int j = i + 1; j < result.Events.Count; j++) {
                    int diff = (int)Math.Round((result.Events[j].Time - result.Events[i].Time) * precision);

                    if (j > i + 1 && diff >= precision)
                        break;

                    yield return diff;
                }
            }
        }

        IEnumerable<int> GetCircular()
            => result?.Events.Select(x => ((int)Math.Round(x.Time * precision)) % precision);
            
        void RunFitter(int col) {
            Control stddevCtrl = fitter.GetControlFromPosition(col, 1);
            Control amountCtrl = fitter.GetControlFromPosition(col, 2);

            double.TryParse(fitter.GetControlFromPosition(col, 0).Text, out double hz);

            if (hz <= 0) {
                stddevCtrl.Text = amountCtrl.Text = "";
                return;
            }
            
            double stddev, amount;

            // Assume polling rate
            double ms = 1 / hz;

            // Offset from closest poll on assumed polling rate
            List<double> off = deltaCache.Select(j => ((j + ms / 2) % ms - ms / 2)).ToList();

            // Remove outliers (two values next to each other that are odd, but cancel themselves out)
            List<double> no_outliers = off.Take(3).ToList();

            for (int i = 3; i < off.Count; i++) {
                if (Math.Abs(off[i - 2]) > ms / 6 && Math.Abs(off[i]) < ms / 10 && Math.Abs(off[i - 3]) < ms / 10) {
                    double fix = (off[i - 2] + off[i - 1] + 3 * ms) % ms;

                    if (Math.Abs(fix) < ms / 10) {
                        no_outliers.RemoveAt(no_outliers.Count - 1);
                        no_outliers.RemoveAt(no_outliers.Count - 2);

                        no_outliers.Add(fix);
                    }
                }
                no_outliers.Add(off[i]);
            }
            
            // Standard deviation
            stddev = no_outliers.StandardDeviation();

            // Amount of samples close to poll rate
            amount = (double)no_outliers.Count(j => Math.Abs(j) < ms / 6) / no_outliers.Count;

            stddevCtrl.Text = stddev.ToString("0.00E0");
            amountCtrl.Text = amount.ToString("0.00%");

            stddevCtrl.ForeColor = stddev < ms / 4? Color.Chartreuse : Color.Crimson;
            amountCtrl.ForeColor = amount >= 0.8? Color.Chartreuse : Color.Crimson;
        }

        void fitterCustomHz_TextChanged(object sender, EventArgs e) {
            if (!hasResult || deltaCache == null) return;
            RunFitter(fitter.ColumnCount - 1);
        }

        void HighPass(double[] data) {
            // https://www.desmos.com/calculator/yukhgjz5g9
            for (int i = 0; i < Math.Min(30, data.Length); i++)
                data[i] /= 1 + Math.Pow(Math.E, -(i - 25) / 4.0);
        }

        void HarmonicProduct(double[] data) {
            if (hpsIterations <= 0) return;

            double[] copy = data.ToArray();

            for (int i = 0; i < data.Length; i++) {
                for (var j = 0; j < hpsIterations; j++) {
                    data[i] *= copy[(int)((double)i * (j + 1) / (hpsIterations + 1))];
                }
            }
        }

        void Normalize(double[] data) {
            double max = data.Max();

            for (int i = 0; i < data.Length; i++)
                data[i] /= max;
        }

        void DrawGraph(Chart chart, IEnumerable<double> data, string title, double xFactor = 1) {
            chart.Series[0].Points.Clear();

            int i = 0;
            foreach (double v in data)
                chart.Series[0].Points.AddXY(i++ * xFactor, v);

            chart.Titles.First().Text = title;
        }

        Dictionary<Chart, double[]> beforeHPS = new Dictionary<Chart, double[]>();

        void RunGraphJob(IEnumerable<int> source, Chart timeDomain, Chart freqDomain, Func<IEnumerable<double>, IEnumerable<double>> timeTransform = null) {
            timeTransform = timeTransform?? (i => i);
            
            Complex32[] data = new Complex32[precision];

            foreach (var g in source.GroupBy(i => i).Where(i => i.Key < precision))
                data[g.Key] = g.Count();

            DrawGraph(timeDomain, timeTransform(data.Select(i => (double)i.Real)), $"{timeDomain.Tag as string} (time domain)", 1000.0 / precision);
            ChartApplyScope(timeDomain, new Scope(), 100);

            Fourier.Forward(data);
            double[] isolated = data.Take(data.Length / 2 + 1).Select(i => (double)i.Magnitude).ToArray();

            HighPass(isolated);
                
            beforeHPS[freqDomain] = isolated.ToArray();
            RunFromHPS(freqDomain);
        }

        void RunFromHPS(Chart freqDomain) {
            double[] data = beforeHPS[freqDomain].ToArray();

            HarmonicProduct(data);
            Normalize(data);

            double hpsFactor = 1.0 / (hpsIterations + 1);
            double max = (data.Length - 1) * hpsFactor;

            Axis a = freqDomain.ChartAreas.First().AxisX;
            a.Interval = a.MajorGrid.Interval = a.MajorTickMark.Interval = max / 10;
            a.MinorGrid.Interval = max / 50;

            string estimate = "";

            if (EstimatePeak(data, out int result))
                estimate = $" - peak at {(int)Math.Round(result * hpsFactor)} Hz";

            DrawGraph(freqDomain, data, $"{freqDomain.Tag as string} (frequency domain{estimate})", hpsFactor);
            ChartApplyScope(freqDomain, new Scope());
        }

        bool silentAnalysis = false;

        void tbPrecision_TextChanged(object sender, EventArgs e) {
            if (silentAnalysis) return;
            CreateCharts();
        }

        private void hps_ValueChanged(object sender, EventArgs e) {
            if (silentAnalysis) return;

            RunFromHPS(chartDiffsFreq);
            RunFromHPS(chartCompoundFreq);
            RunFromHPS(chartCircularFreq);
        }

        int precision = 4000;
        int hpsIterations => (int)hps.Value;
        List<double> deltaCache;

        void CreateCharts() {
            var panels = tlpCharts.Controls.OfType<Panel>();

            foreach (var chart in panels.SelectMany(i => i.Controls.OfType<Chart>()))
                chart.Series[0].Points.Clear();

            var defaultScope = new Scope();
            foreach (var scroll in panels.SelectMany(i => i.Controls.OfType<DarkScrollBar>()))
                UpdateScroll(scroll, defaultScope);

            if (!hasResult) return;

            int.TryParse(tbPrecision.Text, out precision);

            CalcDelta();

            for (int i = 1; i < fitter.ColumnCount; i++)
                RunFitter(i);

            if (precision <= 0) return;

            RunGraphJob(GetDiffs(), chartDiffs, chartDiffsFreq);
            RunGraphJob(GetCompound(), chartCompound, chartCompoundFreq);
            RunGraphJob(GetCircular(), chartCircular, chartCircularFreq, arr => {
                double max = double.MinValue;
                int rot = -1;

                int i = 0;
                foreach (double v in arr) {
                    if (v > max) {
                        max = v;
                        rot = i;
                    }
                    i++;
                }

                return arr.Concat(arr).Skip(rot);
            });
        }

        private void precisionDouble_Click(object sender, EventArgs e) {
            if (precision <= 0) return;
            precision *= 2;
            tbPrecision.Text = precision.ToString();
        }

        private void precisionHalf_Click(object sender, EventArgs e) {
            if (precision <= 0) return;
            precision /= 2;
            tbPrecision.Text = precision.ToString();
        }

        void ChartApplyScope(Chart c, Scope scope, double? maxOverride = null) {
            Axis a = c.ChartAreas.First().AxisX;
            double max = maxOverride?? c.Series[0].Points.Last().XValue;

            a.Minimum = scope.Viewport * max;
            a.Maximum = max / scope.Zoom + a.Minimum;
            a.IntervalOffset = (-a.Minimum) % a.Interval;
            a.MinorGrid.IntervalOffset = (-a.Minimum) % a.MinorGrid.Interval;
        }

        bool updatingScroll = false;

        void UpdateScroll(DarkScrollBar scroll, Scope scope) {
            try {
                updatingScroll = true;

                scroll.Enabled = hasResult;
                if (!hasResult) return;

                scroll.ViewSize = (int)(scroll.Maximum / scope.Zoom);

                if (scroll.ViewSize == scroll.Maximum)
                    scroll.ViewSize -= 1;

                scroll.Value = (int)(scroll.Maximum * scope.Viewport);

            } finally {
                updatingScroll = false;
            }
        }

        void chart_MouseWheel(object sender, MouseEventArgs e) {
            if (result == null || precision <= 0) return;

            Chart c = sender as Chart;
            Axis a = c.ChartAreas.First().AxisX;

            double width = a.Maximum - a.Minimum;
            double x = (a.PixelPositionToValue(e.X) - a.Minimum) / width;

            double max = c.Series[0].Points.Last().XValue;

            Scope scope = new Scope(max / width, a.Minimum / max);
            if (!scope.ApplyWheel(e.Delta, x)) return;

            ChartApplyScope(c, scope);
            UpdateScroll(c.Parent.Controls.OfType<DarkScrollBar>().Single(), scope);
        }

        private void chart_Scroll(object sender, ScrollValueEventArgs e) {
            if (result == null || precision <= 0 || updatingScroll) return;

            DarkScrollBar scroll = sender as DarkScrollBar;
            Chart c = scroll.Parent.Controls.OfType<Chart>().Single();
            Axis a = c.ChartAreas.First().AxisX;

            double max = c.Series[0].Points.Last().XValue;
            double zoom = (a.Maximum - a.Minimum) / max;

            double viewport = (double)scroll.Value / scroll.Maximum;

            a.Minimum = viewport * max;
            a.Maximum = zoom * max + a.Minimum;
            a.IntervalOffset = (-a.Minimum) % a.Interval;
            a.MinorGrid.IntervalOffset = (-a.Minimum) % a.MinorGrid.Interval;
        }

        private void chart_MouseDoubleClick(object sender, MouseEventArgs e) {
            if (e.Button != MouseButtons.Left) return;

            var panel = (sender as Chart).Parent as Panel;

            if (panel.Parent == tlpCharts) {
                tlpCharts.Controls.Remove(panel);
                split.Panel1.Controls.Add(panel);
                split.Panel1.Controls.SetChildIndex(panel, 0);

            } else if (panel.Parent == split.Panel1) {
                split.Panel1.Controls.Remove(panel);
                tlpCharts.Controls.Add(panel);
            }
        }

        FileFormat[] fileFormats;
        OpenFileDialog ofd;
        SaveFileDialog sfd;

        void InitFileFormats() {
            fileFormats = new FileFormat[] {
                new FileFormat("Keyboard Inspector Files", new string[] { "kbi" },
                    path => {
                        using (MemoryStream ms = new MemoryStream(File.ReadAllBytes(path))) {
                            using (BinaryReader br = new BinaryReader(ms)) {
                                return Result.FromBinary(br);
                            }
                        }
                    },
                    (result, path) => {
                        using (MemoryStream ms = new MemoryStream()) {
                            using (BinaryWriter bw = new BinaryWriter(ms)) {
                                result.ToBinary(bw);
                                File.WriteAllBytes(path, ms.ToArray());
                            }
                        }
                    }
                ),
                new FileFormat("TETR.IO Replay Files", new string[] { "ttr", "ttrm" },
                    path => {
                        result = TetrioReplay.ConvertToResult(path);

                        if (result != null) {
                            silentAnalysis = true;
                            tbPrecision.Text = "600";
                            silentAnalysis = false;
                        }

                        return result;
                    },
                    disclaimer:
                    "You are analyzing a TETR.IO replay file.\n\nTETR.IO downsamples input data to 600 Hz to fit it onto the subframe grid which " +
                    "you will notice as a peak at 600 Hz in the frequency domain. This adds another \"sampling rate\" (in addition to the usual " +
                    "USB poll rate and matrix scan rate) to the process.\n\nThis is unlike a regular Keyboard Inspector recording which tries to " +
                    "get the most accurate real-time timestamp it can without any additional downsampling.\n\nYou may still analyze the recording, " +
                    "but be vary of the limitations of the TETR.IO replay format."
                ),
            };

            ofd = new OpenFileDialog() {
                Filter = FileFormat.GetOpenFilter(fileFormats),
                Title = "Open Recording"
            };

            sfd = new SaveFileDialog() {
                Filter = FileFormat.GetSaveFilter(fileFormats),
                Title = "Save Recording"
            };
        }

        void LoadFile(string filename) {
            try {
                result = fileFormats.First(i => i.Match(filename)).Read(filename);

                scope.Reset();

                processResult();
                resultLoaded();

            } catch {
                status.Text = "Couldn't parse file, it is likely corrupt or in an unsupported format.";
                
                #if DEBUG
                    throw;
                #endif
            }
        }

        void SaveFile(string filename) {
            try {
                fileFormats.First(i => i.Match(filename)).Write(result, filename);

            } catch {
                status.Text = "Couldn't save file, try saving it to a different location or with a different file name.";

                #if DEBUG
                    throw;
                #endif
            }
        }

        void open_Click(object sender, EventArgs e) {
            if (ofd.ShowDialog() == DialogResult.OK)
                LoadFile(ofd.FileName);
        }

        void save_Click(object sender, EventArgs e) {
            if (hasResult && sfd.ShowDialog() == DialogResult.OK)
                SaveFile(sfd.FileName);
        }

        bool ValidateFileDrag(DragEventArgs e, out string result) {
            result = null;
            if (!e.Data.GetDataPresent(DataFormats.FileDrop)) return false;

            var arr = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (arr.Length != 1) return false;

            result = arr[0];
            return fileFormats.Any(i => i.Match(arr[0]));
        }

        private void MainForm_DragOver(object sender, DragEventArgs e) {
            e.Effect = DragDropEffects.None;

            if (!ValidateFileDrag(e, out _)) return;

            e.Effect = DragDropEffects.Move;
        }

        private void MainForm_DragDrop(object sender, DragEventArgs e) {
            if (!ValidateFileDrag(e, out string filename)) return;

            LoadFile(filename);
        }

        void MainForm_FormClosing(object sender, FormClosingEventArgs e) {
            if (Recorder.IsRecording) {
                e.Cancel = true;
                return;
            }
        }
    }
}