using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using DarkUI.Forms;

namespace Keyboard_Inspector {
    partial class MainForm: DarkForm {
        public static MainForm Instance { get; private set; }

        public MainForm() {
            if (Instance != null) throw new Exception("Can't have more than one MainForm");
            Instance = this;

            InitializeComponent();
            InitializeCharts();

            InitializeDropFocusClick(this);

            ResultLoaded();
        }

        void InitializeDropFocusClick(Control ctrl) {
            foreach (Control child in ctrl.Controls) {
                child.MouseDown += DropFocus;
                InitializeDropFocusClick(child);
            }
        }

        void DropFocus(object sender, MouseEventArgs e) {
            if (sender is Control ctrl && ctrl.Focused) return;
            
            ActiveControl = null;
        }


        enum UIState {
            None, Recording, Parsing, Downloading
        }

        void UpdateState(UIState state) {
            mainmenu.Enabled = frozen.Enabled = state == UIState.None;

            if (state == UIState.Recording) {
                rec.Text = "Stop Recording";
                rec.Enabled = true;
                split.Enabled = false;
                status.Text = "Recording...";

            } else if (state == UIState.Parsing) {
                rec.Text = "Start Recording";
                rec.Enabled = false;
                split.Enabled = false;
                status.Text = "Parsing file...";

            } else if (state == UIState.Downloading) {
                rec.Text = "Cancel";
                rec.Enabled = true;
                split.Enabled = false;
                status.Text = "Downloading file...";

            } else {
                rec.Text = "Start Recording";
                rec.Enabled = true;
                split.Enabled = true;
                status.Text = null;
            }

            status.Refresh();
            rec.Refresh();
        }

        void ResultLoaded() {
            UpdateState(Recorder.IsRecording? UIState.Recording : UIState.None);

            string title = Result.IsEmpty(Program.Result)? "" : Program.Result.GetTitle();
            Text = (string.IsNullOrWhiteSpace(title)? "" : $"{title} – ") + $"{Constants.Name} {Constants.Version}";

            SetEventCount(null);

            save.Enabled = split.Visible = !Result.IsEmpty(Program.Result);

            screen.LoadData(Program.Result);

            if (Result.IsEmpty(Program.Result)) return;

            SetPrecisionSilently();
            SetHPSSilently();
            SetLowCutSilently();

            Program.Result.Analysis.Analyze();
        }

        void rec_Click(object sender, EventArgs e) {
            if (ctsLoadURL != null) {
                ctsLoadURL.Cancel();

            } else if (!Recorder.IsRecording) {
                Program.Result = null;
                Recorder.StartRecording();

            } else {
                Program.Result = Recorder.StopRecording();
            }

            ResultLoaded();
        }

        public List<Chart> Charts { get; private set; }
        public List<Chart> tCharts { get; private set; }
        public List<Chart> fCharts { get; private set; }

        public class ChartTitles {
            public string Primary;
            public string Secondary;
        }

        string[] PrimaryTitles = new string[] {
            "Differences between consecutive events",
            "Differences between all events",
            "Events wrapped around a second"
        };

        string[] SecondaryTitles = new string[] {
            "time domain",
            "frequency domain"
        };

        static double ScreenInterval(int i) {
            if (i <= 3) return Math.Pow(2, i) / 1000;
            if (i <= 12) {
                double ret = Math.Pow(10, (i / 3) - 3);
                if (i % 3 == 1) ret *= 2;
                if (i % 3 == 2) ret *= 5;
                return ret;
            }
            if (i <= 15) return Math.Pow(2, i - 14) * 60;
            if (i <= 17) return Math.Pow(2, i - 16) * 300;
            return Math.Pow(2, i - 18) * 1800;
        }

        void InitializeCharts() {
            Charts = tlpCharts.Controls.OfType<Chart>().ToList();
            tCharts = Charts.Where(i => tlpCharts.GetRow(i) == 0).ToList();
            fCharts = Charts.Where(i => tlpCharts.GetRow(i) == 2).ToList();

            foreach (var chart in Charts) {
                chart.Spotlight += (_, __) => {
                    if (chart.Parent == tlpCharts) {
                        chart.SuspendLayout();

                        tlpCharts.Controls.Remove(chart);
                        split.Panel1.Controls.Add(chart);
                        split.Panel1.Controls.SetChildIndex(chart, 0);

                        chart.ResumeLayout();

                    } else if (chart.Parent == split.Panel1) {
                        chart.SuspendLayout();

                        split.Panel1.Controls.Remove(chart);
                        tlpCharts.Controls.Add(chart);

                        chart.ResumeLayout();
                    }
                };

                chart.Tag = new ChartTitles() { 
                    Primary = PrimaryTitles[tlpCharts.GetColumn(chart) / 2],
                    Secondary = SecondaryTitles[tlpCharts.GetRow(chart) / 2],
                };
            }

            foreach (var chart in tCharts) {
                chart.ScopeDefaultX = 100;
                chart.IntervalGenerator = i => ScreenInterval(i) * 1000;
            }

            foreach (var chart in fCharts) {
                chart.IntervalGenerator = i => {
                    if (i < 2) return (i + 1) * 5;
                    if (i < 4) return (i - 1) * 25;
                    return Math.Pow(2, i - 4) * 125;
                };
            }

            screen.IntervalGenerator = ScreenInterval;
        }

        void CloseFile() {
            Program.Result = null;
            ResultLoaded();
        }

