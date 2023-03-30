namespace Keyboard_Inspector {
    partial class Chart {
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.ScrollBar = new DarkUI.Controls.DarkScrollBar();
            this.Area = new Keyboard_Inspector.ChartArea();
            this.SuspendLayout();
            // 
            // ScrollBar
            // 
            this.ScrollBar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ScrollBar.Location = new System.Drawing.Point(0, 133);
            this.ScrollBar.Maximum = 1000000000;
            this.ScrollBar.Name = "ScrollBar";
            this.ScrollBar.ScrollOrientation = DarkUI.Controls.DarkScrollOrientation.Horizontal;
            this.ScrollBar.Size = new System.Drawing.Size(150, 17);
            this.ScrollBar.TabIndex = 4;
            this.ScrollBar.ViewSize = 999999999;
            // 
            // Area
            // 
            this.Area.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Area.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(140)))), ((int)(((byte)(240)))));
            this.Area.Location = new System.Drawing.Point(0, 0);
            this.Area.Name = "Area";
            this.Area.Size = new System.Drawing.Size(150, 133);
            this.Area.TabIndex = 5;
            // 
            // Chart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.Area);
            this.Controls.Add(this.ScrollBar);
            this.Name = "Chart";
            this.ResumeLayout(false);

        }

        #endregion
        private DarkUI.Controls.DarkScrollBar ScrollBar;
        public ChartArea Area;
    }
}
