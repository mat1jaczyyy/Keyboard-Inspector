using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Keyboard_Inspector {
    static class Recorder {
        public static Result Recording { get; private set; }

        static Stopwatch time;
        public static double Elapsed => (double)time.ElapsedTicks / Stopwatch.Frequency;

        public static bool IsRecording => Recording != null;

        public static void StopRecording() {
            if (!IsRecording) return;

            ListenerForm.Instance.InvokeIfRequired(() => ListenerForm.Instance.RegisterRawInput(false));

            time.Stop();

            Recording.Time = Elapsed;
            Recording = null;
        }

        public static void StartRecording(Result result) {
            if (IsRecording)
                StopRecording();

            time?.Stop();
            time = new Stopwatch();

            Recording = result;
            time.Start();

            ListenerForm.Instance.InvokeIfRequired(() => ListenerForm.Instance.RegisterRawInput(true));
        }

        public static void RecordInput(double time, bool pressed, Input input)
            => Recording.Record(new Event(time, pressed, input));
    }
}
