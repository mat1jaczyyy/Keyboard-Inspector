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
        static Dictionary<string, string> gamemodes = new Dictionary<string, string>() {
            {"league", "TETRA LEAGUE"},
            {"40l", "40 LINES"},
            {"blitz", "BLITZ"},
            {"zenith", "QUICK PLAY"},
            {"zenithex", "EXPERT QUICK PLAY"}
        };

        public static Result StreamToResult(Stream stream) {
            dynamic ttr = JsonObject.Parse(stream);

            int version = ttr.version()? (int)ttr.version : -1;

            if (version < 1) {
                throw new ExplainableFileLoadException("Only TETR.IO Beta replays are supported.");
            }

            string title = "";

            string gamemode = (ttr.gamemode()? ttr.gamemode : "")?? "";

            if (gamemodes.TryGetValue(gamemode, out string gamemodeStr))
                title = $"[{gamemodeStr}] ";

            bool isMulti = ttr.users()? (ttr.users.Count > 1) : false;
            dynamic data;

            if (isMulti) {
                var form = new TTRMPickerForm(ttr);

                if (MainForm.Instance.InvokeIfRequired(form.ShowDialog) == DialogResult.OK) {
                    data = form.SelectedReplay;
                } else {
                    return null;
                }

                int p1 = ttr.replay.leaderboard[0].id == form.SelectedPlayer? 0 : 1;
                int p2 = ttr.replay.leaderboard[1].id == form.OtherPlayer? 1 : 0;

                title += $"{ttr.replay.leaderboard[p1].username.ToUpper()} ({ttr.replay.leaderboard[p1].wins}) versus ";
                title += $"({ttr.replay.leaderboard[p2].wins}) {ttr.replay.leaderboard[p2].username.ToUpper()}, ";
                title += $"round {form.SelectedIndex + 1}/{ttr.replay.rounds.Count}, played ";

            } else {
                if (gamemode == "40l") title += $"{ttr.replay.results.stats.finaltime / 1000.0:0.000} ";
                if (gamemode == "blitz") title += $"{ttr.replay.results.stats.score:#,##0} ";
                if (gamemode == "zenith" || gamemode == "zenithex") title += $"{ttr.replay.results.stats.zenith.altitude:#,##0.0} m ";

                var user = ttr.users[0].username;
                if (!string.IsNullOrWhiteSpace(user))
                    title += $"played by {user.ToUpper()} ";

                data = ttr.replay;
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
                new Analysis() { BinRate = 600 }
            );
        }
    }
}
