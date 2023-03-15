using System;
using System.Diagnostics;

namespace Keyboard_Inspector {
    class Benchmark: IDisposable {
        Stopwatch s;
        double last = 0;

        public Benchmark() {
            s = new Stopwatch();
            Console.WriteLine("-------------------------------------------------------");
            Tick("START");
            s.Start();
        }

        public void Tick(string text) {
            double tick = s.ElapsedTicks * 1000.0 / Stopwatch.Frequency;
            if (s.IsRunning)
                Console.WriteLine($"{text,30} {tick - last,8:0.000}ms {tick,8:0.000}ms");
            else
                Console.WriteLine($"{text,30} {"",10} {tick,8:0.000}ms");
            last = tick;
        }

        public void Dispose() {
            s.Stop();
            Tick("~END");
        }
    }
}
