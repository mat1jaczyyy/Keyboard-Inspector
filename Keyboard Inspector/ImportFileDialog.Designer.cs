namespace Keyboard_Inspector {
    partial class ImportFileDialog {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImportFileDialog));
            this.input = new DarkUI.Controls.DarkTextBox();
            this.label = new DarkUI.Controls.DarkLabel();
            this.detected = new DarkUI.Controls.DarkLabel();
            this.import = new DarkUI.Controls.DarkButton();
            this.cancel = new DarkUI.Controls.DarkButton();
            this.SuspendLayout();
            // 
            // input
            // 
            this.input.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.input.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.input.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.input.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.input.Location = new System.Drawing.Point(12, 32);
            this.input.Name = "input";
            this.input.Size = new System.Drawing.Size(418, 20);
            this.input.TabIndex = 0;
            this.input.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.KeyPressed);
            // 
            // label
            // 
            this.label.AutoSize = true;
            this.label.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.label.Location = new System.Drawing.Point(9, 9);
            this.label.Name = "label";
            this.label.Size = new System.Drawing.Size(32, 13);
            this.label.TabIndex = 0;
            this.label.Text = "URL:";
            // 
            // detected
            // 
            this.detected.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.detected.Location = new System.Drawing.Point(50, 9);
            this.detected.Name = "detected";
            this.detected.Size = new System.Drawing.Size(380, 13);
            this.detected.TabIndex = 0;
            this.detected.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // import
            // 
            this.import.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.import.Enabled = false;
            this.import.Location = new System.Drawing.Point(274, 67);
            this.import.Name = "import";
            this.import.Padding = new System.Windows.Forms.Padding(5);
            this.import.Size = new System.Drawing.Size(75, 23);
            this.import.TabIndex = 1;
            this.import.Text = "Import";
            this.import.Click += new System.EventHandler(this.Import);
            // 
            // cancel
            // 
            this.cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel.Location = new System.Drawing.Point(355, 67);
            this.cancel.Name = "cancel";
            this.cancel.Padding = new System.Windows.Forms.Padding(5);
            this.cancel.Size = new System.Drawing.Size(75, 23);
            this.cancel.TabIndex = 2;
            this.cancel.Text = "Cancel";
            // 
            // ImportFileDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(442, 102);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.import);
            this.Controls.Add(this.detected);
            this.Controls.Add(this.label);
            this.Controls.Add(this.input);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ImportFileDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Import from URL";
            this.Activated += new System.EventHandler(this.FormActivated);
            this.Shown += new System.EventHandler(this.FormShown);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.KeyPressed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DarkUI.Controls.DarkTextBox input;
        private DarkUI.Controls.DarkLabel label;
        private DarkUI.Controls.DarkLabel detected;
        private DarkUI.Controls.DarkButton import;
        private DarkUI.Controls.DarkButton cancel;
    }
}