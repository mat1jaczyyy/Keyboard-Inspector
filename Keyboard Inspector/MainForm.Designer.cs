
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
            this.screen = new System.Windows.Forms.PictureBox();
            this.scroll = new System.Windows.Forms.HScrollBar();
            this.keymenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.color = new System.Windows.Forms.ToolStripMenuItem();
            this.hide = new System.Windows.Forms.ToolStripMenuItem();
            this.mainmenu = new System.Windows.Forms.MenuStrip();
            this.recording = new System.Windows.Forms.ToolStripMenuItem();
            this.open = new System.Windows.Forms.ToolStripMenuItem();
            this.save = new System.Windows.Forms.ToolStripMenuItem();
            this.key = new System.Windows.Forms.ToolStripMenuItem();
            this.freeze = new System.Windows.Forms.ToolStripMenuItem();
            this.unhide = new System.Windows.Forms.ToolStripMenuItem();
            this.poll = new System.Windows.Forms.ToolStripMenuItem();
            this.analyze = new System.Windows.Forms.ToolStripMenuItem();
            this.export = new System.Windows.Forms.ToolStripMenuItem();
            this.status = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.screen)).BeginInit();
            this.keymenu.SuspendLayout();
            this.mainmenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // rec
            // 
            this.rec.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rec.Location = new System.Drawing.Point(704, 0);
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
            // screen
            // 
            this.screen.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.screen.BackColor = System.Drawing.Color.White;
            this.screen.Location = new System.Drawing.Point(0, 24);
            this.screen.Name = "screen";
            this.screen.Size = new System.Drawing.Size(827, 137);
            this.screen.TabIndex = 2;
            this.screen.TabStop = false;
            this.screen.DragDrop += new System.Windows.Forms.DragEventHandler(this.screen_DragDrop);
            this.screen.DragOver += new System.Windows.Forms.DragEventHandler(this.screen_DragOver);
            this.screen.MouseClick += new System.Windows.Forms.MouseEventHandler(this.screen_MouseClick);
            this.screen.MouseDown += new System.Windows.Forms.MouseEventHandler(this.screen_MouseDown);
            this.screen.MouseMove += new System.Windows.Forms.MouseEventHandler(this.screen_MouseMove);
            this.screen.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.screen_MouseWheel);
            // 
            // scroll
            // 
            this.scroll.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.scroll.LargeChange = 1000000000;
            this.scroll.Location = new System.Drawing.Point(0, 161);
            this.scroll.Maximum = 1000000000;
            this.scroll.Name = "scroll";
            this.scroll.Size = new System.Drawing.Size(827, 17);
            this.scroll.TabIndex = 3;
            this.scroll.Scroll += new System.Windows.Forms.ScrollEventHandler(this.scroll_Scroll);
            // 
            // keymenu
            // 
            this.keymenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.color,
            this.hide});
            this.keymenu.Name = "keymenu";
            this.keymenu.Size = new System.Drawing.Size(157, 48);
            // 
            // color
            // 
            this.color.Name = "color";
            this.color.Size = new System.Drawing.Size(156, 22);
            this.color.Text = "Change &Color...";
            this.color.Click += new System.EventHandler(this.color_Click);
            // 
            // hide
            // 
            this.hide.Name = "hide";
            this.hide.Size = new System.Drawing.Size(156, 22);
            this.hide.Text = "&Hide Key";
            this.hide.Click += new System.EventHandler(this.hide_Click);
            // 
            // mainmenu
            // 
            this.mainmenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.recording,
            this.key,
            this.poll});
            this.mainmenu.Location = new System.Drawing.Point(0, 0);
            this.mainmenu.Name = "mainmenu";
            this.mainmenu.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.mainmenu.Size = new System.Drawing.Size(827, 24);
            this.mainmenu.TabIndex = 7;
            this.mainmenu.Text = "menuStrip1";
            // 
            // recording
            // 
            this.recording.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.open,
            this.save});
            this.recording.Name = "recording";
            this.recording.Size = new System.Drawing.Size(73, 20);
            this.recording.Text = "&Recording";
            // 
            // open
            // 
            this.open.Name = "open";
            this.open.ShortcutKeyDisplayString = "";
            this.open.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.open.Size = new System.Drawing.Size(180, 22);
            this.open.Text = "&Open...";
            this.open.Click += new System.EventHandler(this.open_Click);
            // 
            // save
            // 
            this.save.Name = "save";
            this.save.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.save.Size = new System.Drawing.Size(180, 22);
            this.save.Text = "&Save...";
            this.save.Click += new System.EventHandler(this.save_Click);
            // 
            // key
            // 
            this.key.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.freeze,
            this.unhide});
            this.key.Name = "key";
            this.key.Size = new System.Drawing.Size(95, 20);
            this.key.Text = "&Key Collection";
            // 
            // freeze
            // 
            this.freeze.CheckOnClick = true;
            this.freeze.Name = "freeze";
            this.freeze.Size = new System.Drawing.Size(134, 22);
            this.freeze.Text = "&Freeze Keys";
            // 
            // unhide
            // 
            this.unhide.Name = "unhide";
            this.unhide.Size = new System.Drawing.Size(134, 22);
            this.unhide.Text = "&Unhide All";
            this.unhide.Click += new System.EventHandler(this.unhide_Click);
            // 
            // poll
            // 
            this.poll.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.analyze,
            this.export});
            this.poll.Enabled = false;
            this.poll.Name = "poll";
            this.poll.Size = new System.Drawing.Size(82, 20);
            this.poll.Text = "&Polling Rate";
            // 
            // analyze
            // 
            this.analyze.Name = "analyze";
            this.analyze.Size = new System.Drawing.Size(155, 22);
            this.analyze.Text = "&Analyze";
            this.analyze.Click += new System.EventHandler(this.analyze_Click);
            // 
            // export
            // 
            this.export.Name = "export";
            this.export.Size = new System.Drawing.Size(155, 22);
            this.export.Text = "&Export Details...";
            this.export.Click += new System.EventHandler(this.analyze_Click);
            // 
            // status
            // 
            this.status.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.status.BackColor = System.Drawing.Color.Transparent;
            this.status.Location = new System.Drawing.Point(258, 5);
            this.status.Name = "status";
            this.status.Size = new System.Drawing.Size(440, 17);
            this.status.TabIndex = 1;
            this.status.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // MainForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(827, 178);
            this.Controls.Add(this.rec);
            this.Controls.Add(this.scroll);
            this.Controls.Add(this.screen);
            this.Controls.Add(this.status);
            this.Controls.Add(this.mainmenu);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.mainmenu;
            this.MinimumSize = new System.Drawing.Size(700, 150);
            this.Name = "MainForm";
            this.Text = "Keyboard Inspector";
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.screen)).EndInit();
            this.keymenu.ResumeLayout(false);
            this.mainmenu.ResumeLayout(false);
            this.mainmenu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button rec;
        private System.Windows.Forms.Timer t;
        private System.Windows.Forms.PictureBox screen;
        private System.Windows.Forms.HScrollBar scroll;
        private System.Windows.Forms.ContextMenuStrip keymenu;
        private System.Windows.Forms.ToolStripMenuItem color;
        private System.Windows.Forms.ToolStripMenuItem hide;
        private System.Windows.Forms.MenuStrip mainmenu;
        private System.Windows.Forms.ToolStripMenuItem recording;
        private System.Windows.Forms.ToolStripMenuItem key;
        private System.Windows.Forms.ToolStripMenuItem freeze;
        private System.Windows.Forms.ToolStripMenuItem unhide;
        private System.Windows.Forms.ToolStripMenuItem poll;
        private System.Windows.Forms.ToolStripMenuItem analyze;
        private System.Windows.Forms.ToolStripMenuItem open;
        private System.Windows.Forms.ToolStripMenuItem save;
        private System.Windows.Forms.ToolStripMenuItem export;
        private System.Windows.Forms.Label status;
    }
}