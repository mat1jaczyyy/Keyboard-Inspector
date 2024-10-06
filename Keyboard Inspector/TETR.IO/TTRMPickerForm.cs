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

        static Stats RoundStats(dynamic round) {
            var stats = new Stats();
            stats.pps = round.stats.pps;
            stats.apm = round.stats.apm;
            stats.vs = round.stats.vsscore;

            stats.inputs = 0;
            if (round.replay()) {
                foreach (var i in round.replay.events) {
                    if (i.type.StartsWith("key") && !(i.data.hoisted() && i.data.hoisted == true))
                        stats.inputs++;
                }
            }

            return stats;
        }

        public TTRMPickerForm(dynamic ttr) {
            InitializeComponent();

            panel1.BackColor = leftColor;
            panel2.BackColor = rightColor;

            string p1name = ttr.replay.leaderboard[0].username.ToUpper();
            string p2name = ttr.replay.leaderboard[1].username.ToUpper();

            p1lbl.Text = p1name;
            p2lbl.Text = p2name;

            p1score.Text = ttr.replay.leaderboard[0].wins.ToString();
            p2score.Text = ttr.replay.leaderboard[1].wins.ToString();

            string p1id = ttr.replay.leaderboard[0].id;
            string p2id = ttr.replay.leaderboard[1].id;

            int N = ttr.replay.rounds.Count;

            int baseY = panel1.Bounds.Bottom + 10;
            int stretch = 25;
            int inputsize = 90;
            int height = 19;
            int spacing = 6;

            for (int i = 0; i < N; i++) {
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

                var round = ttr.replay.rounds[i];

                int bp1 = round[0].id == p1id? 0 : 1;
                int bp2 = round[1].id == p2id? 1 : 0;

                Stats p1s = RoundStats(round[bp1]);
                Stats p2s = RoundStats(round[bp2]);

                left.Text = p1s.ToString();
                right.Text = p2s.ToString();

                leftInputs.Font = rightInputs.Font = new Font(leftInputs.Font, FontStyle.Bold);

                leftInputs.Text = p1s.Inputs();
                rightInputs.Text = p2s.Inputs();

                left.BackColor = leftInputs.BackColor = round[bp1].alive? leftWin : panel1.BackColor;
                right.BackColor = rightInputs.BackColor = round[bp2].alive? rightWin : panel2.BackColor;

                var seconds = (int)(Math.Min(round[bp1].replay.frames, round[bp2].replay.frames) / 60.0);
                time.Text = $"{seconds / 60:0}:{seconds % 60:00}";

                Controls.Add(left);
                Controls.Add(leftInputs);
                Controls.Add(right);
                Controls.Add(rightInputs);
                Controls.Add(time);

                int k = i;

                void leftClick(object sender, EventArgs e) {
                    SelectedReplay = round[bp1].replay;
                    SelectedIndex = k;
                    SelectedPlayer = p1id;
                    OtherPlayer = p2id;

                    DialogResult = DialogResult.OK;
                }

                left.Click += leftClick;
                leftInputs.Click += leftClick;

                void rightClick(object sender, EventArgs e) {
                    SelectedReplay = round[bp2].replay;
                    SelectedIndex = k;
                    SelectedPlayer = p2id;
                    OtherPlayer = p1id;

                    DialogResult = DialogResult.OK;
                };

                right.Click += rightClick;
                rightInputs.Click += rightClick;
            }

            p1stats.Text = RoundStats(ttr.replay.leaderboard[0]).ToString();
            p2stats.Text = RoundStats(ttr.replay.leaderboard[1]).ToString();

            Height += N * (height + spacing);
        }
    }
}
