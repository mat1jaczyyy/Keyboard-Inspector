using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

using MathNet.Numerics.Statistics;

namespace Keyboard_Inspector {
    partial class MainForm: Form {
        public static MainForm Instance { get; private set; }

        // WiitarListener needs to grab WM_INPUT from Form...
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        protected override void WndProc(ref Message m) {
            if (WiitarListener.Process(ref m)) return;

            base.WndProc(ref m);
        }

        public static void InvokeIfRequired(Control control, MethodInvoker action) {
            if (control.InvokeRequired) control.Invoke(action);
            else action();
        }

        void CancelDropDownClose(object sender, ToolStripDropDownClosingEventArgs e) {
            if (e.CloseReason == ToolStripDropDownCloseReason.ItemClicked)
                e.Cancel = true;
        }

        public MainForm() {
            if (Instance != null) throw new Exception("Can't have more than one MainForm");
            Instance = this;

            InitializeComponent();

            screen.AllowDrop = true;

            key.DropDown.Closing += CancelDropDownClose;
            gridView.DropDown.Closing += CancelDropDownClose;
        }

        int elapsed;
        Result result = null;
        double zoom, viewport;
        float areaWidth, areaX;
        List<Input> inputs;
        List<RectangleF> textRects = new List<RectangleF>();
        Dictionary<Input, Color> colors = new Dictionary<Input, Color>();

        void processResult() {
            if (!freeze.Checked) {
                inputs = result.Events.Select(i => i.Input).Distinct().ToList();
                colors.Clear();
            }
        }

        void resultLoaded() {
            integrations.Enabled = key.Enabled = recording.Enabled = open.Enabled = !Recorder.IsRecording;
            poll.Enabled = (result?.Events.All(i => i.Input is KeyInput) == true || result?.Events.All(i => i.Input is WiitarInput) == true) && result?.Events.Count >= 60;
            save.Enabled = result?.Events.Any() == true;

            Redraw();
            UpdateScroll();
        }

