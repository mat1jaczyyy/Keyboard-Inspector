using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using DarkUI.Controls;
using DarkUI.Forms;

namespace Keyboard_Inspector {
    public partial class TTRMPickerForm: DarkForm {
        static readonly Color leftColor = Color.FromArgb(25, 53, 95);
        static readonly Color leftWin = Color.FromArgb(38, 109, 205);
        static readonly Color rightColor = Color.FromArgb(95, 25, 25);
        static readonly Color rightWin = Color.FromArgb(205, 38, 38);

        public dynamic SelectedReplay { get; private set; }
        public int SelectedIndex { get; private set; }
        public string SelectedPlayer { get; private set; }
        public string OtherPlayer { get; private set; }

        struct Stats {
            public double pps, apm, vs;
            public int inputs;

            public override string ToString()
                => $"{pps:0.00} PPS · {apm:0.00} APM · {vs:0.00} VS";

            public string Inputs()
                => $"{inputs} inputs";
        }

        static Stats Avg(List<Stats> statlist)
            => new Stats() {
                pps = statlist.Average(s => s.pps),
                apm = statlist.Average(s => s.apm),
                vs = statlist.Average(s => s.vs),
                inputs = statlist.Sum(s => s.inputs)
            };

        static Stats RoundStats(dynamic events) {
            var stats = new Stats();
            stats.inputs = 0;

            foreach (var i in events) {
                if (i.type.StartsWith("key") && !(i.data.hoisted() && i.data.hoisted == true))
                    stats.inputs++;

                if (i.type == "end") {
                    stats.pps = i.data.export.aggregatestats.pps;
                    stats.apm = i.data.export.aggregatestats.apm;
                    stats.vs = i.data.export.aggregatestats.vsscore;
                }
            }

            return stats;
        }

        static dynamic Last(dynamic arr) {
            dynamic ret = null;
            foreach (var i in arr)
                ret = i;
            return ret;
        }

        public TTRMPickerForm(dynamic ttr) {
            InitializeComponent();

            panel1.BackColor = leftColor;
            panel2.BackColor = rightColor;

            string p1name = TetrioReplay.GetUsername(ttr.endcontext[0]);
            string p2name = TetrioReplay.GetUsername(ttr.endcontext[1]);

            p1lbl.Text = p1name;
            p2lbl.Text = p2name;

            p1score.Text = TetrioReplay.GetScore(ttr.endcontext[0]);
            p2score.Text = TetrioReplay.GetScore(ttr.endcontext[1]);

            string p1id = TetrioReplay.GetUserID(ttr.endcontext[0]);
            string p2id = TetrioReplay.GetUserID(ttr.endcontext[1]);

            List<Stats> p1replaystats = new List<Stats>(ttr.data.Count);
            List<Stats> p2replaystats = new List<Stats>(ttr.data.Count);

            int baseY = panel1.Bounds.Bottom + 10;
            int stretch = 25;
            int inputsize = 90;
            int height = 19;
            int spacing = 6;

            for (int i = 0; i < ttr.data.Count; i++) {
                var left = new DarkLabel();
                var leftInputs = new DarkLabel();
                var right = new DarkLabel();
                var rightInputs = new DarkLabel();
                var time = new DarkLabel();

                left.Anchor = leftInputs.Anchor = right.Anchor = rightInputs.Anchor = time.Anchor = AnchorStyles.Top;
                left.AutoSize = leftInputs.AutoSize = right.AutoSize = rightInputs.AutoSize = time.AutoSize = false;

                left.TextAlign = leftInputs.TextAlign = ContentAlignment.MiddleRight;
                right.TextAlign = rightInputs.TextAlign = ContentAlignment.MiddleLeft;
                time.TextAlign = ContentAlignment.MiddleCenter;

                left.Cursor = right.Cursor = leftInputs.Cursor = rightInputs.Cursor = Cursors.Hand;
                time.ForeColor = Color.Gray;

                left.Location = new Point(panel1.Location.X, baseY + i * (height + spacing));
                left.Size = new Size(panel1.Width + stretch - inputsize, height);

                leftInputs.Location = new Point(left.Location.X + left.Width, left.Location.Y);
                leftInputs.Size = new Size(inputsize, height);

                right.Location = new Point(panel2.Location.X - stretch + inputsize, baseY + i * (height + spacing));
                right.Size = new Size(panel2.Width + stretch - inputsize, height);

                rightInputs.Location = new Point(right.Location.X - inputsize, right.Location.Y);
                rightInputs.Size = new Size(inputsize, height);

                time.Location = new Point(vs.Location.X, baseY + i * (height + spacing));
                time.Size = new Size(vs.Width, height);

                var round = ttr.data[i];

                int bp1 = TetrioReplay.GetUserID(round.board[0]) == p1id? 0 : 1;
                int bp2 = TetrioReplay.GetUserID(round.board[1]) == p2id? 1 : 0;

                var opts1 = Last(round.replays[0].events).data.export.options;
                var opts2 = Last(round.replays[1].events).data.export.options;

                int rp1 = Last(round.replays[0].events).data.export.options.username.ToLower() == p1name.ToLower()? 0 : 1;
                int rp2 = Last(round.replays[1].events).data.export.options.username.ToLower() == p2name.ToLower()? 1 : 0;

                Stats p1s = RoundStats(round.replays[rp1].events);
                Stats p2s = RoundStats(round.replays[rp2].events);

                left.Text = p1s.ToString();
                right.Text = p2s.ToString();

                leftInputs.Font = rightInputs.Font = new Font(leftInputs.Font, FontStyle.Bold);

                leftInputs.Text = p1s.Inputs();
                rightInputs.Text = p2s.Inputs();

                p1replaystats.Add(p1s);
                p2replaystats.Add(p2s);

                left.BackColor = leftInputs.BackColor = round.board[bp1].success? leftWin : panel1.BackColor;
                right.BackColor = rightInputs.BackColor = round.board[bp2].success? rightWin : panel2.BackColor;

                var seconds = (int)(Math.Min(round.replays[rp1].frames, round.replays[rp2].frames) / 60.0);
                time.Text = $"{seconds / 60:0}:{seconds % 60:00}";

                Controls.Add(left);
                Controls.Add(leftInputs);
                Controls.Add(right);
                Controls.Add(rightInputs);
                Controls.Add(time);

                int k = i;

                void leftClick(object sender, EventArgs e) {
                    SelectedReplay = round.replays[rp1];
                    SelectedIndex = k;
                    SelectedPlayer = p1id;
                    OtherPlayer = p2id;

                    DialogResult = DialogResult.OK;
                }

                left.Click += leftClick;
                leftInputs.Click += leftClick;

                void rightClick(object sender, EventArgs e) {
                    SelectedReplay = round.replays[rp2];
                    SelectedIndex = k;
                    SelectedPlayer = p2id;
                    OtherPlayer = p1id;

                    DialogResult = DialogResult.OK;
                };

                right.Click += rightClick;
                rightInputs.Click += rightClick;
            }

            p1stats.Text = Avg(p1replaystats).ToString();
            p2stats.Text = Avg(p2replaystats).ToString();

            Height += ttr.data.Count * (height + spacing);
        }
    }
}
