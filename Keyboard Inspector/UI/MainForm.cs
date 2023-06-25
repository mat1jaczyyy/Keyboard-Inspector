using System;
using System.Collections.Generic;
using System.Data;
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
        }

        enum UIState {
            None, Recording, Parsing, Downloading
        }

        void UpdateState(UIState state) {
            mainmenu.Enabled = state == UIState.None;

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

        public Result Result { get; private set; } = null;

        void ResultLoaded() {
            UpdateState(Recorder.IsRecording? UIState.Recording : UIState.None);

            string title = Result.IsEmpty(Result)? "" : Result.GetTitle();
            Text = (string.IsNullOrWhiteSpace(title)? "" : $"{title} - ") + "Keyboard Inspector";

            save.Enabled = split.Visible = !Result.IsEmpty(Result);

            labelN.Text = Result.IsEmpty(Result)? "" : Result.Events.Count.ToString();

            screen.LoadData(Result);

            if (Result.IsEmpty(Result)) return;

            SetPrecisionSilently();
            SetHPSSilently();
            SetLowCutSilently();

            Result.Analysis.CreateCache();
            Result.Analysis.Analyze();
        }

        void rec_Click(object sender, EventArgs e) {
            if (ctsLoadURL != null) {
                ctsLoadURL.Cancel();

            } else if (!Recorder.IsRecording) {
                Result = null;
                Recorder.StartRecording();

            } else {
                Result = Recorder.StopRecording();
            }

            ResultLoaded();
        }

        bool silent;

        void SetPrecisionSilently() {
            silent = true;
            tbPrecision.Text = Result.Analysis.Precision.ToString();
            silent = false;
        }

        void SetHPSSilently() {
            silent = true;
            hps.Value = Result.Analysis.HPS;
            silent = false;
        }

        void SetLowCutSilently() {
            silent = true;
            lowCut.Checked = Result.Analysis.LowCut;
            silent = false;
        }

        void tbPrecision_TextChanged(object sender, EventArgs e) {
            if (silent) return;

            if (!int.TryParse(tbPrecision.Text, out int precision)) return;
            Result.Analysis.Precision = precision;

            SetPrecisionSilently();
            tbPrecision.Refresh();

            Result.Analysis.Analyze();
        }

        void precisionDouble_Click(object sender, EventArgs e) {
            Result.Analysis.Precision *= 2;
            tbPrecision.Text = Result.Analysis.Precision.ToString();
        }

        void precisionHalf_Click(object sender, EventArgs e) {
            Result.Analysis.Precision /= 2;
            tbPrecision.Text = Result.Analysis.Precision.ToString();
        }

        void lowCut_CheckedChanged(object sender, EventArgs e) {
            if (silent) return;

            Result.Analysis.LowCut = lowCut.Checked;
            Result.Analysis.ReanalyzeFromLowCut();
        }

        void hps_ValueChanged(object sender, EventArgs e) {
            if (silent) return;

            Result.Analysis.HPS = (int)hps.Value;
            Result.Analysis.ReanalyzeFromHPS();
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
            fCharts = Charts.Where(i => tlpCharts.GetRow(i) == 1).ToList();

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
                    Primary = PrimaryTitles[tlpCharts.GetColumn(chart)],
                    Secondary = SecondaryTitles[tlpCharts.GetRow(chart)],
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
            Result = null;
            ResultLoaded();
        }

        async Task LoadFile(string filename, FileFormat format = null) {
            UpdateState(UIState.Parsing);

            var load = await FileSystem.Open(filename, format);

            if (load.Error == null) {
                Result = load.Result;
                ResultLoaded();
            }

            UpdateState(UIState.None);
            status.Text = load.Error;
        }

        CancellationTokenSource ctsLoadURL = null;

        async Task LoadURL(Uri url, FileFormat format) {
            UpdateState(UIState.Downloading);
            ctsLoadURL = new CancellationTokenSource();

            var load = await FileSystem.Import(url, format, ctsLoadURL.Token);

            if (load.Error == null) {
                Result = load.Result;
                ResultLoaded();
            }

            UpdateState(UIState.None);
            status.Text = load.Error;
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
            if (Result.IsEmpty(Result)) return;

            status.Text = FileSystem.Save(Result);
        }

        async void import_Click(object sender, EventArgs e) {
            if (!mainmenu.Enabled) return;

            if (FileSystem.ImportDialog(out Uri url, out FileFormat format)) {
                CloseFile();
                await LoadURL(url, format);
            }
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

        private void captureDontClose(object sender, ToolStripDropDownClosingEventArgs e) {
            if (e.CloseReason == ToolStripDropDownCloseReason.ItemClicked)
                e.Cancel = true;
        }
    }
}