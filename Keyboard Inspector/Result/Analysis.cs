using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using FFTW.NET;

namespace Keyboard_Inspector {
    class Analysis: IBinary {
        const int pLow = 20;
        const int pHigh = 128000;

        int _binrate = 4000;
        public int BinRate {
            get => _binrate;
            set => _binrate = value.Clamp(0, pHigh);
        }
        public bool BinRateValid => BinRate >= pLow;

        int _hps = 0;
        public int HPS {
            get => _hps;
            set => _hps = value.Clamp(0, 9);
        }

        public bool LowCut = true;

        public void ToBinary(BinaryWriter bw) {
            bw.Write(BinRate);
            bw.Write(HPS);
            bw.Write(LowCut);
        }

        public static Analysis FromBinary(BinaryReader br, uint fileVersion) {
            var ret = new Analysis();

            ret.BinRate = br.ReadInt32();
            ret.HPS = br.ReadInt32();
            ret.LowCut = br.ReadBoolean();

            return ret;
        }

        public Result Result;

        List<double> diffs;
        void CalcDiffs(List<Event> e) {
            diffs = new List<double>(e.Count);

            for (int i = 1; i < e.Count; i++)
                diffs.Add(e[i].Time - e[i - 1].Time);
        }

        List<double> compound;
        void CalcCompound(List<Event> e) {
            compound = new List<double>();

            for (int i = 0; i < e.Count - 1; i++) {
                for (int j = i + 1; j < e.Count; j++) {
                    double diff = e[j].Time - e[i].Time;

                    if (j > i + 1 && diff >= 1)
                        break;

                    compound.Add(diff);
                }
            }
        }

        List<double> circular;
        void CalcCircular(List<Event> e) {
            circular = new List<double>(e.Count);

            for (int i = 0; i < e.Count; i++)
                circular.Add(e[i].Time % 1);
        }

        AlignedArrayDouble Bin(List<double> source, bool modular = false) {
            int len = Result.Analysis.BinRate;

            AlignedArrayDouble ret = new AlignedArrayDouble(64, len);

            for (int i = 0; i < source.Count; i++) {
                double v = source[i];
                int bin = (int)Math.Round(v * len);

                if (bin >= len) {
                    if (modular) bin %= len;
                    else continue;
                }

                ret[bin] += 1;
            }

            return ret;
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

        void ApplyLowCut(double[] data) {
            if (!Result.Analysis.LowCut) return;

            double max = double.MaxValue;

            int i = 0;
            for (; i < data.Length; i++) {
                double v = data[i];
                bool brk = v > max;
                max = v;
                if (brk) break;
            }

            for (int j = 0; j < i; j++)
                data[j] = 0;
             
            int smooth = Math.Max(15, i / 2);

            for (int j = i; j < Math.Min(i + smooth, data.Length); j++) {
                double x = (j - i) / (double)smooth;
                data[j] *= (1 - Math.Cos(Math.PI * x)) / 2;
            }
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

        void DrawGraph(Chart chart, IEnumerable<double> data, double xFactor = 1) {
            chart.LoadData(data, xFactor);

            var titles = chart.Tag as MainForm.ChartTitles;

            chart.Title = $"{titles.Primary} ({titles.Secondary})";
        }

        Dictionary<Chart, double[]> beforeLowCut = new Dictionary<Chart, double[]>();
        Dictionary<Chart, double[]> beforeHPS = new Dictionary<Chart, double[]>();

        void RunGraphJob(AlignedArrayDouble data, Chart timeChart, Chart freqChart, Func<AlignedArrayDouble, IEnumerable<double>> timeTransform = null) {
            timeTransform = timeTransform?? DefaultTimeTransform;

            DrawGraph(timeChart, timeTransform(data), 1000.0 / Result.Analysis.BinRate);

            AlignedArrayComplex input = new AlignedArrayComplex(64, Result.Analysis.BinRate / 2 + 1);
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

            DrawGraph(chart, data, hpsFactor);
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

            List<Event> events = Result.AllVisibleEvents();

            MainForm.Instance.SetEventCount(events.Count);

            CalcDiffs(events);
            CalcCompound(events);
            CalcCircular(events);

            ReanalyzeFromCache();
        }

        public void ReanalyzeFromCache() {
            if (Result.IsEmpty(Result)) return;
            if (!Result.Analysis.BinRateValid) return;

            RunChartsSuspendedAction(MainForm.Instance.Charts, () => {
                RunGraphJob(Bin(diffs),          MainForm.Instance.tDiffs,    MainForm.Instance.fDiffs);
                RunGraphJob(Bin(compound),       MainForm.Instance.tCompound, MainForm.Instance.fCompound);
                RunGraphJob(Bin(circular, true), MainForm.Instance.tCircular, MainForm.Instance.fCircular, CircularRotationFix);
            });

            DFT.Wisdom.Export(Constants.WisdomFile);
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
