using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Windows.Forms;

using MathNet.Numerics.Statistics;

namespace Keyboard_Inspector {
    public partial class MainForm : Form {
        public MainForm() {
            InitializeComponent();

            screen.AllowDrop = true;

            key.DropDown.Closing += (s, e) => {
                if (e.CloseReason == ToolStripDropDownCloseReason.ItemClicked)
                    e.Cancel = true;
            };
        }

        int elapsed;
        Result result = null;
        double zoom, viewport;
        float areaWidth, areaX;
        List<Keys> keys;
        List<RectangleF> textRects = new List<RectangleF>();
        Dictionary<Keys, Color> colors = new Dictionary<Keys, Color>();

        void processResult() {
            if (!freeze.Checked) {
                keys = result.Events.Select(i => i.Key).Distinct().ToList();
                colors.Clear();
            }
        }

        void resultLoaded() {
            key.Enabled = recording.Enabled = open.Enabled = !KeyRecorder.IsRecording;
            poll.Enabled = result?.Events.Count() >= 60;
            save.Enabled = result?.Events.Any() == true;

            Redraw();
            UpdateScroll();
        }

        void rec_Click(object sender, EventArgs e) {
            status.TextAlign = ContentAlignment.TopRight;

            if (!KeyRecorder.IsRecording) {
                rec.Text = "Stop Recording";
                status.Text = "Recording... 00:00:00";

                result = null;

                KeyRecorder.StartRecording();

                zoom = 1;
                viewport = 0;

                elapsed = -1;
                t_Tick(sender, e);
                t.Enabled = true;

            } else {
                rec.Text = "Start Recording";
                status.Text = "";

                result = KeyRecorder.StopRecording();

                processResult();

                t.Enabled = false;
            }

            resultLoaded();
        }

        void t_Tick(object sender, EventArgs e)
            => status.Text = $"Recording... {TimeSpan.FromSeconds(++elapsed):hh\\:mm\\:ss}";

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

            if (result != null && keys.Any()) {
                textRects.Clear();

                Font font = status.Font;
                Brush textBrush = new SolidBrush(status.ForeColor);
                Brush keyBrush = new SolidBrush(Color.Gray);
                Pen pen = new Pen(Color.LightGray);

                using (Graphics gfx = Graphics.FromImage(img)) {
                    gfx.FillRectangle(new SolidBrush(screen.BackColor), 0, 0, screen.Width, screen.Height);

                    SizeF[] textSize = keys.Select(i => gfx.MeasureString(i.ToString(), status.Font)).ToArray();
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

                    float keyHeight = (float)(screen.Height - 3 * Margin - textHeight) / keys.Count;

                    for (k = 0; k < keys.Count; k++) {
                        textRects.Add(new RectangleF(new PointF(
                            Margin + textWidth - textSize[k].Width,
                            Margin + keyHeight * k + (keyHeight - textHeight) / 2
                        ), textSize[k]));

                        gfx.DrawString(
                            keys[k].ToString(), font, textBrush,
                            textRects[k].X, textRects[k].Y
                        );

                        gfx.DrawLine(
                            pen,
                            2 * Margin + textWidth,
                            Margin + keyHeight * (k + 0.5f),
                            screen.Width - Margin,
                            Margin + keyHeight * (k + 0.5f)
                        );

                        KeyEvent[] events = result.Events.Where(i => i.Key == keys[k]).ToArray();
                        Brush brush = colors.ContainsKey(keys[k])? new SolidBrush(colors[keys[k]]) : keyBrush;

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
                            if (events[0].Pressed == false) {
                                e = 1;
                                drawBar(0, events[0].Timestamp);
                            }

                            while (e < events.Length) {
                                int f;
                                for (f = e + 1; f < events.Length && events[f].Pressed == true; f++);

                                drawBar(
                                    events[e].Timestamp,
                                    f < events.Length ? events[f].Timestamp : result.Time
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
            cd.Color = colors.ContainsKey(keys[i]) ? colors[keys[i]] : Color.Gray;

            if (cd.ShowDialog() == DialogResult.OK) {
                colors[keys[i]] = cd.Color;
                Redraw();
            }
        }

        void hide_Click(object sender, EventArgs e) {
            if (!(keymenu.Tag is int i)) return;

            keys.RemoveAt(i);
            Redraw();
        }

        void unhide_Click(object sender, EventArgs e) {
            if (sender is ToolStripMenuItem item && item.GetCurrentParent() is ToolStripDropDownMenu menu)
                menu.Close();

            IEnumerable<Keys> unhidden = result.Events.Select(i => i.Key);

            if (freeze.Checked)
                unhidden = keys.Concat(unhidden);

            keys = unhidden.Distinct().ToList();

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
                Keys k = keys[d];
                keys.RemoveAt(d);
                keys.Insert(i, k);

                Redraw();
            }
        }

        static readonly double[] PollRates = new double[] {62.5, 125, 250, 500, 1000, 2000, 4000};

        void analyze_Click(object sender, EventArgs e) {
            if (result == null) return;

            StringBuilder file = new StringBuilder();

            // Filter Windows auto-repeat
            List<KeyEvent> no_repeat = result.Events
                .Where((x, i) => x.Pressed != true || result.Events.Take(i).Where(j => j.Key == x.Key).LastOrDefault().Pressed != true)
                .ToList();

            file.Append($"no_repeat:\n{string.Join("\n", no_repeat)}\n\n");

            // Get differences between inputs
            List<double> delta = no_repeat
                .Skip(1)
                .Select((x, i) => x.Timestamp - no_repeat[i].Timestamp)
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
                    using (FileStream read = new FileStream(ofd.FileName, FileMode.Open))
                        if (new DataContractSerializer(typeof(Result)).ReadObject(read) is Result loaded) {
                            result = loaded;

                            processResult();
                            resultLoaded();
                        }

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
                using (FileStream write = new FileStream(sfd.FileName, FileMode.Create))
                    new DataContractSerializer(typeof(Result)).WriteObject(write, result);
        }
    }
}