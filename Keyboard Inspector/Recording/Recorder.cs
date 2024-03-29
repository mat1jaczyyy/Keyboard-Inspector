﻿using System;
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

            events = new List<Event>(200000);
            sources = new Dictionary<long, int>(50);

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

            var result = new Result("", DateTime.Now, ElapsedPrecise, events, resolvedSources);

            result.SortInputsByDevice();

            return result;
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
