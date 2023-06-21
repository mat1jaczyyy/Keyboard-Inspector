using System;
using System.Drawing;
using System.Windows.Forms;

using DarkUI.Controls;
using DarkUI.Forms;

namespace Keyboard_Inspector {
    public partial class TTRMPickerForm: DarkForm {
        public dynamic SelectedReplay { get; private set; }
        public int SelectedIndex { get; private set; }
        public string SelectedPlayer { get; private set; }
        public string OtherPlayer { get; private set; }

        static int EventCount(dynamic events) {
            int cnt = 0;
            foreach (var i in events) {
                if (!i.type.StartsWith("key")) continue;
                if (i.data.hoisted() && i.data.hoisted == true) continue;
                cnt++;
            }
            return cnt;
        }

        public TTRMPickerForm(dynamic ttr) {
            InitializeComponent();

            p1lbl.Text = ttr.endcontext[0].user.username.ToUpper();
            p2lbl.Text = ttr.endcontext[1].user.username.ToUpper();

            string p1id = ttr.endcontext[0].user._id;
            string p2id = ttr.endcontext[1].user._id;

            for (int i = 0; i < ttr.data.Count; i++) {
                var left = new DarkLabel();
                var right = new DarkLabel();
                var time = new DarkLabel();

                left.Anchor = right.Anchor = time.Anchor = AnchorStyles.Top;
                left.AutoSize = right.AutoSize = time.AutoSize = false;

                left.TextAlign = ContentAlignment.MiddleRight;
                right.TextAlign = ContentAlignment.MiddleLeft;
                time.TextAlign = ContentAlignment.MiddleCenter;

                left.Cursor = right.Cursor = Cursors.Hand;
                time.ForeColor = Color.Gray;

                left.Location = new Point(p1lbl.Location.X, 41 + i * 20);
                left.Size = new Size(p1lbl.Width, 13);

                right.Location = new Point(p2lbl.Location.X, 41 + i * 20);
                right.Size = new Size(p2lbl.Width, 13);

                time.Location = new Point(vs.Location.X, 41 + i * 20);
                time.Size = new Size(vs.Width, 13);

                var round = ttr.data[i];

                int p1 = round.board[0].user._id == p1id? 0 : 1;
                int p2 = round.board[1].user._id == p2id? 1 : 0;

                left.Text = EventCount(round.replays[p1].events).ToString();
                right.Text = EventCount(round.replays[p2].events).ToString();

                var seconds = (int)(Math.Min(round.replays[p1].frames, round.replays[p2].frames) / 60.0);

                time.Text = $"{seconds / 60:0}:{seconds % 60:00}";

                if (round.board[p1].success)
                    left.BackColor = p1lbl.BackColor;

                if (round.board[p2].success)
                    right.BackColor = p2lbl.BackColor;

                Controls.Add(left);
                Controls.Add(right);
                Controls.Add(time);

                int k = i;

                left.Click += (s, e) => {
                    SelectedReplay = round.replays[p1];
                    SelectedIndex = k;
                    SelectedPlayer = p1id;
                    OtherPlayer = p2id;

                    DialogResult = DialogResult.OK;
                };

                right.Click += (s, e) => {
                    SelectedReplay = round.replays[p2];
                    SelectedIndex = k;
                    SelectedPlayer = p2id;
                    OtherPlayer = p1id;

                    DialogResult = DialogResult.OK;
                };
            }

            Height += ttr.data.Count * 20;
        }
    }
}
