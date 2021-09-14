using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Keyboard_Inspector {
    static class Recorder {
        public delegate void RecordEventMethod(Event e);

        static Stopwatch time;
        static List<Event> events;
        static List<Poll> polls;

        static double ElapsedPrecise(this Stopwatch sw) => (double)sw.ElapsedTicks / Stopwatch.Frequency;

        public static bool IsRecording { get; private set; }

        public static void StartRecording() {
            if (IsRecording)
                StopRecording();

            events = new List<Event>();
            polls = new List<Poll>();

            time?.Stop();
            time = new Stopwatch();
            time.Start();

            IsRecording = true;

            KeyListener.Start();
            NestopiaListener.Start();
            WiitarListener.Start();
        }

        public static Result StopRecording() {
            if (!IsRecording) return null;

            KeyListener.Stop();
            NestopiaListener.Stop();
            WiitarListener.Stop();

            time.Stop();

            IsRecording = false;

            return new Result(time.ElapsedPrecise(), events, polls);
        }

        public static void RecordInput(bool pressed, Input input)
            => events.Add(new Event(time.ElapsedPrecise(), pressed, input));

        // Used if there is support for recording poll events separately from input events (emulated environments, for example)
        public static void RecordPoll()
            => polls.Add(new Poll(time.ElapsedPrecise()));
    }
}
