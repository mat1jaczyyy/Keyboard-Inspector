
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
            this.rec = new DarkUI.Controls.DarkButton();
            this.t = new System.Windows.Forms.Timer(this.components);
            this.status = new DarkUI.Controls.DarkLabel();
            this.split = new System.Windows.Forms.SplitContainer();
            this.tlpCharts = new System.Windows.Forms.TableLayoutPanel();
            this.chartDiffs = new Keyboard_Inspector.Chart();
            this.chartCompound = new Keyboard_Inspector.Chart();
            this.chartCircular = new Keyboard_Inspector.Chart();
            this.chartDiffsFreq = new Keyboard_Inspector.Chart();
            this.chartCompoundFreq = new Keyboard_Inspector.Chart();
            this.chartCircularFreq = new Keyboard_Inspector.Chart();
            this.tlpMain = new System.Windows.Forms.TableLayoutPanel();
            this.precisionPanel = new System.Windows.Forms.Panel();
            this.lowCut = new DarkUI.Controls.DarkCheckBox();
            this.label25 = new DarkUI.Controls.DarkLabel();
            this.label24 = new DarkUI.Controls.DarkLabel();
            this.hps = new DarkUI.Controls.DarkNumericUpDown();
            this.label23 = new DarkUI.Controls.DarkLabel();
            this.precisionHalf = new DarkUI.Controls.DarkButton();
            this.precisionDouble = new DarkUI.Controls.DarkButton();
            this.labelN = new DarkUI.Controls.DarkLabel();
            this.tbPrecision = new DarkUI.Controls.DarkTextBox();
            this.label21 = new DarkUI.Controls.DarkLabel();
            this.label22 = new DarkUI.Controls.DarkLabel();
            this.screen = new Keyboard_Inspector.Chart();
            this.recording = new System.Windows.Forms.ToolStripMenuItem();
            this.open = new System.Windows.Forms.ToolStripMenuItem();
            this.save = new System.Windows.Forms.ToolStripMenuItem();
            this.mainmenu = new DarkUI.Controls.DarkMenuStrip();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.split)).BeginInit();
            this.split.Panel1.SuspendLayout();
            this.split.Panel2.SuspendLayout();
            this.split.SuspendLayout();
            this.tlpCharts.SuspendLayout();
            this.tlpMain.SuspendLayout();
            this.precisionPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.hps)).BeginInit();
            this.mainmenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // rec
            // 
            this.rec.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rec.Location = new System.Drawing.Point(1172, 0);
            this.rec.Name = "rec";
            this.rec.Padding = new System.Windows.Forms.Padding(5);
            this.rec.Size = new System.Drawing.Size(122, 24);
            this.rec.TabIndex = 0;
            this.rec.Text = "Start Recording";
            this.rec.Click += new System.EventHandler(this.rec_Click);
            // 
            // t
            // 
            this.t.Interval = 1000;
            this.t.Tick += new System.EventHandler(this.t_Tick);
            // 
            // status
            // 
            this.status.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.status.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.status.Location = new System.Drawing.Point(249, 5);
            this.status.Name = "status";
            this.status.Size = new System.Drawing.Size(916, 17);
            this.status.TabIndex = 1;
            this.status.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // split
            // 
            this.split.Dock = System.Windows.Forms.DockStyle.Fill;
            this.split.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.split.Location = new System.Drawing.Point(0, 24);
            this.split.Name = "split";
            this.split.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // split.Panel1
            // 
            this.split.Panel1.Controls.Add(this.tlpCharts);
            this.split.Panel1.Controls.Add(this.tlpMain);
            this.split.Panel1MinSize = 400;
            // 
            // split.Panel2
            // 
            this.split.Panel2.Controls.Add(this.screen);
            this.split.Panel2MinSize = 125;
            this.split.Size = new System.Drawing.Size(1294, 787);
            this.split.SplitterDistance = 561;
            this.split.TabIndex = 8;
            this.split.Visible = false;
            // 
            // tlpCharts
            // 
            this.tlpCharts.ColumnCount = 3;
            this.tlpCharts.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tlpCharts.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tlpCharts.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tlpCharts.Controls.Add(this.chartDiffs, 0, 0);
            this.tlpCharts.Controls.Add(this.chartCompound, 1, 0);
            this.tlpCharts.Controls.Add(this.chartCircular, 2, 0);
            this.tlpCharts.Controls.Add(this.chartDiffsFreq, 0, 1);
            this.tlpCharts.Controls.Add(this.chartCompoundFreq, 1, 1);
            this.tlpCharts.Controls.Add(this.chartCircularFreq, 2, 1);
            this.tlpCharts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpCharts.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.FixedSize;
            this.tlpCharts.Location = new System.Drawing.Point(0, 0);
            this.tlpCharts.Name = "tlpCharts";
            this.tlpCharts.RowCount = 2;
            this.tlpCharts.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpCharts.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpCharts.Size = new System.Drawing.Size(1294, 486);
            this.tlpCharts.TabIndex = 0;
            // 
            // chartDiffs
            // 
            this.chartDiffs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chartDiffs.Location = new System.Drawing.Point(3, 3);
            this.chartDiffs.Name = "chartDiffs";
            this.chartDiffs.Size = new System.Drawing.Size(425, 237);
            this.chartDiffs.TabIndex = 0;
            // 
            // chartCompound
            // 
            this.chartCompound.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chartCompound.Location = new System.Drawing.Point(434, 3);
            this.chartCompound.Name = "chartCompound";
            this.chartCompound.Size = new System.Drawing.Size(425, 237);
            this.chartCompound.TabIndex = 1;
            // 
            // chartCircular
            // 
            this.chartCircular.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chartCircular.Location = new System.Drawing.Point(865, 3);
            this.chartCircular.Name = "chartCircular";
            this.chartCircular.Size = new System.Drawing.Size(426, 237);
            this.chartCircular.TabIndex = 2;
            // 
            // chartDiffsFreq
            // 
            this.chartDiffsFreq.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chartDiffsFreq.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(120)))), ((int)(((byte)(0)))));
            this.chartDiffsFreq.Location = new System.Drawing.Point(3, 246);
            this.chartDiffsFreq.Name = "chartDiffsFreq";
            this.chartDiffsFreq.Size = new System.Drawing.Size(425, 237);
            this.chartDiffsFreq.TabIndex = 3;
            // 
            // chartCompoundFreq
            // 
            this.chartCompoundFreq.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chartCompoundFreq.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(120)))), ((int)(((byte)(0)))));
            this.chartCompoundFreq.Location = new System.Drawing.Point(434, 246);
            this.chartCompoundFreq.Name = "chartCompoundFreq";
            this.chartCompoundFreq.Size = new System.Drawing.Size(425, 237);
            this.chartCompoundFreq.TabIndex = 4;
            // 
            // chartCircularFreq
            // 
            this.chartCircularFreq.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chartCircularFreq.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(120)))), ((int)(((byte)(0)))));
            this.chartCircularFreq.Location = new System.Drawing.Point(865, 246);
            this.chartCircularFreq.Name = "chartCircularFreq";
            this.chartCircularFreq.Size = new System.Drawing.Size(426, 237);
            this.chartCircularFreq.TabIndex = 5;
            // 
            // tlpMain
            // 
            this.tlpMain.ColumnCount = 3;
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tlpMain.Controls.Add(this.precisionPanel, 0, 0);
            this.tlpMain.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tlpMain.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.FixedSize;
            this.tlpMain.Location = new System.Drawing.Point(0, 486);
            this.tlpMain.Name = "tlpMain";
            this.tlpMain.RowCount = 1;
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpMain.Size = new System.Drawing.Size(1294, 75);
            this.tlpMain.TabIndex = 1;
            // 
            // precisionPanel
            // 
            this.tlpMain.SetColumnSpan(this.precisionPanel, 3);
            this.precisionPanel.Controls.Add(this.lowCut);
            this.precisionPanel.Controls.Add(this.label25);
            this.precisionPanel.Controls.Add(this.label24);
            this.precisionPanel.Controls.Add(this.hps);
            this.precisionPanel.Controls.Add(this.label23);
            this.precisionPanel.Controls.Add(this.precisionHalf);
            this.precisionPanel.Controls.Add(this.precisionDouble);
            this.precisionPanel.Controls.Add(this.labelN);
            this.precisionPanel.Controls.Add(this.tbPrecision);
            this.precisionPanel.Controls.Add(this.label21);
            this.precisionPanel.Controls.Add(this.label22);
            this.precisionPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.precisionPanel.Location = new System.Drawing.Point(3, 3);
            this.precisionPanel.Name = "precisionPanel";
            this.precisionPanel.Size = new System.Drawing.Size(1288, 69);
            this.precisionPanel.TabIndex = 5;
            // 
            // lowCut
            // 
            this.lowCut.AutoSize = true;
            this.lowCut.Checked = true;
            this.lowCut.CheckState = System.Windows.Forms.CheckState.Checked;
            this.lowCut.Location = new System.Drawing.Point(323, 26);
            this.lowCut.Name = "lowCut";
            this.lowCut.Size = new System.Drawing.Size(90, 17);
            this.lowCut.TabIndex = 13;
            this.lowCut.Text = "Low Cut Filter";
            this.toolTip1.SetToolTip(this.lowCut, resources.GetString("lowCut.ToolTip"));
            this.lowCut.UseMnemonic = false;
            this.lowCut.CheckedChanged += new System.EventHandler(this.lowCut_CheckedChanged);
            // 
            // label25
            // 
            this.label25.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label25.AutoSize = true;
            this.label25.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.label25.Location = new System.Drawing.Point(234, 51);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(40, 13);
            this.label25.TabIndex = 12;
            this.label25.Text = "partials";
            this.label25.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // label24
            // 
            this.label24.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label24.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.label24.Location = new System.Drawing.Point(33, 5);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(123, 13);
            this.label24.TabIndex = 11;
            this.label24.Text = "Event Count:";
            this.label24.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // hps
            // 
            this.hps.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.hps.Location = new System.Drawing.Point(162, 49);
            this.hps.Maximum = new decimal(new int[] {
            9,
            0,
            0,
            0});
            this.hps.Name = "hps";
            this.hps.Size = new System.Drawing.Size(71, 20);
            this.hps.TabIndex = 10;
            this.hps.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.hps.ValueChanged += new System.EventHandler(this.hps_ValueChanged);
            // 
            // label23
            // 
            this.label23.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label23.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.label23.Location = new System.Drawing.Point(33, 51);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(123, 13);
            this.label23.TabIndex = 9;
            this.label23.Text = "HPS Partial Elimination:";
            this.label23.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // precisionHalf
            // 
            this.precisionHalf.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.precisionHalf.Location = new System.Drawing.Point(245, 26);
            this.precisionHalf.Name = "precisionHalf";
            this.precisionHalf.Padding = new System.Windows.Forms.Padding(5, 3, 5, 5);
            this.precisionHalf.Size = new System.Drawing.Size(29, 20);
            this.precisionHalf.TabIndex = 8;
            this.precisionHalf.Tag = "";
            this.precisionHalf.Text = "/ 2";
            this.precisionHalf.Click += new System.EventHandler(this.precisionHalf_Click);
            // 
            // precisionDouble
            // 
            this.precisionDouble.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.precisionDouble.Location = new System.Drawing.Point(274, 26);
            this.precisionDouble.Name = "precisionDouble";
            this.precisionDouble.Padding = new System.Windows.Forms.Padding(5, 3, 5, 5);
            this.precisionDouble.Size = new System.Drawing.Size(29, 20);
            this.precisionDouble.TabIndex = 8;
            this.precisionDouble.Tag = "";
            this.precisionDouble.Text = "* 2";
            this.precisionDouble.Click += new System.EventHandler(this.precisionDouble_Click);
            // 
            // labelN
            // 
            this.labelN.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelN.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.labelN.Location = new System.Drawing.Point(162, 5);
            this.labelN.Name = "labelN";
            this.labelN.Size = new System.Drawing.Size(57, 13);
            this.labelN.TabIndex = 4;
            this.labelN.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbPrecision
            // 
            this.tbPrecision.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.tbPrecision.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.tbPrecision.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbPrecision.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.tbPrecision.Location = new System.Drawing.Point(162, 26);
            this.tbPrecision.MaxLength = 7;
            this.tbPrecision.Name = "tbPrecision";
            this.tbPrecision.Size = new System.Drawing.Size(56, 20);
            this.tbPrecision.TabIndex = 7;
            this.tbPrecision.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.tbPrecision.TextChanged += new System.EventHandler(this.tbPrecision_TextChanged);
            // 
            // label21
            // 
            this.label21.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label21.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.label21.Location = new System.Drawing.Point(33, 28);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(123, 13);
            this.label21.TabIndex = 4;
            this.label21.Text = "Analysis Precision:";
            this.label21.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label22
            // 
            this.label22.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label22.AutoSize = true;
            this.label22.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.label22.Location = new System.Drawing.Point(219, 28);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(20, 13);
            this.label22.TabIndex = 6;
            this.label22.Text = "Hz";
            this.label22.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // screen
            // 
            this.screen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.screen.Location = new System.Drawing.Point(0, 0);
            this.screen.Name = "screen";
            this.screen.Size = new System.Drawing.Size(1294, 222);
            this.screen.TabIndex = 1;
            // 
            // recording
            // 
            this.recording.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.recording.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.open,
            this.save});
            this.recording.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.recording.Name = "recording";
            this.recording.Size = new System.Drawing.Size(37, 20);
            this.recording.Text = "&File";
            // 
            // open
            // 
            this.open.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.open.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.open.Name = "open";
            this.open.ShortcutKeyDisplayString = "";
            this.open.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.open.Size = new System.Drawing.Size(163, 22);
            this.open.Text = "&Open...";
            this.open.Click += new System.EventHandler(this.open_Click);
            // 
            // save
            // 
            this.save.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.save.Enabled = false;
            this.save.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(153)))), ((int)(((byte)(153)))));
            this.save.Name = "save";
            this.save.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.save.Size = new System.Drawing.Size(163, 22);
            this.save.Text = "&Save As...";
            this.save.Click += new System.EventHandler(this.save_Click);
            // 
            // mainmenu
            // 
            this.mainmenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.mainmenu.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.mainmenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.recording});
            this.mainmenu.Location = new System.Drawing.Point(0, 0);
            this.mainmenu.Name = "mainmenu";
            this.mainmenu.Padding = new System.Windows.Forms.Padding(3, 2, 0, 2);
            this.mainmenu.Size = new System.Drawing.Size(1294, 24);
            this.mainmenu.TabIndex = 7;
            this.mainmenu.Text = "menuStrip1";
            // 
            // MainForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1294, 811);
            this.Controls.Add(this.split);
            this.Controls.Add(this.rec);
            this.Controls.Add(this.status);
            this.Controls.Add(this.mainmenu);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.mainmenu;
            this.MinimumSize = new System.Drawing.Size(1310, 850);
            this.Name = "MainForm";
            this.Text = "Keyboard Inspector";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.MainForm_DragDrop);
            this.DragOver += new System.Windows.Forms.DragEventHandler(this.MainForm_DragOver);
            this.split.Panel1.ResumeLayout(false);
            this.split.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.split)).EndInit();
            this.split.ResumeLayout(false);
            this.tlpCharts.ResumeLayout(false);
            this.tlpMain.ResumeLayout(false);
            this.precisionPanel.ResumeLayout(false);
            this.precisionPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.hps)).EndInit();
            this.mainmenu.ResumeLayout(false);
            this.mainmenu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private DarkUI.Controls.DarkButton rec;
        private System.Windows.Forms.Timer t;
        private DarkUI.Controls.DarkLabel status;
        private System.Windows.Forms.SplitContainer split;
        private System.Windows.Forms.TableLayoutPanel tlpCharts;
        private System.Windows.Forms.Panel precisionPanel;
        private DarkUI.Controls.DarkLabel labelN;
        private DarkUI.Controls.DarkTextBox tbPrecision;
        private DarkUI.Controls.DarkLabel label21;
        private DarkUI.Controls.DarkLabel label22;
        private DarkUI.Controls.DarkButton precisionHalf;
        private DarkUI.Controls.DarkButton precisionDouble;
        private DarkUI.Controls.DarkLabel label23;
        private DarkUI.Controls.DarkNumericUpDown hps;
        private DarkUI.Controls.DarkLabel label24;
        private DarkUI.Controls.DarkLabel label25;
        private System.Windows.Forms.ToolStripMenuItem recording;
        private System.Windows.Forms.ToolStripMenuItem open;
        private System.Windows.Forms.ToolStripMenuItem save;
        private DarkUI.Controls.DarkMenuStrip mainmenu;
        private System.Windows.Forms.TableLayoutPanel tlpMain;
        private DarkUI.Controls.DarkCheckBox lowCut;
        private System.Windows.Forms.ToolTip toolTip1;
        private Chart chartDiffs;
        private Chart chartCompound;
        private Chart chartCircular;
        private Chart chartDiffsFreq;
        private Chart chartCompoundFreq;
        private Chart chartCircularFreq;
        private Chart screen;
    }
}