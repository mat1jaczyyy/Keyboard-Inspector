using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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

            if (isMulti) {
                title += $"{ttr.endcontext[0].user.username.Value.ToUpper()} ({ttr.endcontext[0].wins.Value}) versus ";
                title += $"({ttr.endcontext[1].wins.Value}) {ttr.endcontext[1].user.username.Value.ToUpper()} played ";

            } else {
                if (gametype == "40 LINES") title += $"{ttr.endcontext.finalTime.Value / 1000.0:0.000} ";
                if (gametype == "BLITZ") title += $"{ttr.endcontext.score.Value:0,000} ";

                title += $"played by {ttr.user.username.Value.ToUpper()} ";
            }

            DateTime ts = ttr.ts.Value.ToLocalTime();
            title += $"on {ts:MM/dd/yyyy, h:mm:ss tt}";

            return new Result(
                title,
                ts,
                ttr.data.frames.Value / 60.0,
                (ttr.data.events as JArray)
                    .Select(i => (i as dynamic))
                    .Where(i => i.type.Value.StartsWith("key") && i.data.hoisted?.Value != true)
                    .Select(i => new Event(
                        (i.frame.Value + i.data.subframe.Value) / 60.0,
                        i.type.Value == "keydown",
                        new TetrioInput(Enum.TryParse<TetrioKeys>(i.data.key.Value, out TetrioKeys key) ? key : throw new Exception("Unknown TETR.IO key"))
                    ))
                    .ToList()
            );
        }
    }
}
