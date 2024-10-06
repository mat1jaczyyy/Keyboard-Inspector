using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Keyboard_Inspector {
    static class Native {
        /// <summary>
        /// Enumeration containing HID usage page flags.
        /// </summary>
        public enum HIDUsagePage: ushort {
            /// <summary>Unknown usage page.</summary>
            Undefined = 0x00,
            /// <summary>Generic desktop controls.</summary>
            Generic = 0x01,
            /// <summary>Simulation controls.</summary>
            Simulation = 0x02,
            /// <summary>Virtual reality controls.</summary>
            VR = 0x03,
            /// <summary>Sports controls.</summary>
            Sport = 0x04,
            /// <summary>Games controls.</summary>
            Game = 0x05,
            /// <summary>Keyboard controls.</summary>
            Keyboard = 0x07,
            /// <summary>LED controls.</summary>
            LED = 0x08,
            /// <summary>Button.</summary>
            Button = 0x09,
            /// <summary>Ordinal.</summary>
            Ordinal = 0x0A,
            /// <summary>Telephony.</summary>
            Telephony = 0x0B,
            /// <summary>Consumer.</summary>
            Consumer = 0x0C,
            /// <summary>Digitizer.</summary>
            Digitizer = 0x0D,
            /// <summary>Physical interface device.</summary>
            PID = 0x0F,
            /// <summary>Unicode.</summary>
            Unicode = 0x10,
            /// <summary>Alphanumeric display.</summary>
            AlphaNumeric = 0x14,
            /// <summary>Medical instruments.</summary>
            Medical = 0x40,
            /// <summary>Monitor page 0.</summary>
            MonitorPage0 = 0x80,
            /// <summary>Monitor page 1.</summary>
            MonitorPage1 = 0x81,
            /// <summary>Monitor page 2.</summary>
            MonitorPage2 = 0x82,
            /// <summary>Monitor page 3.</summary>
            MonitorPage3 = 0x83,
            /// <summary>Power page 0.</summary>
            PowerPage0 = 0x84,
            /// <summary>Power page 1.</summary>
            PowerPage1 = 0x85,
            /// <summary>Power page 2.</summary>
            PowerPage2 = 0x86,
            /// <summary>Power page 3.</summary>
            PowerPage3 = 0x87,
            /// <summary>Bar code scanner.</summary>
            BarCode = 0x8C,
            /// <summary>Scale page.</summary>
            Scale = 0x8D,
            /// <summary>Magnetic strip reading devices.</summary>
            MSR = 0x8E
        }

        /// <summary>
        /// Enumeration containing the HID usage values.
        /// </summary>
        public enum HIDUsage: ushort {
            Pointer = 0x01,
            Mouse = 0x02,
            Joystick = 0x04,
            Gamepad = 0x05,
            Keyboard = 0x06,
            Keypad = 0x07,
            SystemControl = 0x80,
            X = 0x30,
            Y = 0x31,
            Z = 0x32,
            RelativeX = 0x33,    
            RelativeY = 0x34,
            RelativeZ = 0x35,
            Slider = 0x36,
            Dial = 0x37,
            Wheel = 0x38,
            HatSwitch = 0x39,
            CountedBuffer = 0x3A,
            ByteCount = 0x3B,
            MotionWakeup = 0x3C,
            VX = 0x40,
            VY = 0x41,
            VZ = 0x42,
            VBRX = 0x43,
            VBRY = 0x44,
            VBRZ = 0x45,
            VNO = 0x46,
            SystemControlPower = 0x81,
            SystemControlSleep = 0x82,
            SystemControlWake = 0x83,
            SystemControlContextMenu = 0x84,
            SystemControlMainMenu = 0x85,
            SystemControlApplicationMenu = 0x86,
            SystemControlHelpMenu = 0x87,
            SystemControlMenuExit = 0x88,
            SystemControlMenuSelect = 0x89,
            SystemControlMenuRight = 0x8A,
            SystemControlMenuLeft = 0x8B,
            SystemControlMenuUp = 0x8C,
            SystemControlMenuDown = 0x8D,
            KeyboardNoEvent = 0x00,
            KeyboardRollover = 0x01,
            KeyboardPostFail = 0x02,
            KeyboardUndefined = 0x03,
            KeyboardaA = 0x04,
            KeyboardzZ = 0x1D,
            Keyboard1 = 0x1E,
            Keyboard0 = 0x27,
            KeyboardLeftControl = 0xE0,
            KeyboardLeftShift = 0xE1,
            KeyboardLeftALT = 0xE2,
            KeyboardLeftGUI = 0xE3,
            KeyboardRightControl = 0xE4,
            KeyboardRightShift = 0xE5,
            KeyboardRightALT = 0xE6,
            KeyboardRightGUI = 0xE7,
            KeyboardScrollLock = 0x47,
            KeyboardNumLock = 0x53,
            KeyboardCapsLock = 0x39,
            KeyboardF1 = 0x3A,
            KeyboardF12 = 0x45,
            KeyboardReturn = 0x28,
            KeyboardEscape = 0x29,
            KeyboardDelete = 0x2A,
            KeyboardPrintScreen = 0x46,
            LEDNumLock = 0x01,
            LEDCapsLock = 0x02,
            LEDScrollLock = 0x03,
            LEDCompose = 0x04,
            LEDKana = 0x05,
            LEDPower = 0x06,
            LEDShift = 0x07,
            LEDDoNotDisturb = 0x08,
            LEDMute = 0x09,
            LEDToneEnable = 0x0A,
            LEDHighCutFilter = 0x0B,
            LEDLowCutFilter = 0x0C,
            LEDEqualizerEnable = 0x0D,
            LEDSoundFieldOn = 0x0E,
            LEDSurroundFieldOn = 0x0F,
            LEDRepeat = 0x10,
            LEDStereo = 0x11,
            LEDSamplingRateDirect = 0x12,
            LEDSpinning = 0x13,
            LEDCAV = 0x14,
            LEDCLV = 0x15,
            LEDRecordingFormatDet = 0x16,
            LEDOffHook = 0x17,
            LEDRing = 0x18,
            LEDMessageWaiting = 0x19,
            LEDDataMode = 0x1A,
            LEDBatteryOperation = 0x1B,
            LEDBatteryOK = 0x1C,
            LEDBatteryLow = 0x1D,
            LEDSpeaker = 0x1E,
            LEDHeadset = 0x1F,
            LEDHold = 0x20,
            LEDMicrophone = 0x21,
            LEDCoverage = 0x22,
            LEDNightMode = 0x23,
            LEDSendCalls = 0x24,
            LEDCallPickup = 0x25,
            LEDConference = 0x26,
            LEDStandBy = 0x27,
            LEDCameraOn = 0x28,
            LEDCameraOff = 0x29,    
            LEDOnLine = 0x2A,
            LEDOffLine = 0x2B,
            LEDBusy = 0x2C,
            LEDReady = 0x2D,
            LEDPaperOut = 0x2E,
            LEDPaperJam = 0x2F,
            LEDRemote = 0x30,
            LEDForward = 0x31,
            LEDReverse = 0x32,
            LEDStop = 0x33,
            LEDRewind = 0x34,
            LEDFastForward = 0x35,
            LEDPlay = 0x36,
            LEDPause = 0x37,
            LEDRecord = 0x38,
            LEDError = 0x39,
            LEDSelectedIndicator = 0x3A,
            LEDInUseIndicator = 0x3B,
            LEDMultiModeIndicator = 0x3C,
            LEDIndicatorOn = 0x3D,
            LEDIndicatorFlash = 0x3E,
            LEDIndicatorSlowBlink = 0x3F,
            LEDIndicatorFastBlink = 0x40,
            LEDIndicatorOff = 0x41,
            LEDFlashOnTime = 0x42,
            LEDSlowBlinkOnTime = 0x43,
            LEDSlowBlinkOffTime = 0x44,
            LEDFastBlinkOnTime = 0x45,
            LEDFastBlinkOffTime = 0x46,
            LEDIndicatorColor = 0x47,
            LEDRed = 0x48,
            LEDGreen = 0x49,
            LEDAmber = 0x4A,
            LEDGenericIndicator = 0x3B,
            TelephonyPhone = 0x01,
            TelephonyAnsweringMachine = 0x02,
            TelephonyMessageControls = 0x03,
            TelephonyHandset = 0x04,
            TelephonyHeadset = 0x05,
            TelephonyKeypad = 0x06,
            TelephonyProgrammableButton = 0x07,
            SimulationRudder = 0xBA,
            SimulationThrottle = 0xBB
        }

        /// <summary>
        /// Enumeration containing flags for a raw input device.
        /// </summary>
        [Flags()]
        public enum RawInputDeviceFlags {
            /// <summary>No flags.</summary>
            None = 0,
            /// <summary>If set, this removes the top level collection from the inclusion list. This tells the operating system to stop reading from a device which matches the top level collection.</summary>
            Remove = 0x00000001,
            /// <summary>If set, this specifies the top level collections to exclude when reading a complete usage page. This flag only affects a TLC whose usage page is already specified with PageOnly.</summary>
            Exclude = 0x00000010,
            /// <summary>If set, this specifies all devices whose top level collection is from the specified usUsagePage. Note that Usage must be zero. To exclude a particular top level collection, use Exclude.</summary>
            PageOnly = 0x00000020,
            /// <summary>If set, this prevents any devices specified by UsagePage or Usage from generating legacy messages. This is only for the mouse and keyboard.</summary>
            NoLegacy = 0x00000030,
            /// <summary>If set, this enables the caller to receive the input even when the caller is not in the foreground. Note that WindowHandle must be specified.</summary>
            InputSink = 0x00000100,
            /// <summary>If set, the mouse button click does not activate the other window.</summary>
            CaptureMouse = 0x00000200,
            /// <summary>If set, the application-defined keyboard device hotkeys are not handled. However, the system hotkeys; for example, ALT+TAB and CTRL+ALT+DEL, are still handled. By default, all keyboard hotkeys are handled. NoHotKeys can be specified even if NoLegacy is not specified and WindowHandle is NULL.</summary>
            NoHotKeys = 0x00000200,
            /// <summary>If set, application keys are handled.  NoLegacy must be specified.  Keyboard only.</summary>
            AppKeys = 0x00000400
        }

        /// <summary>
        /// Value type for raw input devices.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct RAWINPUTDEVICE {
            /// <summary>Top level collection Usage page for the raw input device.</summary>
            public HIDUsagePage UsagePage;
            /// <summary>Top level collection Usage for the raw input device. </summary>
            public HIDUsage Usage;
            /// <summary>Mode flag that specifies how to interpret the information provided by UsagePage and Usage.</summary>
            public RawInputDeviceFlags Flags;
            /// <summary>Handle to the target device. If NULL, it follows the keyboard focus.</summary>
            public IntPtr WindowHandle;
        }

        /// <summary>Function to register a raw input device.</summary>
        /// <param name="pRawInputDevices">Array of raw input devices.</param>
        /// <param name="uiNumDevices">Number of devices.</param>
        /// <param name="cbSize">Size of the RAWINPUTDEVICE structure.</param>
        /// <returns>TRUE if successful, FALSE if not.</returns>
        [DllImport("user32.dll")]
        public static extern bool RegisterRawInputDevices([MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] RAWINPUTDEVICE[] pRawInputDevices, int uiNumDevices, int cbSize);

        /// <summary>
        /// Enumeration contanining the command types to issue.
        /// </summary>
        public enum RawInputCommand {
            /// <summary>
            /// Get input data.
            /// </summary>
            Input = 0x10000003,
            /// <summary>
            /// Get header data.
            /// </summary>
            Header = 0x10000005,
            PreparsedData = 0x20000005,
            DeviceName = 0x20000007,
            DeviceInfo = 0x2000000b
        }

        public enum RawInputType : int {
            RIM_TYPE_MOUSE,
            RIM_TYPE_KEYBOARD,
            RIM_TYPE_HID
        }

        /// <summary>
        /// Value type for a raw input header.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct RawInputHeader {
            /// <summary>Type of device the input is coming from.</summary>
            public RawInputType Type;
            /// <summary>Size of the packet of data.</summary>
            public int Size;
            /// <summary>Handle to the device sending the data.</summary>
            public IntPtr Device;
            /// <summary>wParam from the window message.</summary>
            public IntPtr wParam;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RawKeyboard {
            public RawInputHeader Header;
            public ushort MakeCode;
            public RawKeyboardFlags Flags;
            ushort Reserved;
            public ushort VKey;
            public uint Message;
            public uint ExtraInformation;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RawHID {
            public RawInputHeader Header;
            public uint dwSizeHid;
            public uint dwCount;
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType=UnmanagedType.LPArray, SizeConst=256)]
            public byte[] bRawData;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RawMouse {
            public RawInputHeader Header;
            public ushort Flags;
            public ushort ButtonData;
            public ushort ButtonFlags;
            public uint RawButtons;
            public int LastX;
            public int LastY;
            public uint ExtraInformation;
        }

        /// <summary>
        /// Function to retrieve raw input data.
        /// </summary>
        /// <param name="hRawInput">Handle to the raw input.</param>
        /// <param name="uiCommand">Command to issue when retrieving data.</param>
        /// <param name="pData">Raw input data.</param>
        /// <param name="pcbSize">Number of bytes in the array.</param>
        /// <param name="cbSizeHeader">Size of the header.</param>
        /// <returns>0 if successful if pData is null, otherwise number of bytes if pData is not null.</returns>
        [DllImport("user32.dll")]
        static extern bool GetRawInputData(IntPtr hRawInput, RawInputCommand uiCommand, out RawHID pData, out int pcbSize, int cbSizeHeader);

        public static bool GetRawInputData(IntPtr hRawInput, out RawHID pData)
            => GetRawInputData(hRawInput, RawInputCommand.Input, out pData, out _, Marshal.SizeOf(typeof(RawInputHeader)))
                && GetRawInputData(hRawInput, RawInputCommand.Input, out pData, out _, Marshal.SizeOf(typeof(RawInputHeader)));

        [DllImport("user32.dll")]
        static extern int GetRawInputDeviceInfo(IntPtr hDevice, RawInputCommand uiCommand, byte[] pData, out int pcbSize);

        public static bool GetRawInputDeviceInfo(IntPtr hDevice, RawInputCommand uiCommand, out byte[] pData) {
            pData = null;
            if (GetRawInputDeviceInfo(hDevice, uiCommand, null, out int size) != 0) return false;
            
            pData = new byte[size];
            return GetRawInputDeviceInfo(hDevice, uiCommand, pData, out size) > 0;
        }

        public static readonly ushort KEYBOARD_OVERRUN_MAKE_CODE = 0xFF;

        [Flags()]
        public enum RawKeyboardFlags: ushort {
            RI_KEY_MAKE = 0,
            RI_KEY_BREAK = 1,
            RI_KEY_E0 = 2,
            RI_KEY_E1 = 4
        }

        [DllImport("user32.dll")]
        public static extern uint MapVirtualKey(uint uCode, uint uMapType);

        public static readonly uint MAPVK_VSC_TO_VK_EX = 3;

        [StructLayout(LayoutKind.Sequential)]
        public struct HIDP_CAPS {
            [MarshalAs(UnmanagedType.U2)]
            public HIDUsage Usage;
            [MarshalAs(UnmanagedType.U2)]
            public HIDUsagePage UsagePage;
            [MarshalAs(UnmanagedType.U2)]
            public UInt16 InputReportByteLength;
            [MarshalAs(UnmanagedType.U2)]
            public UInt16 OutputReportByteLength;
            [MarshalAs(UnmanagedType.U2)]
            public UInt16 FeatureReportByteLength;
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType=UnmanagedType.LPArray, SizeConst=17)]
            UInt16[] Reserved;
            [MarshalAs(UnmanagedType.U2)]
            public UInt16 NumberLinkCollectionNodes;
            [MarshalAs(UnmanagedType.U2)]
            public UInt16 NumberInputButtonCaps;
            [MarshalAs(UnmanagedType.U2)]
            public UInt16 NumberInputValueCaps;
            [MarshalAs(UnmanagedType.U2)]
            public UInt16 NumberInputDataIndices;
            [MarshalAs(UnmanagedType.U2)]
            public UInt16 NumberOutputButtonCaps;
            [MarshalAs(UnmanagedType.U2)]
            public UInt16 NumberOutputValueCaps;
            [MarshalAs(UnmanagedType.U2)]
            public UInt16 NumberOutputDataIndices;
            [MarshalAs(UnmanagedType.U2)]
            public UInt16 NumberFeatureButtonCaps;
            [MarshalAs(UnmanagedType.U2)]
            public UInt16 NumberFeatureValueCaps;
            [MarshalAs(UnmanagedType.U2)]
            public UInt16 NumberFeatureDataIndices;
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct HIDP_BUTTON_CAPS {
            [MarshalAs(UnmanagedType.U2)]
            public HIDUsagePage UsagePage;
            [MarshalAs(UnmanagedType.U1)]
            public byte ReportID;
            [MarshalAs(UnmanagedType.U1)]
            public bool IsAlias;
            [MarshalAs(UnmanagedType.U2)]
            public UInt16 BitField;
            [MarshalAs(UnmanagedType.U2)]
            public UInt16 LinkCollection;
            [MarshalAs(UnmanagedType.U2)]
            public UInt16 LinkUsage;
            [MarshalAs(UnmanagedType.U2)]
            public UInt16 LinkUsagePage;
            [MarshalAs(UnmanagedType.U1)]
            public bool IsRange;
            [MarshalAs(UnmanagedType.U1)]
            public bool IsStringRange;
            [MarshalAs(UnmanagedType.U1)]
            public bool IsDesignatorRange;
            [MarshalAs(UnmanagedType.U1)]
            public bool IsAbsolute;
            [MarshalAs(UnmanagedType.U2)]
            public UInt16 ReportCount;
            [MarshalAs(UnmanagedType.U2)]
            UInt16 Reserved2;
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType=UnmanagedType.LPArray, SizeConst=9)]
            UInt32[] Reserved;
            [MarshalAs(UnmanagedType.U2)]
            public UInt16 UsageMin;
            [MarshalAs(UnmanagedType.U2)]
            public UInt16 UsageMax;
            [MarshalAs(UnmanagedType.U2)]
            public UInt16 StringMin;
            [MarshalAs(UnmanagedType.U2)]
            public UInt16 StringMax;
            [MarshalAs(UnmanagedType.U2)]
            public UInt16 DesignatorMin;
            [MarshalAs(UnmanagedType.U2)]
            public UInt16 DesignatorMax;
            [MarshalAs(UnmanagedType.U2)]
            public UInt16 DataIndexMin;
            [MarshalAs(UnmanagedType.U2)]
            public UInt16 DataIndexMax;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct HIDP_VALUE_CAPS {
            [MarshalAs(UnmanagedType.U2)]
            public HIDUsagePage UsagePage;
            [MarshalAs(UnmanagedType.U1)]
            public byte ReportID;
            [MarshalAs(UnmanagedType.U1)]
            public bool IsAlias;
            [MarshalAs(UnmanagedType.U2)]
            public UInt16 BitField;
            [MarshalAs(UnmanagedType.U2)]
            public UInt16 LinkCollection;
            [MarshalAs(UnmanagedType.U2)]
            public UInt16 LinkUsage;
            [MarshalAs(UnmanagedType.U2)]
            public UInt16 LinkUsagePage;
            [MarshalAs(UnmanagedType.U1)]
            public bool IsRange;
            [MarshalAs(UnmanagedType.U1)]
            public bool IsStringRange;
            [MarshalAs(UnmanagedType.U1)]
            public bool IsDesignatorRange;
            [MarshalAs(UnmanagedType.U1)]
            public bool IsAbsolute;
            [MarshalAs(UnmanagedType.U1)]
            public bool HasNull;
            [MarshalAs(UnmanagedType.U1)]
            byte Reserved;
            [MarshalAs(UnmanagedType.U2)]
            public UInt16 BitSize;
            [MarshalAs(UnmanagedType.U2)]
            public UInt16 ReportCount;
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.LPArray, SizeConst = 5)]
            UInt16[] Reserved2;
            [MarshalAs(UnmanagedType.U4)]
            public UInt32 UnitsExp;
            [MarshalAs(UnmanagedType.U4)]
            public UInt32 Units;
            [MarshalAs(UnmanagedType.U4)]
            public Int32 LogicalMin;
            [MarshalAs(UnmanagedType.U4)]
            public Int32 LogicalMax;
            [MarshalAs(UnmanagedType.U4)]
            public Int32 PhysicalMin;
            [MarshalAs(UnmanagedType.U4)]
            public Int32 PhysicalMax;
            [MarshalAs(UnmanagedType.U2)]
            public HIDUsage UsageMin;
            [MarshalAs(UnmanagedType.U2)]
            public UInt16 UsageMax;
            [MarshalAs(UnmanagedType.U2)]
            public UInt16 StringMin;
            [MarshalAs(UnmanagedType.U2)]
            public UInt16 StringMax;
            [MarshalAs(UnmanagedType.U2)]
            public UInt16 DesignatorMin;
            [MarshalAs(UnmanagedType.U2)]
            public UInt16 DesignatorMax;
            [MarshalAs(UnmanagedType.U2)]
            public UInt16 DataIndexMin;
            [MarshalAs(UnmanagedType.U2)]
            public UInt16 DataIndexMax;
        }

        public enum NTSTATUS: uint {
            HIDP_STATUS_SUCCESS = 0x00110000,
            HIDP_INVALID_REPORT_LENGTH = 0xC0110003,
            HIDP_INVALID_REPORT_TYPE = 0xC0110002,
            HIDP_STATUS_BUFFER_TOO_SMALL = 0xC0110007,
            HIDP_STATUS_INCOMPATIBLE_REPORT_ID = 0xC011000A,
            HIDP_STATUS_INVALID_PREPARSED_DATA = 0xC0110001,
            HIDP_STATUS_USAGE_NOT_FOUND = 0xC0110004
        }

        [DllImport("hid.dll", SetLastError = true)]
        public static extern NTSTATUS HidP_GetCaps(
            byte[] PreparsedData,
            out HIDP_CAPS Capabilities
        );

        public enum HIDP_REPORT_TYPE {
            HidP_Input,
            HidP_Output,
            HidP_Feature
        }

        [DllImport("hid.dll", SetLastError = true)]
        public static extern NTSTATUS HidP_GetButtonCaps(
            HIDP_REPORT_TYPE ReportType,
            [In, Out] HIDP_BUTTON_CAPS[] ButtonCaps,
            ref ushort ButtonCapsLength,
            byte[] PreparsedData
        );

        [DllImport("hid.dll", SetLastError = true)]
        public static extern NTSTATUS HidP_GetValueCaps(
            HIDP_REPORT_TYPE ReportType,
            [In, Out] HIDP_VALUE_CAPS[] ValueCaps,
            ref ushort ValueCapsLength,
            byte[] PreparsedData
        );

        [DllImport("hid.dll", SetLastError = true)]
        public static extern NTSTATUS HidP_GetUsages(
            HIDP_REPORT_TYPE ReportType,
            HIDUsagePage UsagePage,
            ushort LinkCollection,
            [In, Out] ushort[] UsageList,
            ref uint UsageLength,
            byte[] PreparsedData,
            byte[] Report,
            uint ReportLength
        );

        [DllImport("hid.dll", SetLastError = true)]
        public static extern NTSTATUS HidP_GetUsageValue(
            HIDP_REPORT_TYPE ReportType,
            HIDUsagePage UsagePage,
            ushort LinkCollection,
            HIDUsage Usage,
            out int UsageValue,
            byte[] PreparsedData,
            byte[] Report,
            uint ReportLength
        );

        [DllImport("hid.dll", SetLastError = true)]
        public static extern bool HidD_FreePreparsedData(byte[] PreparsedData);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr CreateFile(
            string lpFileName,
            uint dwDesiredAccess,
            uint dwShareMode,
            IntPtr lpSecurityAttributes,
            uint dwCreationDisposition,
            uint dwFlagsAndAttributes,
            IntPtr hTemplateFile
        );

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool CloseHandle(IntPtr hObject);

        [DllImport("hid.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool HidD_GetProductString(IntPtr hFile, StringBuilder buffer, int bufferLength);

        [StructLayout(LayoutKind.Sequential)]
        public struct DEVPROPKEY {
            public Guid fmtid;
            public uint pid;
        }

        public static DEVPROPKEY DEVPKEY_Device_InstanceId = new DEVPROPKEY() {
            fmtid = new Guid(0x78c34fc8, 0x104a, 0x4aca, 0x9e, 0xa4, 0x52, 0x4d, 0x52, 0x99, 0x6e, 0x57),
            pid = 256
        };

        public static DEVPROPKEY DEVPKEY_Device_BusTypeGuid = new DEVPROPKEY() {
            fmtid = new Guid(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0),
            pid = 21
        };

        public static DEVPROPKEY DEVPKEY_NAME = new DEVPROPKEY() {
            fmtid = new Guid(0xb725f130, 0x47ef, 0x101a, 0xa5, 0xf1, 0x02, 0x60, 0x8c, 0x9e, 0xeb, 0xac),
            pid = 10
        };

        public static readonly uint DEVPROP_TYPE_STRING = 0x12;
        public static readonly uint DEVPROP_TYPE_GUID = 0x0D;

        public static readonly int CR_SUCCESS = 0x00;
        public static readonly int CR_BUFFER_SMALL = 0x1A;

        [DllImport("CfgMgr32.dll", CharSet = CharSet.Unicode)]
        public static extern int CM_Get_Device_Interface_Property(
            string pszDeviceInterface,
            ref DEVPROPKEY PropertyKey,
            out uint PropertyType,
            StringBuilder PropertyBuffer,
            ref int PropertyBufferSize,
            uint ulFlags
        );

        [Flags()]
        public enum Locate_DevNode_Flags {
            CM_LOCATE_DEVNODE_NORMAL = 0,
            CM_LOCATE_DEVNODE_PHANTOM = 1,
            CM_LOCATE_DEVNODE_CANCELREMOVE = 2,
            CM_LOCATE_DEVNODE_NOVALIDATION = 4
        }

        [DllImport("CfgMgr32.dll", CharSet = CharSet.Unicode)]
        public static extern int CM_Locate_DevNode(
            out int pdnDevInst,
            string pDeviceID,
            Locate_DevNode_Flags ulFlags
        );

        [DllImport("CfgMgr32.dll", CharSet = CharSet.Unicode)]
        public static extern int CM_Get_DevNode_Property(
            int dnDevInst,
            ref DEVPROPKEY PropertyKey,
            out uint PropertyType,
            StringBuilder PropertyBuffer,
            ref int PropertyBufferSize,
            uint ulFlags
        );

        [DllImport("CfgMgr32.dll", CharSet = CharSet.Unicode)]
        public static extern int CM_Get_DevNode_Property(
            int dnDevInst,
            ref DEVPROPKEY PropertyKey,
            out uint PropertyType,
            ref Guid PropertyBuffer,
            ref int PropertyBufferSize,
            uint ulFlags
        );

        public static Guid GUID_BUS_TYPE_HID = new Guid(0xeeaf37d0, 0x1963, 0x47c4, 0xaa, 0x48, 0x72, 0x47, 0x6d, 0xb7, 0xcf, 0x49);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct WNDCLASSEX {
            [MarshalAs(UnmanagedType.U4)]
            public int cbSize;
            [MarshalAs(UnmanagedType.U4)]
            public int style;
            public IntPtr lpfnWndProc;
            public int cbClsExtra;
            public int cbWndExtra;
            public IntPtr hInstance;
            public IntPtr hIcon;
            public IntPtr hCursor;
            public IntPtr hbrBackground;
            public string lpszMenuName;
            public string lpszClassName;
            public IntPtr hIconSm;

            public static WNDCLASSEX Build() {
                var nw = new WNDCLASSEX();
                nw.cbSize = Marshal.SizeOf(typeof(WNDCLASSEX));
                return nw;
            }
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.U2)]
        public static extern short RegisterClassEx([In] ref WNDCLASSEX lpwcx);

        public delegate IntPtr WndProc(IntPtr hWnd, uint msg, UIntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern IntPtr DefWindowProc(IntPtr hWnd, uint msg, UIntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Ansi)]
        public static extern IntPtr CreateWindowEx(uint dwExStyle, string lpClassName, string lpWindowName, uint dwStyle, int x, int y, int nWidth, int nHeight, IntPtr hWndParent, IntPtr hMenu, IntPtr hInstance, IntPtr lpParam);

        public const int CW_USEDEFAULT = unchecked((int)0x80000000);

        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT {
            public int X;
            public int Y;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MSG {
            public IntPtr hwnd;
            public uint message;
            public UIntPtr wParam;
            public IntPtr lParam;
            public int time;
            public POINT pt;
            public int lPrivate;
        }

        public const int WM_INPUT = 0x00FF;

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern bool PeekMessage(out Message msg, IntPtr hWnd, uint messageFilterMin, uint messageFilterMax, uint flags);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern bool GetMessage(out MSG msg, IntPtr hWnd, uint messageFilterMin, uint messageFilterMax);

        [DllImport("user32.dll")]
        public static extern bool TranslateMessage([In] ref MSG msg);

        [DllImport("user32.dll")]
        public static extern bool DispatchMessage([In] ref MSG msg);
    }
}
