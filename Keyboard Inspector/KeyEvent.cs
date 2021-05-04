using System.Windows.Forms;

namespace Keyboard_Inspector {
    public struct KeyEvent {
        const int WM_KEYDOWN = 0x0100;
        const int WM_KEYUP = 0x0101;
        /*
        const int WM_SYSKEYDOWN = 0x0104;
        const int WM_SYSKEYUP = 0x0105;
        */
        const int MASK = 0xFFFB;

        public double Timestamp;
        public bool? Pressed;
        public Keys Key;

        public KeyEvent(double ticks, int wParam, int lParam) {
            Timestamp = ticks;

            wParam &= MASK;
            Pressed =
                wParam == WM_KEYDOWN
                    ? true
                    : wParam == WM_KEYUP
                        ? false
                        : (bool?)null;

            Key = (Keys)lParam;
        }

        public override string ToString()
            => $"{Timestamp:0.00000} {(Pressed == true? "DN" : "UP")} {Key}";
    }
}
