
namespace Keyboard_Inspector {
    partial class MainForm {
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.rec = new System.Windows.Forms.Button();
            this.t = new System.Windows.Forms.Timer(this.components);
            this.status = new System.Windows.Forms.Label();
            this.screen = new System.Windows.Forms.PictureBox();
            this.scroll = new System.Windows.Forms.HScrollBar();
            this.freeze = new System.Windows.Forms.CheckBox();
            this.poll = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.screen)).BeginInit();
            this.SuspendLayout();
            // 
            // rec
            // 
            this.rec.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.rec.Location = new System.Drawing.Point(693, 150);
            this.rec.Name = "rec";
            this.rec.Size = new System.Drawing.Size(122, 23);
            this.rec.TabIndex = 0;
            this.rec.Text = "Start Recording";
            this.rec.UseVisualStyleBackColor = true;
            this.rec.Click += new System.EventHandler(this.rec_Click);
            // 
            // t
            // 
            this.t.Interval = 1000;
            this.t.Tick += new System.EventHandler(this.t_Tick);
            // 
            // status
            // 
            this.status.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.status.Location = new System.Drawing.Point(274, 155);
            this.status.Name = "status";
            this.status.Size = new System.Drawing.Size(413, 18);
            this.status.TabIndex = 1;
            this.status.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // screen
            // 
            this.screen.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.screen.BackColor = System.Drawing.Color.White;
            this.screen.Location = new System.Drawing.Point(0, 0);
            this.screen.Name = "screen";
            this.screen.Size = new System.Drawing.Size(827, 127);
            this.screen.TabIndex = 2;
            this.screen.TabStop = false;
            this.screen.DragDrop += new System.Windows.Forms.DragEventHandler(this.screen_DragDrop);
            this.screen.DragOver += new System.Windows.Forms.DragEventHandler(this.screen_DragOver);
            this.screen.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.screen_MouseDoubleClick);
            this.screen.MouseDown += new System.Windows.Forms.MouseEventHandler(this.screen_MouseDown);
            this.screen.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.screen_MouseWheel);
            // 
            // scroll
            // 
            this.scroll.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.scroll.LargeChange = 1000000000;
            this.scroll.Location = new System.Drawing.Point(0, 127);
            this.scroll.Maximum = 1000000000;
            this.scroll.Name = "scroll";
            this.scroll.Size = new System.Drawing.Size(827, 17);
            this.scroll.TabIndex = 3;
            this.scroll.Scroll += new System.Windows.Forms.ScrollEventHandler(this.scroll_Scroll);
            // 
            // freeze
            // 
            this.freeze.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.freeze.AutoSize = true;
            this.freeze.Location = new System.Drawing.Point(12, 154);
            this.freeze.Name = "freeze";
            this.freeze.Size = new System.Drawing.Size(128, 17);
            this.freeze.TabIndex = 4;
            this.freeze.Text = "Freeze Key Collection";
            this.freeze.UseVisualStyleBackColor = true;
            // 
            // poll
            // 
            this.poll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.poll.Enabled = false;
            this.poll.Location = new System.Drawing.Point(146, 150);
            this.poll.Name = "poll";
            this.poll.Size = new System.Drawing.Size(122, 23);
            this.poll.TabIndex = 5;
            this.poll.Text = "Analyze Polling Rate";
            this.poll.UseVisualStyleBackColor = true;
            this.poll.Click += new System.EventHandler(this.poll_Click);
            this.poll.MouseUp += new System.Windows.Forms.MouseEventHandler(this.poll_MouseUp);
            // 
            // MainForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(827, 178);
            this.Controls.Add(this.poll);
            this.Controls.Add(this.freeze);
            this.Controls.Add(this.scroll);
            this.Controls.Add(this.screen);
            this.Controls.Add(this.status);
            this.Controls.Add(this.rec);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(600, 150);
            this.Name = "MainForm";
            this.Text = "Keyboard Inspector";
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.screen)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button rec;
        private System.Windows.Forms.Timer t;
        private System.Windows.Forms.Label status;
        private System.Windows.Forms.PictureBox screen;
        private System.Windows.Forms.HScrollBar scroll;
        private System.Windows.Forms.CheckBox freeze;
        private System.Windows.Forms.Button poll;
    }
}