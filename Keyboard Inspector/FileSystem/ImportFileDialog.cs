using System;
using System.Windows.Forms;

using DarkUI.Forms;

namespace Keyboard_Inspector {
    partial class ImportFileDialog: DarkForm {
        public Uri URL { get; private set; } = null;
        public FileFormat Format { get; private set; } = null;

        bool HasURL => URL != null && Format != null;

        public ImportFileDialog() {
            InitializeComponent();
            input.TextChanged += URLChanged;
        }

        void FormShown(object sender, EventArgs e) {
            if (!ClipboardCheck())
                URLChanged(sender, e);
        }

        bool ClipboardCheck() {
            if (!Clipboard.ContainsText()) return false;

            string url = Clipboard.GetText();
            if (!Check(url)) return false;

            input.TextChanged -= URLChanged;
            input.Text = url;
            input.TextChanged += URLChanged;

            return true;
        }

        void FormActivated(object sender, EventArgs e) {
            if (HasURL) return;
            ClipboardCheck();
        }

        bool Check(string url) {
            import.Enabled = false;
            detected.Text = "";

            URL = null;
            Format = null;

            var format = FileSystem.FindFormat(url, out Uri filtered);
            if (format == null) return false;

            import.Enabled = true;
            detected.Text = format.Name;

            URL = filtered;
            Format = format;

            return true;
        }

        void Import(object sender, EventArgs e) {
            if (HasURL)
                DialogResult = DialogResult.OK;
        }

        void URLChanged(object sender, EventArgs e)
            => Check(input.Text);

        void KeyPressed(object sender, KeyPressEventArgs e) {
            if (e.KeyChar == (char)Keys.Return) {
                import.Focus();
                Import(sender, e);
            }
        }
    }
}
