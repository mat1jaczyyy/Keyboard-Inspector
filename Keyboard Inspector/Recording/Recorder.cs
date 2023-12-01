using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Keyboard_Inspector {
    static class Recorder {
        public delegate void RecordEventMethod(Event e);

        static Stopwatch time;
        static List<Event> events;
        static Dictionary<long, int> sources;

        public static double ElapsedPrecise => (double)time.ElapsedTicks / Stopwatch.Frequency;

        public static bool IsRecording { get; private set; }

        public static void StartRecording() {
            if (IsRecording)
                StopRecording();

            // TODO maybe increase initial capacity?
            events = new List<Event>();
            sources = new Dictionary<long, int>();

            time?.Stop();
            time = new Stopwatch();
            time.Start();

            IsRecording = true;

            Listener.Start();
        }

        public static Result StopRecording() {
            if (!IsRecording) return null;

            Listener.Stop();

            time.Stop();

            IsRecording = false;

            var resolvedSources = sources.Keys.ToDictionary(i => i, i => Source.FromHandle(i, sources[i]));

            foreach (var duplicates in resolvedSources.GroupBy(i => i.Value.Name).Where(i => i.Count() > 1)) {
                int i = 0;
                foreach (var source in duplicates)
                    source.Value.AppendIndex(i++);
            }

            return new Result("", DateTime.Now, ElapsedPrecise, events, resolvedSources);
        }

        public static void RecordInput(bool pressed, Input input)
            => RecordInput(ElapsedPrecise, pressed, input);

        public static void RecordInput(double precise, bool pressed, Input input) {
            events.Add(new Event(precise, pressed, input));

            if (!sources.ContainsKey(input.Source))
                sources[input.Source] = 0;

            sources[input.Source]++;
        }
    }
}
