using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using DarkUI.Controls;
using DarkUI.Forms;

using Newtonsoft.Json.Linq;

namespace Keyboard_Inspector {
    public partial class TTRMPickerForm: DarkForm {
        public dynamic SelectedReplay { get; private set; }
        public int SelectedIndex { get; private set; }
        public int SelectedPlayer { get; private set; }

        public TTRMPickerForm(dynamic ttr) {
            InitializeComponent();

            p1.Text = ttr.endcontext[0].user.username.Value.ToUpper();
            p2.Text = ttr.endcontext[1].user.username.Value.ToUpper();

            var data = ttr.data as JArray;

            for (int i = 0; i < data.Count; i++) {
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

                left.Location = new Point(p1.Location.X, 41 + i * 20);
                left.Size = new Size(p1.Width, 13);

                right.Location = new Point(p2.Location.X, 41 + i * 20);
                right.Size = new Size(p2.Width, 13);

                time.Location = new Point(vs.Location.X, 41 + i * 20);
                time.Size = new Size(vs.Width, 13);

                var round = data[i] as dynamic;

                left.Text = (round.replays[0].events as JArray)
                    .Select(j => j as dynamic)
                    .Count(j => j.type.Value.StartsWith("key") && j.data.hoisted?.Value != true).ToString();

                right.Text = (round.replays[1].events as JArray)
                    .Select(j => j as dynamic)
                    .Count(j => j.type.Value.StartsWith("key") && j.data.hoisted?.Value != true).ToString();

                var seconds = (int)(Math.Min(round.replays[0].frames.Value, round.replays[1].frames.Value) / 60.0);

                time.Text = $"{seconds / 60:0}:{seconds % 60:00}";

                if (round.board[0].success.Value)
                    left.BackColor = p1.BackColor;

                if (round.board[1].success.Value)
                    right.BackColor = p2.BackColor;

                Controls.Add(left);
                Controls.Add(right);
                Controls.Add(time);

                int k = i;

                left.Click += (s, e) => {
                    SelectedReplay = round.replays[0];
                    SelectedIndex = k;
                    SelectedPlayer = 0;

                    DialogResult = DialogResult.OK;
                };

                right.Click += (s, e) => {
                    SelectedReplay = round.replays[1];
                    SelectedIndex = k;
                    SelectedPlayer = 1;

                    DialogResult = DialogResult.OK;
                };
            }

            Height += data.Count * 20;
        }
    }
}
