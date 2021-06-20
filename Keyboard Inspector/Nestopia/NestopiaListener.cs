using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Keyboard_Inspector {
    static partial class NestopiaListener {
        public enum AttachResult {
            SUCCESS,
            FAILED,
            BAD_VER
        }

        static Task DebuggerThread = null;
        public static bool IsConnected => DebuggerThread?.IsCompleted == false;

        static bool reporting = false;

        static bool abort = false;

        static IntPtr verificationAddress = new IntPtr(0x0046F666);
        static readonly byte[] verificationData = new byte[] {
            0x25, 0xCF, 0x00, 0x00, 0x00,
            0x8B, 0xD0,
            0x81, 0xE2, 0xC0, 0x00, 0x00, 0x00,
            0x80, 0xFA, 0xC0,
            0x75, 0x03,
            0x83, 0xE0, 0x3F,
            0x89, 0x43, 0x18, /* breakpointed instruction */
            0x8B, 0x46, 0x04,
            0x09, 0x05, 0x78, 0x6F, 0x57, 0x00,
            0x5F,
            0x5E,
            0xC3,
            0xCC
        };

        static IntPtr breakpoint = new IntPtr(0x0046F67B);
        static byte[] instruction = new byte[1];

        static uint last = 0;

        static void SetBreakpoint(IntPtr hProcess) {
            ReadProcessMemory(hProcess, breakpoint, instruction, 1, out _);
            WriteProcessMemory(hProcess, breakpoint, new byte[] { 0xCC }, 1, out _);
            FlushInstructionCache(hProcess, breakpoint, new UIntPtr(1));
        }

        static void ResetBreakpoint(IntPtr hProcess) {
            WriteProcessMemory(hProcess, breakpoint, instruction, 1, out _);
            FlushInstructionCache(hProcess, breakpoint, new UIntPtr(1));
        }

        static SemaphoreSlim debuggerAttaching = new SemaphoreSlim(0, 1);
        static SemaphoreSlim debuggerDetaching = new SemaphoreSlim(0, 1);

        public static async Task<AttachResult> AttachDebugger(int pid) {
            if (IsConnected) return AttachResult.SUCCESS;

            AttachResult result = AttachResult.SUCCESS;

            DebuggerThread = Task.Run(() => {
                abort = false;
                bool success = DebugActiveProcess((uint)pid);

                if (!success) {
                    result = AttachResult.FAILED;

                    abort = false;
                    debuggerAttaching.Release();
                    return;
                }

                DEBUG_EVENT e = default;

                if (!WaitForDebugEvent(ref e, 5000) || e.dwDebugEventCode != DebugEventType.CREATE_PROCESS_DEBUG_EVENT) {
                    result = AttachResult.FAILED;

                    DebugActiveProcessStop((uint)pid);

                    abort = false;
                    debuggerAttaching.Release();
                    return;
                }

                IntPtr hProcess = e.CreateProcessInfo.hProcess;
                IntPtr hThread = e.CreateProcessInfo.hThread;

                byte[] verify = new byte[verificationData.Length];
                ReadProcessMemory(hProcess, verificationAddress, verify, verify.Length, out _);

                if (!verificationData.SequenceEqual(verify)) {
                    result = AttachResult.BAD_VER;

                    debuggerAttaching.Release();
                    return;
                }

                // Success! Let's go
                debuggerAttaching.Release();

                SetBreakpoint(hProcess);

                ContinueDebugEvent(e.dwProcessId, e.dwThreadId, ContinueStatus.DBG_CONTINUE);

                while (!abort) {
                    // Check for abort every 200ms
                    if (!WaitForDebugEvent(ref e, 200)) continue;

                    if (e.dwDebugEventCode == DebugEventType.EXCEPTION_DEBUG_EVENT && e.Exception.ExceptionRecord.ExceptionCode == 0x80000003u && e.Exception.ExceptionRecord.ExceptionAddress == breakpoint) {
                        CONTEXT c = default;
                        c.ContextFlags = (uint)CONTEXT_FLAGS.CONTEXT_ALL;
                        GetThreadContext(hThread, ref c);

                        byte[] pad = new byte[4];
                        ReadProcessMemory(hProcess, new IntPtr(c.Esp - 4), pad, 4, out _);
                        int i = BitConverter.ToInt32(pad, 0);

                        if (i == 0) {
                            if (reporting) {
                                for (uint i1 = last, i2 = c.Eax, n = 0; n < Enum.GetNames(typeof(NESKeys)).Length; i1 >>= 1, i2 >>= 1, n++) {
                                    if ((i1 & 1) != (i2 & 1))
                                        Recorder.RecordInput((i2 & 1) == 1, new NESInput((NESKeys)n));
                                }
                            }

                            last = c.Eax;
                        }

                        c.Eip--;
                        c.EFlags |= 0x100; // Set trap flag (single step)
                        SetThreadContext(hThread, ref c);

                        ResetBreakpoint(hProcess);

                        do {
                            ContinueDebugEvent(e.dwProcessId, e.dwThreadId, ContinueStatus.DBG_CONTINUE);
                            WaitForDebugEvent(ref e, 1000);

                        } while (e.Exception.ExceptionRecord.ExceptionCode != 0x80000004u);

                        if (!abort)
                            SetBreakpoint(hProcess);

                    }

                    ContinueDebugEvent(e.dwProcessId, e.dwThreadId, ContinueStatus.DBG_CONTINUE);

                    if (e.dwDebugEventCode == DebugEventType.EXIT_PROCESS_DEBUG_EVENT) // User closed Nestopia
                        abort = true;
                }

                abort = false;

                if (e.dwDebugEventCode != DebugEventType.EXIT_PROCESS_DEBUG_EVENT) {
                    ResetBreakpoint(hProcess);
                    DebugActiveProcessStop((uint)pid);

                    debuggerDetaching.Release();

                } else MainForm.Instance?.NestopiaWasDisconnected();
            });

            await debuggerAttaching.WaitAsync();

            return result;
        }

        public static async Task DetachDebugger() {
            if (!IsConnected) return;

            abort = true;
            await debuggerDetaching.WaitAsync();
        }

        public static void Start() {
            reporting = true;

            for (uint i = last, n = 0; n < 8; i >>= 1, n++) {
                if ((i & 1) == 1)
                    Recorder.RecordInput(true, new NESInput((NESKeys)n));
            }
        }

        public static void Stop() {
            reporting = false;
        }
    }
}
