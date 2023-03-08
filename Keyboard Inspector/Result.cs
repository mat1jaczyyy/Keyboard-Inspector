using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace Keyboard_Inspector {
    class Result: IBinary {
        public string GetTitle()
            => string.IsNullOrWhiteSpace(Title)? $"Untitled {Recorded:MM/dd/yyyy, h:mm:ss tt}" : Title;

        public string Title;
        public DateTime Recorded;

        public double Time;
        public ReadOnlyCollection<Event> Events;

        public Result(string title, DateTime recorded, double time, List<Event> events) {
            Title = title;
            Recorded = recorded;
            Time = time;
            
            // Filter auto-repeat
            Events = events
                .Where((x, i) => !x.Pressed || !(events.Take(i).Where(j => j.Input == x.Input).LastOrDefault()?.Pressed ?? false))
                .ToList().AsReadOnly();
        }

        public void ToBinary(BinaryWriter bw) {
            bw.Write(Title);
            bw.Write(Recorded.ToBinary());

            bw.Write(Time);
            Events.ToBinary(bw);
        }

        public static Result FromBinary(BinaryReader br) {
            return new Result(
                br.ReadString(),
                DateTime.FromBinary(br.ReadInt64()),
                br.ReadDouble(),
                Event.ListFromBinary(br)
            );
        }
    }
}
