using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Windows.Forms;

using DarkUI.Controls;
using DarkUI.Forms;

using FFTW.NET;

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

            key.DropDown.Closing += (s, e) => e.Cancel = e.CloseReason == ToolStripDropDownCloseReason.ItemClicked;

            // TODO REFACTOR ALL BELOW THIS
            InitFileFormats();

            allCharts = tlpCharts.Controls.OfType<Chart>().ToList();
            freqCharts = allCharts.Where(i => tlpCharts.GetRow(i) == 1).ToList();

            InitCharts();
        }

        int elapsed;
        Result result = null;

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

        void resultLoaded() {
            string title = Result.IsEmpty(result)? "" : result.GetTitle();
            Text = (string.IsNullOrWhiteSpace(title)? "" : $"{title} - ") + "Keyboard Inspector";

            key.Enabled = recording.Enabled = open.Enabled = !Recorder.IsRecording;
            save.Enabled = split.Visible = !Result.IsEmpty(result);

            labelN.Text = Result.IsEmpty(result)? "" : result.Events.Count.ToString();

            screen.Area.LoadData(result);

            if (!PrecisionValid) {
                silentAnalysis = true;
                tbPrecision.Text = "4000";
                silentAnalysis = false;
            }

            if (!Result.IsEmpty(result)) {
                CalcDiffs();
                CalcCompound();
                CalcCircular();
            }

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

                elapsed = -1;
                t_Tick(sender, e);
                t.Enabled = true;

            } else {
                rec.Text = "Start Recording";
                status.Text = "";
                split.Enabled = true;

                result = Recorder.StopRecording();

                t.Enabled = false;
            }

            resultLoaded();
        }

        void t_Tick(object sender, EventArgs e) {
            status.TextAlign = ContentAlignment.TopRight;
            status.Text = $"Recording... {TimeSpan.FromSeconds(++elapsed):hh\\:mm\\:ss}";
        }

        bool EstimatePeak(double[] data, out int result) {
            double max = double.MaxValue;
            result = -1;

            int i = 0;
            for (; i < data.Length; i++) {
                double v = data[i];
                bool brk = v > max;
                max = v;
                if (brk) break;
            }
            for (; i < data.Length; i++) {
                double v = data[i];
                if (v >= max) {
                    max = v;
                    result = i;
                }
            }
            
            return result != -1;
        }

        List<double> cacheDiffs;
        void CalcDiffs() {
            cacheDiffs = new List<double>(result.Events.Count);

            for (int i = 1; i < result.Events.Count; i++)
                cacheDiffs.Add(result.Events[i].Time - result.Events[i - 1].Time);
        }

        List<double> cacheCompound;
        void CalcCompound() {
            cacheCompound = new List<double>(result.Events.Count * result.Events.Count / 2);

            for (int i = 0; i < result.Events.Count - 1; i++) {
                for (int j = i + 1; j < result.Events.Count; j++) {
                    double diff = result.Events[j].Time - result.Events[i].Time;

                    if (j > i + 1 && diff >= 1)
                        break;

                    cacheCompound.Add(diff);
                }
            }
        }
        
        List<double> cacheCircular;
        void CalcCircular() {
            cacheCircular = new List<double>(result.Events.Count);

            for (int i = 0; i < result.Events.Count; i++)
                cacheCircular.Add(result.Events[i].Time % 1);
        }

        IEnumerable<double> CircularRotationFix(AlignedArrayDouble arr) {
            double max = double.MinValue;
            int rot = -1;

            for (int i = 0; i < arr.Length; i++) {
                if (arr[i] > max) {
                    max = arr[i];
                    rot = i;
                }
                i++;
            }

            for (int i = 0; i < arr.Length; i++) {
                yield return arr[(i + rot) % arr.Length];
            }
        }
        
        IEnumerable<double> DefaultTimeTransform(AlignedArrayDouble arr) {
            for (int i = 0; i < arr.Length; i++)
                yield return arr[i];
        }

        void LowCut(double[] data) {
            if (!lowCut.Checked) return;

            // https://www.desmos.com/calculator/yukhgjz5g9
            for (int i = 0; i < Math.Min(70, data.Length); i++)
                data[i] /= 1 + Math.Pow(Math.E, -(i - 25) / 4.0);
        }

        void HarmonicProduct(double[] data, double[] copy) {
            if (hpsIterations <= 0) return;

            if (copy == null) copy = data.ToArray();

            for (int i = 0; i < data.Length; i++) {
                for (var j = 0; j < hpsIterations; j++) {
                    data[i] *= copy[(int)((double)i * (j + 1) / (hpsIterations + 1))];
                }
            }
        }

        void Normalize(double[] data) {
            double max = Math.Max(1, data.Max());

            for (int i = 0; i < data.Length; i++)
                data[i] /= max;
        }

        void DrawGraph(Chart chart, IEnumerable<double> data, double xFactor = 1, string estimate = "") {
            chart.Area.LoadData(data, xFactor);

            int row = tlpCharts.GetRow(chart);
            int col = tlpCharts.GetColumn(chart);

            chart.Area.Title = $"{BaseTitles[col]} ({SecondaryTitles[row]}{estimate})";
        }

        Dictionary<Chart, double[]> beforeLowCut = new Dictionary<Chart, double[]>();
        Dictionary<Chart, double[]> beforeHPS = new Dictionary<Chart, double[]>();

        void RunGraphJob(List<double> source, Chart timeChart, Chart freqChart, Func<AlignedArrayDouble, IEnumerable<double>> timeTransform = null) {
            timeTransform = timeTransform?? DefaultTimeTransform;
            
            AlignedArrayDouble data = new AlignedArrayDouble(64, precision);
            for (int i = 0; i < source.Count; i++) {
                double v = source[i];
                if (0 <= v && v < 1)
                    data[(int)Math.Round(v * precision)] += 1;
            }

            DrawGraph(timeChart, timeTransform(data), 1000.0 / precision);

            AlignedArrayComplex input = new AlignedArrayComplex(64, precision / 2 + 1);
            DFT.FFT(data, input, PlannerFlags.Estimate, Environment.ProcessorCount);

            double[] freq = new double[input.Length];
            for (int i = 0; i < freq.Length; i++)
                freq[i] = Math.Sqrt(input[i].Real * input[i].Real + input[i].Imaginary * input[i].Imaginary);

            beforeLowCut[freqChart] = freq;
            RunFromLowCut(freqChart);
        }

        void RunFromLowCut(Chart chart) {
            double[] data = beforeLowCut[chart].ToArray();

            LowCut(data);

            beforeHPS[chart] = data;

            RunFromHPS(chart);
        }

        void RunFromHPS(Chart chart) {
            double[] data = beforeHPS[chart].ToArray();

            HarmonicProduct(data, beforeHPS[chart]);

            Normalize(data);

            double hpsFactor = 1.0 / (hpsIterations + 1);

            string estimate = "";
            if (EstimatePeak(data, out int result))
                estimate = $" - peak at {(int)Math.Round(result * hpsFactor)} Hz";

            DrawGraph(chart, data, hpsFactor, estimate);
        }

        bool silentAnalysis = false;

        const int precisionLimit = 128000;
        void tbPrecision_TextChanged(object sender, EventArgs e) {
            if (silentAnalysis) return;

            int.TryParse(tbPrecision.Text, out precision);

            if (precision > precisionLimit) {
                silentAnalysis = true;
                tbPrecision.Text = (precision = precisionLimit).ToString();
                silentAnalysis = false;
            }

            CreateCharts();
        }

        private void lowCut_CheckedChanged(object sender, EventArgs e) {
            if (silentAnalysis) return;

            RunChartsSuspendedAction(freqCharts, () => {
                RunFromLowCut(chartDiffsFreq);
                RunFromLowCut(chartCompoundFreq);
                RunFromLowCut(chartCircularFreq);
            });
        }

        private void hps_ValueChanged(object sender, EventArgs e) {
            if (silentAnalysis) return;

            RunChartsSuspendedAction(freqCharts, () => {
                RunFromHPS(chartDiffsFreq);
                RunFromHPS(chartCompoundFreq);
                RunFromHPS(chartCircularFreq);
            });
        }

        int precision = 4000;
        bool PrecisionValid => precision >= 20;

        int hpsIterations => (int)hps.Value;

        List<Chart> allCharts, freqCharts;

        void RunChartsSuspendedAction(IEnumerable<Chart> charts, Action action) {
            try {
                foreach (var chart in charts)
                    chart.Area.SuspendPaint();

                action();

            } finally {
                foreach (var chart in charts)
                    chart.Area.ResumePaint();
            }
        }

        void CreateCharts() {
            if (Result.IsEmpty(result)) return;

            int.TryParse(tbPrecision.Text, out precision);

            if (!PrecisionValid) return;

            RunChartsSuspendedAction(allCharts, () => {
                RunGraphJob(cacheDiffs, chartDiffs, chartDiffsFreq);
                RunGraphJob(cacheCompound, chartCompound, chartCompoundFreq);
                RunGraphJob(cacheCircular, chartCircular, chartCircularFreq, CircularRotationFix);
            });

            DFT.Wisdom.Export(Program.WisdomFile);
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

        string[] BaseTitles = new string[] {
            "Differences between consecutive events",
            "Differences between all events",
            "Events wrapped around a second"
        };

        string[] SecondaryTitles = new string[] {
            "time domain",
            "frequency domain"
        };

        Action<Scope>[] ScopeDefaults = new Action<Scope>[] {
            i => i.SetBetween(0, 100, (i.Control as ChartArea).XMaxFactored),
            i => i.Reset()
        };

        void InitCharts() {
            var TimeInterval = (Func<int, double>)(i => ScreenInterval(i) * 1000);
            var FreqInterval = (Func<int, double>)(i => {
                if (i < 2) return (i + 1) * 5;
                if (i < 4) return (i - 1) * 25;
                return Math.Pow(2, i - 4) * 125;
            });

            foreach (var chart in allCharts) {
                int row = tlpCharts.GetRow(chart);
                int col = tlpCharts.GetColumn(chart);

                chart.Area.Scope.IntervalGenerator = row == 0? TimeInterval : FreqInterval;
                chart.Area.Scope.Default = ScopeDefaults[row];

                chart.Area.Spotlight += chart_Spotlight;
            }

            screen.Area.Scope.IntervalGenerator = ScreenInterval;
        }

        void chart_Spotlight(object sender, EventArgs e) {
            var chart = sender as Chart;

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
            if (!Result.IsEmpty(result) && sfd.ShowDialog() == DialogResult.OK)
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

        private void MainForm_Shown(object sender, EventArgs e) {
            if (Program.Args.Length > 0)
                LoadFile(Program.Args[0]);
        }

        void MainForm_FormClosing(object sender, FormClosingEventArgs e) {
            if (Recorder.IsRecording) {
                e.Cancel = true;
                return;
            }
        }
    }
}