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
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.p1score = new DarkUI.Controls.DarkLabel();
            this.p2score = new DarkUI.Controls.DarkLabel();
            this.p1stats = new DarkUI.Controls.DarkLabel();
            this.p2stats = new DarkUI.Controls.DarkLabel();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // p1lbl
            // 
            this.p1lbl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.p1lbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.p1lbl.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.p1lbl.Location = new System.Drawing.Point(0, 0);
            this.p1lbl.Name = "p1lbl";
            this.p1lbl.Size = new System.Drawing.Size(270, 23);
            this.p1lbl.TabIndex = 0;
            this.p1lbl.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // p2lbl
            // 
            this.p2lbl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.p2lbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.p2lbl.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.p2lbl.Location = new System.Drawing.Point(0, 0);
            this.p2lbl.Name = "p2lbl";
            this.p2lbl.Size = new System.Drawing.Size(270, 23);
            this.p2lbl.TabIndex = 0;
            this.p2lbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // vs
            // 
            this.vs.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.vs.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.vs.ForeColor = System.Drawing.Color.Goldenrod;
            this.vs.Location = new System.Drawing.Point(301, 16);
            this.vs.Name = "vs";
            this.vs.Size = new System.Drawing.Size(42, 66);
            this.vs.TabIndex = 0;
            this.vs.Text = "VS";
            this.vs.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel1
            // 
            this.panel1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.panel1.Controls.Add(this.p1stats);
            this.panel1.Controls.Add(this.p1score);
            this.panel1.Controls.Add(this.p1lbl);
            this.panel1.Location = new System.Drawing.Point(0, 10);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(270, 72);
            this.panel1.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.panel2.Controls.Add(this.p2stats);
            this.panel2.Controls.Add(this.p2score);
            this.panel2.Controls.Add(this.p2lbl);
            this.panel2.Location = new System.Drawing.Point(374, 10);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(270, 72);
            this.panel2.TabIndex = 1;
            // 
            // p1score
            // 
            this.p1score.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.p1score.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.p1score.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.p1score.Location = new System.Drawing.Point(0, 23);
            this.p1score.Name = "p1score";
            this.p1score.Size = new System.Drawing.Size(270, 30);
            this.p1score.TabIndex = 0;
            this.p1score.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // p2score
            // 
            this.p2score.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.p2score.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.p2score.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.p2score.Location = new System.Drawing.Point(0, 23);
            this.p2score.Name = "p2score";
            this.p2score.Size = new System.Drawing.Size(270, 30);
            this.p2score.TabIndex = 1;
            this.p2score.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // p1stats
            // 
            this.p1stats.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.p1stats.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.p1stats.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.p1stats.Location = new System.Drawing.Point(0, 53);
            this.p1stats.Name = "p1stats";
            this.p1stats.Size = new System.Drawing.Size(270, 19);
            this.p1stats.TabIndex = 0;
            this.p1stats.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // p2stats
            // 
            this.p2stats.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.p2stats.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.p2stats.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.p2stats.Location = new System.Drawing.Point(0, 53);
            this.p2stats.Name = "p2stats";
            this.p2stats.Size = new System.Drawing.Size(270, 19);
            this.p2stats.TabIndex = 0;
            this.p2stats.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // TTRMPickerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(644, 95);
            this.Controls.Add(this.vs);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TTRMPickerForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Select TETR.IO Replay";
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DarkUI.Controls.DarkLabel p1lbl;
        private DarkUI.Controls.DarkLabel p2lbl;
        private DarkUI.Controls.DarkLabel vs;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private DarkUI.Controls.DarkLabel p1score;
        private DarkUI.Controls.DarkLabel p2score;
        private DarkUI.Controls.DarkLabel p1stats;
        private DarkUI.Controls.DarkLabel p2stats;
    }
}