        void LoadFinished(FileSystem.FileResult load) {
            if (load.Error == null) {
                SetFreezeSilently(false);
                Program.SetFreeze(false);

                Program.Result = load.Result;
                ResultLoaded();
            }

            UpdateState(UIState.None);
            status.Text = load.Error;
        }

        async Task LoadFile(string filename, FileFormat format = null) {
            UpdateState(UIState.Parsing);

            LoadFinished(await FileSystem.Open(filename, format));
        }

        CancellationTokenSource ctsLoadURL = null;

        async Task LoadURL(Uri url, FileFormat format) {
            UpdateState(UIState.Downloading);

            ctsLoadURL = new CancellationTokenSource();
            LoadFinished(await FileSystem.Import(url, format, ctsLoadURL.Token));
            ctsLoadURL = null;
        }

        public void DownloadFinished() {
            if (ctsLoadURL == null) return;

            ctsLoadURL = null;
            UpdateState(UIState.Parsing);
        }

        async void open_Click(object sender, EventArgs e) {
            if (!mainmenu.Enabled) return;

            if (FileSystem.OpenDialog(out string filename, out FileFormat format)) {
                CloseFile();
                await LoadFile(filename, format);
            }
        }

        void save_Click(object sender, EventArgs e) {
            if (!mainmenu.Enabled) return;
            if (Result.IsEmpty(Program.Result)) return;

            status.Text = FileSystem.Save(Program.Result);
        }

        async void import_Click(object sender, EventArgs e) {
            if (!mainmenu.Enabled) return;

            if (FileSystem.ImportDialog(out Uri url, out FileFormat format)) {
                CloseFile();
                await LoadURL(url, format);
            }
        }

        private void discord_Click(object sender, EventArgs e) {
            Process.Start(Constants.DiscordURL);
        }

        private void github_Click(object sender, EventArgs e) {
            Process.Start(Constants.GitHubURL);
        }

        bool ValidateFileDrag(DragEventArgs e, out string result) {
            result = null;
            if (!e.Data.GetDataPresent(DataFormats.FileDrop)) return false;

            var arr = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (arr.Length != 1) return false;

            result = arr[0];
            return FileSystem.SupportsFormat(arr[0]);
        }

        void MainForm_DragOver(object sender, DragEventArgs e) {
            e.Effect = DragDropEffects.None;

            if (!ValidateFileDrag(e, out _)) return;

            e.Effect = DragDropEffects.Move;
        }

        async void MainForm_DragDrop(object sender, DragEventArgs e) {
            if (!ValidateFileDrag(e, out string filename)) return;

            await LoadFile(filename);
        }

        async void MainForm_Shown(object sender, EventArgs e) {
            if (Program.Args.Length > 0)
                await LoadFile(Program.Args[0]);
        }

        void MainForm_FormClosing(object sender, FormClosingEventArgs e) {
            if (Recorder.IsRecording) {
                e.Cancel = true;
                return;
            }
        }

        void split_Paint(object sender, PaintEventArgs e) {
            float y = split.SplitterDistance + split.SplitterWidth / 2;
            e.Graphics.DrawLine(new Pen(Color.FromArgb(44, 44, 44)), 10, y, split.Width - 10, y);
        }

        void split_Layout(object sender, LayoutEventArgs e) {
            split.Invalidate(new Rectangle(0, split.SplitterDistance, split.Width, split.SplitterWidth));
        }

        public void SetEventCount(int? n)
            => labelN.Text = n == null? null : $"{n} input events";

        bool silent;

        void SetFreezeSilently(bool value) {
            silent = true;
            frozen.Checked = value;
            silent = false;
        }

        void frozen_CheckedChanged(object sender, EventArgs e) {
            if (silent) return;

            SetFreezeSilently(frozen.Checked);
            Program.SetFreeze(frozen.Checked);
        }

        void SetPrecisionSilently() {
            silent = true;
            tbPrecision.Text = Program.Result.Analysis.Precision.ToString();
            silent = false;
        }

        void SetHPSSilently() {
            silent = true;
            hps.Value = Program.Result.Analysis.HPS;
            silent = false;
        }

        void SetLowCutSilently() {
            silent = true;
            lowCut.Checked = Program.Result.Analysis.LowCut;
            silent = false;
        }

        void tbPrecision_TextChanged(object sender, EventArgs e) {
            if (silent) return;

            if (!int.TryParse(tbPrecision.Text, out int precision)) return;
            Program.Result.Analysis.Precision = precision;

            SetPrecisionSilently();
            tbPrecision.Refresh();

            Program.Result.Analysis.ReanalyzeFromCache();
        }

        void precisionDouble_Click(object sender, EventArgs e) {
            Program.Result.Analysis.Precision *= 2;
            tbPrecision.Text = Program.Result.Analysis.Precision.ToString();
        }

        void precisionHalf_Click(object sender, EventArgs e) {
            Program.Result.Analysis.Precision /= 2;
            tbPrecision.Text = Program.Result.Analysis.Precision.ToString();
        }

        void lowCut_CheckedChanged(object sender, EventArgs e) {
            if (silent) return;

            Program.Result.Analysis.LowCut = lowCut.Checked;
            Program.Result.Analysis.ReanalyzeFromLowCut();
        }

        void hps_ValueChanged(object sender, EventArgs e) {
            if (silent) return;

            Program.Result.Analysis.HPS = (int)hps.Value;
            Program.Result.Analysis.ReanalyzeFromHPS();
        }
    }
}