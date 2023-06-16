using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Keyboard_Inspector {
    class Result: IBinary {
        static readonly char[] Header = new char[] { 'K', 'B', 'I', '\0' };
        static readonly uint FileVersion = 1;

        public string Title;
        public DateTime Recorded;

        public double Time;
        public List<Event> Events;
        public Dictionary<long, Source> Sources;

        public Analysis Analysis;

        public Result(string title, DateTime recorded, double time, List<Event> events, Dictionary<long, Source> sources, Analysis analysis = null) {
            Title = title?? "";
            Recorded = recorded;
            Time = time;
            
            // Filter auto-repeat
            Dictionary<Input, bool> last = new Dictionary<Input, bool>();
            Events = new List<Event>(events.Count);

            for (int i = 0; i < events.Count; i++) {
                var e = events[i];

                if (last.ContainsKey(e.Input) && last[e.Input] == e.Pressed)
                    continue;

                last[e.Input] = e.Pressed;
                Events.Add(e);
            }

            Sources = sources;

            Analysis = analysis?? new Analysis();
            Analysis.Result = this;
        }

        public string GetTitle()
            => string.IsNullOrWhiteSpace(Title)? $"{GetBestSource(0.66)?.Name?? "Untitled"} on {Recorded:MM/dd/yyyy, h:mm:ss tt}" : Title;

        public Source GetBestSource(double threshold = 0) {
            Source best = null;

            foreach (var source in Sources.Values) {
                if (source.Count > (best?.Count ?? 0))
                    best = source;
            }

            if (best?.Count <= Events.Count * threshold)
                return null;

            return best;
        }

        public void ToBinary(BinaryWriter bw) {
            bw.Write(Header);
            bw.Write(FileVersion);

            bw.Write(Title);
            bw.Write(Recorded.ToBinary());

            bw.Write(Time);
            Events.ToBinary(bw);
            Sources.ToBinary(bw);
            Analysis.ToBinary(bw);
        }

        static Dictionary<int, string> LegacySources = new Dictionary<int, string>() {
            {0, "Keyboard"},
            {1, "Gamepad"},
            {2, "Mouse"},
            {3, "TETR.IO Replay"}
        };

        public static Result FromBinary(BinaryReader br) {
            if (!br.ReadChars(Header.Length).SequenceEqual(Header))
                throw new Exception("Invalid header");

            uint fileVersion = br.ReadUInt32();

            string title = br.ReadString();
            DateTime recorded = DateTime.FromBinary(br.ReadInt64());
            double time = br.ReadDouble();
            List<Event> events = Event.ListFromBinary(br, fileVersion);

            Dictionary<long, Source> sources = new Dictionary<long, Source>();

            if (fileVersion == 0) {
                var srcs = events.Select(i => i.Input.Source);
                var uniques = srcs.Distinct().ToList();

                foreach (var entry in LegacySources) {
                    if (!uniques.Contains(entry.Key)) continue;
                    
                    sources.Add(entry.Key, new Source(srcs.Count(i => i == entry.Key), entry.Value));
                }

            } else {
                sources = Source.DictionaryFromBinary(br, fileVersion);
            }

            Analysis analysis = Analysis.FromBinary(br, fileVersion);

            return new Result(title, recorded, time, events, sources, analysis);
        }

        public static Result FromPath(string path) {
            using (MemoryStream ms = new MemoryStream(File.ReadAllBytes(path))) {
                using (BinaryReader br = new BinaryReader(ms)) {
                    return FromBinary(br);
                }
            }
        }

        public static bool IsEmpty(Result result) => result?.Events.Any() != true;
    }
}
