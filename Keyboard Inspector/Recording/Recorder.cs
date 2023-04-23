using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Keyboard_Inspector {
    static class Recorder {
        public delegate void RecordEventMethod(Event e);

        static Stopwatch time;
        static List<Event> events;

        public static double ElapsedPrecise => (double)time.ElapsedTicks / Stopwatch.Frequency;

        public static bool IsRecording { get; private set; }

        public static void StartRecording() {
            if (IsRecording)
                StopRecording();

            // TODO maybe increase capacity?
            events = new List<Event>();

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

            return new Result("", DateTime.Now, ElapsedPrecise, events);
        }

        public static void RecordInput(bool pressed, Input input)
            => events.Add(new Event(ElapsedPrecise, pressed, input));

        public static void RecordInput(double precise, bool pressed, Input input)
            => events.Add(new Event(precise, pressed, input));
    }
}
