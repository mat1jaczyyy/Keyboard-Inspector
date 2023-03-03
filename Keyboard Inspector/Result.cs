using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace Keyboard_Inspector {
    class Result: IBinary {
        public double Time;

        public ReadOnlyCollection<Event> Events;

        public Result(double time, List<Event> events) {
            Time = time;
            
            // Filter Windows auto-repeat
            Events = events
                .Where(i => i.Input is KeyInput || i.Input is WiitarInput)
                .Where((x, i) => !x.Pressed || !(events.Take(i).Where(j => j.Input == x.Input).LastOrDefault()?.Pressed ?? false))
                .ToList().AsReadOnly();
        }

        public void ToBinary(BinaryWriter bw) {
            bw.Write(Time);
            Events.ToBinary(bw);
        }

        public static Result FromBinary(BinaryReader br) {
            return new Result(
                br.ReadDouble(),
                Event.ListFromBinary(br)
            );
        }
    }
}
