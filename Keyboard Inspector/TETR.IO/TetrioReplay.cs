using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

using DynaJson;

namespace Keyboard_Inspector {
    class TetrioReplay {
        static Dictionary<string, string> gametypes = new Dictionary<string, string>() {
            {"league", "TETRA LEAGUE"},
            {"40l", "40 LINES"},
            {"blitz", "BLITZ"},
        };

        public static Result ConvertToResult(string path) {
            dynamic ttr;
            using (var reader = File.OpenRead(path))
                ttr = JsonObject.Parse(reader);

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

                title += $"{ttr.endcontext[form.SelectedPlayer].user.username.ToUpper()} ({ttr.endcontext[form.SelectedPlayer].wins}) versus ";
                title += $"({ttr.endcontext[1 - form.SelectedPlayer].wins}) {ttr.endcontext[1 - form.SelectedPlayer].user.username.ToUpper()}, ";
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
                    new TetrioInput(Enum.TryParse<TetrioKeys>(v.data.key, out TetrioKeys key)? key : throw new Exception("Unknown TETR.IO key"))
                ));
            }

            return new Result(title, ts, data.frames / 60.0, events);
        }
    }
}
