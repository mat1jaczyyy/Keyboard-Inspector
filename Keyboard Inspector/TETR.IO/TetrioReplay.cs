using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

using DynaJson;

namespace Keyboard_Inspector {
    enum TetrioKeys {
        moveLeft = 0,
        moveRight = 1,
        softDrop = 2,
        hardDrop = 3,
        rotateCCW = 4,
        rotateCW = 5,
        rotate180 = 6,
        hold = 7
    };

    static class TetrioReplay {
        static Dictionary<string, string> gametypes = new Dictionary<string, string>() {
            {"league", "TETRA LEAGUE"},
            {"40l", "40 LINES"},
            {"blitz", "BLITZ"},
        };

        public static Result StreamToResult(Stream stream) {
            dynamic ttr = JsonObject.Parse(stream);

            string title = "";

            if (gametypes.TryGetValue(ttr.gametype()? ttr.gametype : "", out string gametype))
                title = $"[{gametype}] ";

            bool isMulti = ttr.ismulti()? (ttr.ismulti == true) : false;
            dynamic data;

            if (isMulti) {
                var form = new TTRMPickerForm(ttr);

                if (form.ShowDialog() == DialogResult.OK) {
                    data = form.SelectedReplay;
                } else {
                    return null;
                }

                int p1 = ttr.endcontext[0].user._id == form.SelectedPlayer? 0 : 1;
                int p2 = ttr.endcontext[1].user._id == form.OtherPlayer? 1 : 0;

                title += $"{ttr.endcontext[p1].user.username.ToUpper()} ({ttr.endcontext[p1].wins}) versus ";
                title += $"({ttr.endcontext[p2].wins}) {ttr.endcontext[p2].user.username.ToUpper()}, ";
                title += $"round {form.SelectedIndex + 1}/{ttr.data.Count}, played ";

            } else {
                if (gametype == "40 LINES") title += $"{ttr.endcontext.finalTime / 1000.0:0.000} ";
                if (gametype == "BLITZ") title += $"{ttr.endcontext.score:0,000} ";

                var user = ttr.user.username;
                if (!string.IsNullOrWhiteSpace(user))
                    title += $"played by {user.ToUpper()} ";

                data = ttr.data;
            }

            DateTime ts = DateTime.Parse(ttr.ts, null, DateTimeStyles.RoundtripKind).ToLocalTime();
            title += $"on {ts:MM/dd/yyyy, h:mm:ss tt}";

            List<Event> events = new List<Event>(data.events.Count);

            for (int i = 0; i < data.events.Count; i++) {
                dynamic v = data.events[i];

                if (!v.type.StartsWith("key")) continue;
                if (v.data.hoisted() && v.data.hoisted == true) continue;

                events.Add(new Event(
                    Math.Round((double)(v.frame + v.data.subframe), 1) / 60.0,
                    v.type == "keydown",
                    new Input(v.data.key, 0)
                ));
            }

            return new Result(
                title,
                ts,
                data.frames / 60.0,
                events,
                new Dictionary<long, Source>() {{0, new Source(events.Count, "TETR.IO Replay")}},
                new Analysis() { Precision = 600 }
            );
        }
    }
}
