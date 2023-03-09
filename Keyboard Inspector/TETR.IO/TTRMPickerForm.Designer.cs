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
            this.p1 = new DarkUI.Controls.DarkLabel();
            this.p2 = new DarkUI.Controls.DarkLabel();
            this.vs = new DarkUI.Controls.DarkLabel();
            this.SuspendLayout();
            // 
            // p1
            // 
            this.p1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.p1.BackColor = System.Drawing.Color.Navy;
            this.p1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.p1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.p1.Location = new System.Drawing.Point(0, 9);
            this.p1.Name = "p1";
            this.p1.Size = new System.Drawing.Size(152, 23);
            this.p1.TabIndex = 0;
            this.p1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // p2
            // 
            this.p2.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.p2.BackColor = System.Drawing.Color.Maroon;
            this.p2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.p2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.p2.Location = new System.Drawing.Point(206, 9);
            this.p2.Name = "p2";
            this.p2.Size = new System.Drawing.Size(152, 23);
            this.p2.TabIndex = 0;
            this.p2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
            this.Controls.Add(this.p2);
            this.Controls.Add(this.p1);
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

        private DarkUI.Controls.DarkLabel p1;
        private DarkUI.Controls.DarkLabel p2;
        private DarkUI.Controls.DarkLabel vs;
    }
}