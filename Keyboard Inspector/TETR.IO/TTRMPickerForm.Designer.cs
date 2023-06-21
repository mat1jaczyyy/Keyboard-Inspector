namespace Keyboard_Inspector {
    partial class TTRMPickerForm {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TTRMPickerForm));
            this.p1lbl = new DarkUI.Controls.DarkLabel();
            this.p2lbl = new DarkUI.Controls.DarkLabel();
            this.vs = new DarkUI.Controls.DarkLabel();
            this.SuspendLayout();
            // 
            // p1lbl
            // 
            this.p1lbl.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.p1lbl.BackColor = System.Drawing.Color.Navy;
            this.p1lbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.p1lbl.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.p1lbl.Location = new System.Drawing.Point(0, 9);
            this.p1lbl.Name = "p1lbl";
            this.p1lbl.Size = new System.Drawing.Size(152, 23);
            this.p1lbl.TabIndex = 0;
            this.p1lbl.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // p2lbl
            // 
            this.p2lbl.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.p2lbl.BackColor = System.Drawing.Color.Maroon;
            this.p2lbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.p2lbl.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.p2lbl.Location = new System.Drawing.Point(206, 9);
            this.p2lbl.Name = "p2lbl";
            this.p2lbl.Size = new System.Drawing.Size(152, 23);
            this.p2lbl.TabIndex = 0;
            this.p2lbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // vs
            // 
            this.vs.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.vs.ForeColor = System.Drawing.Color.Gray;
            this.vs.Location = new System.Drawing.Point(158, 9);
            this.vs.Name = "vs";
            this.vs.Size = new System.Drawing.Size(42, 23);
            this.vs.TabIndex = 0;
            this.vs.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TTRMPickerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(358, 43);
            this.Controls.Add(this.vs);
            this.Controls.Add(this.p2lbl);
            this.Controls.Add(this.p1lbl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TTRMPickerForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Select TETR.IO Replay";
            this.ResumeLayout(false);

        }

        #endregion

        private DarkUI.Controls.DarkLabel p1lbl;
        private DarkUI.Controls.DarkLabel p2lbl;
        private DarkUI.Controls.DarkLabel vs;
    }
}