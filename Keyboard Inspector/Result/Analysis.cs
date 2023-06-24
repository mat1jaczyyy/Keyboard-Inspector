using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using FFTW.NET;

namespace Keyboard_Inspector {
    class Analysis: IBinary {
        const int pLow = 20;
        const int pHigh = 128000;

        int _precision = 4000;
        public int Precision {
            get => _precision;
            set => _precision = value.Clamp(0, pHigh);
        }
        public bool PrecisionValid => Precision >= pLow;

        int _hps = 0;
        public int HPS {
            get => _hps;
            set => _hps = value.Clamp(0, 9);
        }

        public bool LowCut = true;

        public void ToBinary(BinaryWriter bw) {
            bw.Write(Precision);
            bw.Write(HPS);
            bw.Write(LowCut);
        }

        public static Analysis FromBinary(BinaryReader br, uint fileVersion) {
            var ret = new Analysis();

            ret.Precision = br.ReadInt32();
            ret.HPS = br.ReadInt32();
            ret.LowCut = br.ReadBoolean();

            return ret;
        }

        public Result Result;

        List<double> diffs;
        void CalcDiffs() {
            diffs = new List<double>(Result.Events.Count);

            for (int i = 1; i < Result.Events.Count; i++)
                diffs.Add(Result.Events[i].Time - Result.Events[i - 1].Time);
        }

        List<double> compound;
        void CalcCompound() {
            compound = new List<double>(Math.Min(100_000_000, Result.Events.Count * Result.Events.Count / 2));

            for (int i = 0; i < Result.Events.Count - 1; i++) {
                for (int j = i + 1; j < Result.Events.Count; j++) {
                    double diff = Result.Events[j].Time - Result.Events[i].Time;

                    if (j > i + 1 && diff >= 1)
                        break;

                    compound.Add(diff);
                }
            }
        }

        List<double> circular;
        void CalcCircular() {
            circular = new List<double>(Result.Events.Count);

            for (int i = 0; i < Result.Events.Count; i++)
                circular.Add(Result.Events[i].Time % 1);
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

        public void CreateCache() {
            if (Result.IsEmpty(Result)) return;

            CalcDiffs();
            CalcCompound();
            CalcCircular();
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

        IEnumerable<double> DefaultTimeTransform(AlignedArrayDouble arr) {
            for (int i = 0; i < arr.Length; i++)
                yield return arr[i];
        }

        void ApplyLowCut(double[] data) {
            if (!Result.Analysis.LowCut) return;

            // https://www.desmos.com/calculator/yukhgjz5g9
            for (int i = 0; i < Math.Min(70, data.Length); i++)
                data[i] /= 1 + Math.Pow(Math.E, -(i - 25) / 4.0);
        }

        void HarmonicProduct(double[] data, double[] copy) {
            if (Result.Analysis.HPS <= 0) return;

            if (copy == null) copy = data.ToArray();

            for (int i = 0; i < data.Length; i++) {
                for (var j = 0; j < Result.Analysis.HPS; j++) {
                    data[i] *= copy[(int)((double)i * (j + 1) / (Result.Analysis.HPS + 1))];
                }
            }
        }

        void Normalize(double[] data) {
            double max = Math.Max(1, data.Max());

            for (int i = 0; i < data.Length; i++)
                data[i] /= max;
        }

        void DrawGraph(Chart chart, IEnumerable<double> data, double xFactor = 1, string estimate = "") {
            chart.LoadData(data, xFactor);

            var titles = chart.Tag as MainForm.ChartTitles;

            chart.Title = $"{titles.Primary} ({titles.Secondary}{estimate})";
        }

        Dictionary<Chart, double[]> beforeLowCut = new Dictionary<Chart, double[]>();
        Dictionary<Chart, double[]> beforeHPS = new Dictionary<Chart, double[]>();

        void RunGraphJob(List<double> source, Chart timeChart, Chart freqChart, Func<AlignedArrayDouble, IEnumerable<double>> timeTransform = null) {
            timeTransform = timeTransform ?? DefaultTimeTransform;

            AlignedArrayDouble data = new AlignedArrayDouble(64, Result.Analysis.Precision);
            for (int i = 0; i < source.Count; i++) {
                double v = source[i];
                if (v.InRangeIE(0, 1))
                    data[(int)Math.Round(v * Result.Analysis.Precision)] += 1;
            }

            DrawGraph(timeChart, timeTransform(data), 1000.0 / Result.Analysis.Precision);

            AlignedArrayComplex input = new AlignedArrayComplex(64, Result.Analysis.Precision / 2 + 1);
            DFT.FFT(data, input, PlannerFlags.Estimate, Environment.ProcessorCount);

            double[] freq = new double[input.Length];
            for (int i = 0; i < freq.Length; i++)
                freq[i] = Math.Sqrt(input[i].Real * input[i].Real + input[i].Imaginary * input[i].Imaginary);

            beforeLowCut[freqChart] = freq;
            RunFromLowCut(freqChart);
        }

        void RunFromLowCut(Chart chart) {
            double[] data = beforeLowCut[chart].ToArray();

            ApplyLowCut(data);

            beforeHPS[chart] = data;

            RunFromHPS(chart);
        }

        void RunFromHPS(Chart chart) {
            double[] data = beforeHPS[chart].ToArray();

            HarmonicProduct(data, beforeHPS[chart]);

            Normalize(data);

            double hpsFactor = 1.0 / (Result.Analysis.HPS + 1);

            string estimate = "";
            if (EstimatePeak(data, out int peak))
                estimate = $" - peak at {(int)Math.Round(peak * hpsFactor)} Hz";

            DrawGraph(chart, data, hpsFactor, estimate);
        }
        
        void RunChartsSuspendedAction(IEnumerable<Chart> charts, Action action) {
            try {
                foreach (var chart in charts)
                    chart.SuspendPaint();

                action();

            } finally {
                foreach (var chart in charts)
                    chart.ResumePaint();
            }
        }

        public void Analyze() {
            if (Result.IsEmpty(Result)) return;
            if (!Result.Analysis.PrecisionValid) return;

            RunChartsSuspendedAction(MainForm.Instance.Charts, () => {
                RunGraphJob(diffs, MainForm.Instance.tDiffs, MainForm.Instance.fDiffs);
                RunGraphJob(compound, MainForm.Instance.tCompound, MainForm.Instance.fCompound);
                RunGraphJob(circular, MainForm.Instance.tCircular, MainForm.Instance.fCircular, CircularRotationFix);
            });

            DFT.Wisdom.Export(Program.WisdomFile);
        }

        public void ReanalyzeFromLowCut() {
            RunChartsSuspendedAction(MainForm.Instance.fCharts, () => {
                RunFromLowCut(MainForm.Instance.fDiffs);
                RunFromLowCut(MainForm.Instance.fCompound);
                RunFromLowCut(MainForm.Instance.fCircular);
            });
        }

        public void ReanalyzeFromHPS() {
            RunChartsSuspendedAction(MainForm.Instance.fCharts, () => {
                RunFromHPS(MainForm.Instance.fDiffs);
                RunFromHPS(MainForm.Instance.fCompound);
                RunFromHPS(MainForm.Instance.fCircular);
            });
        }
    }
}
