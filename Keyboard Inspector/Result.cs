using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Keyboard_Inspector {
    class Result: IBinary {
        static readonly char[] Header = new char[] { 'K', 'B', 'I', '\0' };
        static readonly uint FileVersion = 0;
        
        public string GetTitle()
            => string.IsNullOrWhiteSpace(Title)? $"Untitled {Recorded:MM/dd/yyyy, h:mm:ss tt}" : Title;

        public string Title;
        public DateTime Recorded;

        public double Time;
        public List<Event> Events;

        public Analysis Analysis;

        public Result(string title, DateTime recorded, double time, List<Event> events, Analysis analysis = null) {
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

            Analysis = analysis?? new Analysis();
        }

        public void ToBinary(BinaryWriter bw) {
            bw.Write(Header);
            bw.Write(FileVersion);

            bw.Write(Title);
            bw.Write(Recorded.ToBinary());

            bw.Write(Time);
            Events.ToBinary(bw);

            Analysis.ToBinary(bw);
        }

        public static Result FromBinary(BinaryReader br) {
            if (!br.ReadChars(Header.Length).SequenceEqual(Header))
                throw new Exception("Invalid header");

            uint fileVersion = br.ReadUInt32();

            return new Result(
                br.ReadString(),
                DateTime.FromBinary(br.ReadInt64()),
                br.ReadDouble(),
                Event.ListFromBinary(br, fileVersion),
                Analysis.FromBinary(br, fileVersion)
            );
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
