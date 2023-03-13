using System;
using System.Windows.Forms;

using DarkUI.Controls;

namespace Keyboard_Inspector {
    internal class HTTransparentDarkLabel: DarkLabel {
        private const int WM_NCHITTEST = 0x84;
        private const int HTTRANSPARENT = -1;

        protected override void WndProc(ref Message message) {
            if (message.Msg == WM_NCHITTEST)
                message.Result = (IntPtr)HTTRANSPARENT;
            else
                base.WndProc(ref message);
        }
    }
}