        void rec_Click(object sender, EventArgs e) {
            status.TextAlign = ContentAlignment.TopRight;

            if (!Recorder.IsRecording) {
                rec.Text = "Stop Recording";
                status.Text = "Recording... 00:00:00";

                result = null;

                Recorder.StartRecording();

                zoom = 1;
                viewport = 0;

                elapsed = -1;
                t_Tick(sender, e);
                t.Enabled = true;

            } else {
                rec.Text = "Start Recording";
                status.Text = "";

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

        void MainForm_Resize(object sender, EventArgs e) => Redraw();

        void screen_MouseWheel(object sender, MouseEventArgs e) {
            if (result == null) return;

            int x = (int)(e.X - areaX);
            if (x < 0 || areaWidth <= x) return;

            double s = 1 / zoom;

            double change = Math.Pow(1.05, e.Delta / 120.0);
            zoom *= change;

            if (zoom <= 1) {
                zoom = 1;
                viewport = 0;

            } else {
                if (zoom > 200000) {
                    zoom = 200000;
                    change = zoom * s;
                }

                double v = x / areaWidth * (1 - 1 / change);
                viewport += v * s;

                if (viewport < 0) viewport = 0;
                else {
                    double t = 1 / zoom;
                    if (viewport + t > 1) viewport = 1 - t;
                }
            }

            Redraw();
            UpdateScroll();
        }

        private void scroll_Scroll(object sender, ScrollEventArgs e) {
            viewport = (double)scroll.Value / scroll.Maximum;

            Redraw();
        }

        void UpdateScroll() {
            scroll.Enabled = result != null;

            if (result == null) return;

            scroll.LargeChange = (int)(scroll.Maximum / zoom);
            scroll.Value = (int)(scroll.Maximum * viewport);
        }

        new const int Margin = 5;
        const int BarMargin = 1;

        void Redraw() {
            if (screen.Width <= 0 || screen.Height <= 0) return;

            Bitmap img = new Bitmap(screen.Width, screen.Height);

            if (result != null && inputs.Any()) {
                textRects.Clear();

                Font font = status.Font;
                Brush textBrush = new SolidBrush(status.ForeColor);
                Pen pen = new Pen(Color.LightGray);
                Pen pollPen = new Pen(Color.Blue);

                bool multipleSources = inputs.Select(i => i.Source).Distinct().Count() > 1;

                using (Graphics gfx = Graphics.FromImage(img)) {
                    gfx.FillRectangle(new SolidBrush(screen.BackColor), 0, 0, screen.Width, screen.Height);

                    SizeF[] textSize = inputs.Select(i => gfx.MeasureString(i.ToString(multipleSources), status.Font)).ToArray();
                    float textWidth = textSize.Max(i => i.Width);
                    float textHeight = textSize[0].Height;
                    areaX = 2 * Margin + textWidth;
                    areaWidth = screen.Width - Margin - areaX;

                    float increment = 0.5f;
                    float px;

                    for (;;) {
                        px = (float)(increment / result.Time * areaWidth * zoom);

                        if (px < 40) increment *= 2;
                        else if (px >= 80) increment /= 2;
                        else break;
                    }

                    double pos = viewport * result.Time / increment;
                    
                    if (realtimeGrid.Checked) {
                        int k = (int)Math.Ceiling(pos);

                        for (float s = (float)((k - pos) * px); s < areaWidth; s = (float)(++k - pos) * px) {
                            gfx.DrawLine(
                                pen,
                                2 * Margin + textWidth + s,
                                Margin,
                                2 * Margin + textWidth + s,
                                screen.Height - 2 * Margin - textHeight
                            );

                            string t = (increment * k).ToString("0.####");

                            gfx.DrawString(
                                t, font, textBrush,
                                2 * Margin + textWidth + s - gfx.MeasureString(t, font).Width / 2,
                                screen.Height - Margin - textHeight
                            );
                        }
                    }

                    float keyHeight = (float)(screen.Height - 3 * Margin - textHeight) / inputs.Count;

                    for (int k = 0; k < inputs.Count; k++) {
                        textRects.Add(new RectangleF(new PointF(
                            Margin + textWidth - textSize[k].Width,
                            Margin + keyHeight * k + (keyHeight - textHeight) / 2
                        ), textSize[k]));

                        gfx.DrawString(
                            inputs[k].ToString(multipleSources), font, textBrush,
                            textRects[k].X, textRects[k].Y
                        );

                        gfx.DrawLine(
                            pen,
                            2 * Margin + textWidth,
                            Margin + keyHeight * (k + 0.5f),
                            screen.Width - Margin,
                            Margin + keyHeight * (k + 0.5f)
                        );
                    }

                    if (nesPollsGrid.Checked) {
                        for (int j = 0; j < result.Polls.Count; j++) {
                            // TODO optimize...?
                            if (result.Polls[j] < increment * pos) continue;
                            if (result.Polls[j] > increment * (pos + areaWidth / px)) break;

                            gfx.DrawLine(
                                pollPen,
                                2 * Margin + textWidth + (float)((result.Polls[j] - increment * pos) * (px / increment)),
                                Margin,
                                2 * Margin + textWidth + (float)((result.Polls[j] - increment * pos) * (px / increment)),
                                screen.Height - 2 * Margin - textHeight
                            );
                        }
                    }

                    for (int k = 0; k < inputs.Count; k++) {
                        Event[] events = result.Events.Where(i => i.Input == inputs[k]).ToArray();
                        Brush brush = new SolidBrush(colors.ContainsKey(inputs[k])? colors[inputs[k]] : inputs[k].DefaultColor);

                        if (events.Any()) {
                            void drawBar(double start, double end) {
                                start = (start - viewport * result.Time) * zoom / result.Time;
                                end = (end - viewport * result.Time) * zoom / result.Time;

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

        static readonly double[] PollRates = new double[] {62.5, 125, 250, 500, 1000, 2000, 4000};

        void analyze_Click(object sender, EventArgs e) {
            if (result == null) return;

            StringBuilder file = new StringBuilder();

            // Filter Windows auto-repeat
            List<Event> no_repeat = result.Events
                .Where(i => i.Input is KeyInput || i.Input is WiitarInput)
                .Where((x, i) => !x.Pressed || !(result.Events.Take(i).Where(j => j.Input == x.Input).LastOrDefault()?.Pressed?? false))
                .ToList();

            file.Append($"no_repeat:\n{string.Join("\n", no_repeat)}\n\n");

            // Get differences between inputs
            List<double> delta = no_repeat
                .Skip(1)
                .Select((x, i) => x.Time - no_repeat[i].Time)
                .ToList();

            file.Append($"delta:\n{string.Join("\n", delta.Select(i => i.ToString("0.00000")))}\n\n");

            List<double> stddev = new List<double>();
            List<double> amount = new List<double>();

            // Assume polling rate
            foreach (double hz in PollRates) {
                file.Append($"Testing {hz}Hz\n\n\t");

                double ms = 1 / hz;

                // Offset from closest poll on assumed polling rate
                List<double> off = delta.Select(j => ((j + ms / 2) % ms - ms / 2)).ToList();

                // Remove outliers (two values next to each other that are odd, but cancel themselves out)
                List<double> no_outliers = new List<double>() { off[0], off[1], off[2] };

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

                file.Append($"off:\n\t{string.Join("\n\t", off.Select(i => i.ToString("0.00000")))}\n\n\t");
                file.Append($"no_outliers:\n\t{string.Join("\n\t", no_outliers.Select(i => i.ToString("0.00000")))}\n\n\t");

                // Amount of samples close to poll rate
                amount.Add((double)no_outliers.Count(j => Math.Abs(j) < ms / 6) / no_outliers.Count);

                // Standard deviation
                stddev.Add(no_outliers.StandardDeviation());

                file.Append($"amount: {amount.Last() * 100:0.00}%\n\t");
                file.Append($"stddev: {stddev.Last():0.00000000} {stddev.Last() < ms / 4}\n\n");
            }

            List<bool> stddev_results = stddev.Select((x, i) => x < (1 / PollRates[i]) / 4).ToList();
            List<bool> amount_results = amount.Select(i => i >= 0.8).ToList();

            int stddev_result = stddev_results.TakeWhile(i => !i).Count();
            int amount_result = amount_results.TakeWhile(i => !i).Count();

            bool stddev_hasresult = stddev_result < PollRates.Length && !stddev_results.Skip(stddev_result).Take(2).Any(i => !i);
            bool amount_hasresult = amount_result < PollRates.Length && !amount_results.Skip(amount_result).Take(2).Any(i => !i);

            int results = Convert.ToInt32(stddev_hasresult) + Convert.ToInt32(amount_hasresult);

            file.Append($"stddev_results: {string.Join(", ", stddev_results)}\n");
            file.Append($"stddev_result: {stddev_result}\n");
            file.Append($"stddev_hasresult: {amount_hasresult}\n\n");

            file.Append($"amount_results: {string.Join(", ", amount_results)}\n");
            file.Append($"amount_result: {amount_result}\n");
            file.Append($"amount_hasresult: {amount_hasresult}\n\n");

            file.Append($"results: {results}\n");

            status.TextAlign = ContentAlignment.TopLeft;

            if (results == 2) {
                if (stddev_result == amount_result) {
                    status.Text = $"Definitely {PollRates[stddev_result]}Hz!";
                } else {
                    status.Text = $"Either {PollRates[stddev_result]}Hz or {PollRates[amount_result]}Hz... Try again with more data.";
                }
            } else if (results == 1) {
                status.Text = $"Not sure, maybe {PollRates[stddev_hasresult ? stddev_result : amount_result]}Hz? Try again with more data.";
            } else {
                status.Text = $"Wasn't able to tell at all... Try again with more data.";
            }

            file.Append($"{status.Text}\n");

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Text Files (*.txt)|*.txt";
            sfd.Title = "Export Polling Rate Details";

            if (sender == export && sfd.ShowDialog() == DialogResult.OK)
                File.WriteAllText(sfd.FileName, file.ToString());
        }

        void open_Click(object sender, EventArgs e) {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "XML Files (*.xml)|*.xml";
            ofd.Title = "Open Recording";

            if (ofd.ShowDialog() == DialogResult.OK) {
                try {
                    XmlDocument xml = new XmlDocument();
                    xml.LoadXml(File.ReadAllText(ofd.FileName));

                    result = Result.FromXML(xml.GetNode("r"));

                    zoom = 1;
                    viewport = 0;

                    processResult();
                    resultLoaded();

                } catch {
                    status.Text = "Couldn't load file, it might be invalid?";
                }
            }
        }

        void save_Click(object sender, EventArgs e) {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "XML Files (*.xml)|*.xml";
            sfd.Title = "Save Recording";

            if (sfd.ShowDialog() == DialogResult.OK)
                File.WriteAllText(sfd.FileName, result.ToXML().ToString());
        }

        async void ConnectNestopia(object sender, EventArgs e) {
            if (Recorder.IsRecording) return;

            integrations.Enabled = recording.Enabled = rec.Enabled = false;

            status.TextAlign = ContentAlignment.TopLeft;
            status.Text = "Connecting to Nestopia...";

            Process nestopia = Process.GetProcessesByName("nestopia").FirstOrDefault();

            if (nestopia != null) {
                NestopiaListener.AttachResult result = await NestopiaListener.AttachDebugger(nestopia.Id);

                if (result == NestopiaListener.AttachResult.SUCCESS) {
                    status.Text = "Connected to Nestopia!";

                } else if (result == NestopiaListener.AttachResult.FAILED) {
                    status.Text = "Failed to connect Nestopia!";

                } else if (result == NestopiaListener.AttachResult.BAD_VER) {
                    status.Text = "Incorrect Nestopia version (use 1.50)";
                }

            } else status.Text = "Couldn't find a Nestopia process!";

            status.TextAlign = ContentAlignment.TopLeft;

            integrations.Enabled = recording.Enabled = rec.Enabled = true;
        }

        async Task DisconnectNestopia(object sender, EventArgs e) {
            if (Recorder.IsRecording) return;

            status.TextAlign = ContentAlignment.TopLeft;
            status.Text = "Disconnecting from Nestopia...";

            await NestopiaListener.DetachDebugger();

            NestopiaWasDisconnected();
        }

        public void NestopiaWasDisconnected()
            => InvokeIfRequired(status, () => {
                status.TextAlign = ContentAlignment.TopLeft;
                status.Text = "Disconnected from Nestopia.";
            });

        void integrations_DropDownOpening(object sender, EventArgs e) {
            if (Recorder.IsRecording) return;

            ToolStripMenuItem nestopia = new ToolStripMenuItem();

            if (NestopiaListener.IsConnected) {
                nestopia.Text = "Disconnect from &Nestopia";
                nestopia.Click += async (_, __) => await DisconnectNestopia(_, __);

            } else {
                nestopia.Text = "Connect to &Nestopia";
                nestopia.Click += ConnectNestopia;
            }

            integrations.DropDownItems.Add(nestopia);
        }

        void integrations_DropDownClosed(object sender, EventArgs e)
            => integrations.DropDownItems.Clear();

        void gridViewItem_Click(object sender, EventArgs e)
            => Redraw();

        async void MainForm_FormClosing(object sender, FormClosingEventArgs e) {
            if (Recorder.IsRecording) {
                e.Cancel = true;
                return;
            }

            if (NestopiaListener.IsConnected) {
                e.Cancel = true;
                
                await DisconnectNestopia(sender, e);

                Close();
            }
        }
    }
}