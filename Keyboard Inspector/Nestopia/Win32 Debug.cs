using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Keyboard_Inspector {
    static partial class NestopiaListener {
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool DebugActiveProcess(uint dwProcessId);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool DebugActiveProcessStop(uint dwProcessId);

        enum DebugEventType : uint {
            RIP_EVENT = 9,
            OUTPUT_DEBUG_STRING_EVENT = 8,
            UNLOAD_DLL_DEBUG_EVENT = 7,
            LOAD_DLL_DEBUG_EVENT = 6,
            EXIT_PROCESS_DEBUG_EVENT = 5,
            EXIT_THREAD_DEBUG_EVENT = 4,
            CREATE_PROCESS_DEBUG_EVENT = 3,
            CREATE_THREAD_DEBUG_EVENT = 2,
            EXCEPTION_DEBUG_EVENT = 1,
        }

        [StructLayout(LayoutKind.Sequential)]
        struct EXCEPTION_RECORD {
            public uint ExceptionCode;
            public uint ExceptionFlags;
            public IntPtr ExceptionRecord;
            public IntPtr ExceptionAddress;
            public uint NumberParameters;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15, ArraySubType = UnmanagedType.U4)]
            public uint[] ExceptionInformation;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct EXCEPTION_DEBUG_INFO {
            public EXCEPTION_RECORD ExceptionRecord;
            public uint dwFirstChance;
        }

        delegate uint PTHREAD_START_ROUTINE(IntPtr lpThreadParameter);

        [StructLayout(LayoutKind.Sequential)]
        struct CREATE_THREAD_DEBUG_INFO {
            public IntPtr hThread;
            public IntPtr lpThreadLocalBase;
            public PTHREAD_START_ROUTINE lpStartAddress;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct CREATE_PROCESS_DEBUG_INFO {
            public IntPtr hFile;
            public IntPtr hProcess;
            public IntPtr hThread;
            public IntPtr lpBaseOfImage;
            public uint dwDebugInfoFileOffset;
            public uint nDebugInfoSize;
            public IntPtr lpThreadLocalBase;
            public PTHREAD_START_ROUTINE lpStartAddress;
            public IntPtr lpImageName;
            public ushort fUnicode;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct EXIT_THREAD_DEBUG_INFO {
            public uint dwExitCode;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct EXIT_PROCESS_DEBUG_INFO {
            public uint dwExitCode;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct LOAD_DLL_DEBUG_INFO {
            public IntPtr hFile;
            public IntPtr lpBaseOfDll;
            public uint dwDebugInfoFileOffset;
            public uint nDebugInfoSize;
            public IntPtr lpImageName;
            public ushort fUnicode;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct UNLOAD_DLL_DEBUG_INFO {
            public IntPtr lpBaseOfDll;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct OUTPUT_DEBUG_STRING_INFO {
            [MarshalAs(UnmanagedType.LPStr)] public string lpDebugStringData;
            public ushort fUnicode;
            public ushort nDebugStringLength;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct RIP_INFO {
            public uint dwError;
            public uint dwType;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct DEBUG_EVENT {
            public DebugEventType dwDebugEventCode;
            public int dwProcessId;
            public int dwThreadId;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 86, ArraySubType = UnmanagedType.U1)]
            byte[] debugInfo;

            public EXCEPTION_DEBUG_INFO Exception => GetDebugInfo<EXCEPTION_DEBUG_INFO>();
            public CREATE_THREAD_DEBUG_INFO CreateThread => GetDebugInfo<CREATE_THREAD_DEBUG_INFO>();
            public CREATE_PROCESS_DEBUG_INFO CreateProcessInfo => GetDebugInfo<CREATE_PROCESS_DEBUG_INFO>();
            public EXIT_THREAD_DEBUG_INFO ExitThread => GetDebugInfo<EXIT_THREAD_DEBUG_INFO>();
            public EXIT_PROCESS_DEBUG_INFO ExitProcess => GetDebugInfo<EXIT_PROCESS_DEBUG_INFO>();
            public LOAD_DLL_DEBUG_INFO LoadDll => GetDebugInfo<LOAD_DLL_DEBUG_INFO>();
            public UNLOAD_DLL_DEBUG_INFO UnloadDll => GetDebugInfo<UNLOAD_DLL_DEBUG_INFO>();
            public OUTPUT_DEBUG_STRING_INFO DebugString => GetDebugInfo<OUTPUT_DEBUG_STRING_INFO>();
            public RIP_INFO RipInfo => GetDebugInfo<RIP_INFO>();

            private T GetDebugInfo<T>() where T : struct {
                var structSize = Marshal.SizeOf(typeof(T));
                var pointer = Marshal.AllocHGlobal(structSize);
                Marshal.Copy(debugInfo, 0, pointer, structSize);

                var result = Marshal.PtrToStructure(pointer, typeof(T));
                Marshal.FreeHGlobal(pointer);
                return (T)result;
            }
        }

        [DllImport("kernel32.dll", EntryPoint = "WaitForDebugEvent")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool WaitForDebugEvent(ref DEBUG_EVENT lpDebugEvent, uint dwMilliseconds);

        enum ContinueStatus : uint {
            DBG_CONTINUE = 0x00010002,
            DBG_EXCEPTION_NOT_HANDLED = 0x80010001,
            DBG_REPLY_LATER = 0x40010001
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool ContinueDebugEvent(int dwProcessId, int dwThreadId, ContinueStatus dwContinueStatus);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, int nSize, out IntPtr lpNumberOfBytesRead);

        [DllImport("kernel32.dll")]
        static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, int nSize, out IntPtr lpNumberOfBytesWritten);

        [DllImport("kernel32.dll")]
        static extern bool FlushInstructionCache(IntPtr hProcess, IntPtr lpBaseAddress, UIntPtr dwSize);

        enum CONTEXT_FLAGS : uint {
            CONTEXT_i386 = 0x10000,
            CONTEXT_i486 = 0x10000,   //  same as i386
            CONTEXT_CONTROL = CONTEXT_i386 | 0x01, // SS:SP, CS:IP, FLAGS, BP
            CONTEXT_INTEGER = CONTEXT_i386 | 0x02, // AX, BX, CX, DX, SI, DI
            CONTEXT_SEGMENTS = CONTEXT_i386 | 0x04, // DS, ES, FS, GS
            CONTEXT_FLOATING_POINT = CONTEXT_i386 | 0x08, // 387 state
            CONTEXT_DEBUG_REGISTERS = CONTEXT_i386 | 0x10, // DB 0-3,6,7
            CONTEXT_EXTENDED_REGISTERS = CONTEXT_i386 | 0x20, // cpu specific extensions
            CONTEXT_FULL = CONTEXT_CONTROL | CONTEXT_INTEGER | CONTEXT_SEGMENTS,
            CONTEXT_ALL = CONTEXT_CONTROL | CONTEXT_INTEGER | CONTEXT_SEGMENTS | CONTEXT_FLOATING_POINT | CONTEXT_DEBUG_REGISTERS | CONTEXT_EXTENDED_REGISTERS
        }

        [StructLayout(LayoutKind.Sequential)]
        struct FLOATING_SAVE_AREA {
            public uint ControlWord;
            public uint StatusWord;
            public uint TagWord;
            public uint ErrorOffset;
            public uint ErrorSelector;
            public uint DataOffset;
            public uint DataSelector;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 80)]
            public byte[] RegisterArea;
            public uint Cr0NpxState;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct CONTEXT {
            public uint ContextFlags; //set this to an appropriate value
                                      // Retrieved by CONTEXT_DEBUG_REGISTERS
            public uint Dr0;
            public uint Dr1;
            public uint Dr2;
            public uint Dr3;
            public uint Dr6;
            public uint Dr7;
            // Retrieved by CONTEXT_FLOATING_POINT
            public FLOATING_SAVE_AREA FloatSave;
            // Retrieved by CONTEXT_SEGMENTS
            public uint SegGs;
            public uint SegFs;
            public uint SegEs;
            public uint SegDs;
            // Retrieved by CONTEXT_INTEGER
            public uint Edi;
            public uint Esi;
            public uint Ebx;
            public uint Edx;
            public uint Ecx;
            public uint Eax;
            // Retrieved by CONTEXT_CONTROL
            public uint Ebp;
            public uint Eip;
            public uint SegCs;
            public uint EFlags;
            public uint Esp;
            public uint SegSs;
            // Retrieved by CONTEXT_EXTENDED_REGISTERS
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 512)]
            public byte[] ExtendedRegisters;
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool GetThreadContext(IntPtr hThread, ref CONTEXT lpContext);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool SetThreadContext(IntPtr hThread, ref CONTEXT lpContext);
    }
}
