using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

using Newtonsoft.Json.Linq;

namespace Keyboard_Inspector {
    class TetrioReplay {
        static Dictionary<string, string> gametypes = new Dictionary<string, string>() {
            {"league", "TETRA LEAGUE"},
            {"40l", "40 LINES"},
            {"blitz", "BLITZ"},
        };

        public static Result ConvertToResult(string path) {
            dynamic ttr = JObject.Parse(File.ReadAllText(path));

            string title = "";

            if (gametypes.TryGetValue(ttr.gametype.Value?? "", out string gametype))
                title = $"[{gametype}] ";

            bool isMulti = ttr.ismulti?.Value == true;
            dynamic data;

            if (isMulti) {
                var form = new TTRMPickerForm(ttr);

                if (form.ShowDialog() == DialogResult.OK) {
                    data = form.SelectedReplay;
                } else {
                    return null;
                }

                title += $"{ttr.endcontext[form.SelectedPlayer].user.username.Value.ToUpper()} ({ttr.endcontext[form.SelectedPlayer].wins.Value}) versus ";
                title += $"({ttr.endcontext[1 - form.SelectedPlayer].wins.Value}) {ttr.endcontext[1 - form.SelectedPlayer].user.username.Value.ToUpper()}, ";
                title += $"round {form.SelectedIndex + 1}/{(ttr.data as JArray).Count}, played ";

            } else {
                if (gametype == "40 LINES") title += $"{ttr.endcontext.finalTime.Value / 1000.0:0.000} ";
                if (gametype == "BLITZ") title += $"{ttr.endcontext.score.Value:0,000} ";

                var user = ttr.user.username.Value;
                if (!string.IsNullOrWhiteSpace(user))
                    title += $"played by {user.ToUpper()} ";

                data = ttr.data;
            }

            DateTime ts = ttr.ts.Value.ToLocalTime();
            title += $"on {ts:MM/dd/yyyy, h:mm:ss tt}";

            return new Result(
                title,
                ts,
                data.frames.Value / 60.0,
                (data.events as JArray)
                    .Select(i => i as dynamic)
                    .Where(i => i.type.Value.StartsWith("key") && i.data.hoisted?.Value != true)
                    .Select(i => new Event(
                        Math.Round((double)(i.frame.Value + i.data.subframe.Value), 1) / 60.0,
                        i.type.Value == "keydown",
                        new TetrioInput(Enum.TryParse<TetrioKeys>(i.data.key.Value, out TetrioKeys key)? key : throw new Exception("Unknown TETR.IO key"))
                    ))
                    .ToList()
            );
        }
    }
}
