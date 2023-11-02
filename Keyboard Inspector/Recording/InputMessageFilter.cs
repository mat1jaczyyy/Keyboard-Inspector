using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Keyboard_Inspector {
    class InputMessageFilter: IMessageFilter {
        ConcurrentQueue<UnprocessedInput> queue = new ConcurrentQueue<UnprocessedInput>();

        Task ProcessThread;

        public bool PreFilterMessage(ref Message m) {
            const int WM_INPUT = 0x00FF;

            if (m.Msg == WM_INPUT && Recorder.IsRecording) {
                double time = Recorder.Elapsed;

                if (Native.GetRawInputData(m.LParam, out Native.RawHID raw)) {
                    queue.Enqueue(new UnprocessedInput() {
                        Time = time,
                        Raw = raw
                    });

                    if (ProcessThread?.IsCompleted != false) {
                        ProcessThread = Task.Run(() => {
                            while (queue.TryDequeue(out var input))
                                input.Process();
                        });
                    }
                }

                return true;
            }

            return false;
        }
    }
}
