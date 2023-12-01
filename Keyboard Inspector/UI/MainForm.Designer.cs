
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
            this.status = new DarkUI.Controls.DarkLabel();
            this.split = new System.Windows.Forms.SplitContainer();
            this.tlpCharts = new System.Windows.Forms.TableLayoutPanel();
            this.tDiffs = new Keyboard_Inspector.Chart();
            this.tCompound = new Keyboard_Inspector.Chart();
            this.tCircular = new Keyboard_Inspector.Chart();
            this.fDiffs = new Keyboard_Inspector.Chart();
            this.fCompound = new Keyboard_Inspector.Chart();
            this.fCircular = new Keyboard_Inspector.Chart();
            this.analysisPanel = new System.Windows.Forms.Panel();
            this.labelN = new DarkUI.Controls.DarkLabel();
            this.partials = new DarkUI.Controls.DarkLabel();
            this.hps = new DarkUI.Controls.DarkNumericUpDown();
            this.lblHPS = new DarkUI.Controls.DarkLabel();
            this.lowCut = new DarkUI.Controls.DarkCheckBox();
            this.precisionDouble = new DarkUI.Controls.DarkButton();
            this.precisionHalf = new DarkUI.Controls.DarkButton();
            this.tbPrecision = new DarkUI.Controls.DarkTextBox();
            this.lblPrecision = new DarkUI.Controls.DarkLabel();
            this.lblHz = new DarkUI.Controls.DarkLabel();
            this.screen = new Keyboard_Inspector.Chart();
            this.recording = new System.Windows.Forms.ToolStripMenuItem();
            this.open = new System.Windows.Forms.ToolStripMenuItem();
            this.save = new System.Windows.Forms.ToolStripMenuItem();
            this.import = new System.Windows.Forms.ToolStripMenuItem();
            this.mainmenu = new DarkUI.Controls.DarkMenuStrip();
            this.capture = new System.Windows.Forms.ToolStripMenuItem();
            this.captureKeyboard = new System.Windows.Forms.ToolStripMenuItem();
            this.captureGamepad = new System.Windows.Forms.ToolStripMenuItem();
            this.captureMouse = new System.Windows.Forms.ToolStripMenuItem();
            this.help = new System.Windows.Forms.ToolStripMenuItem();
            this.discord = new System.Windows.Forms.ToolStripMenuItem();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.frozen = new DarkUI.Controls.DarkCheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.split)).BeginInit();
            this.split.Panel1.SuspendLayout();
            this.split.Panel2.SuspendLayout();
            this.split.SuspendLayout();
            this.tlpCharts.SuspendLayout();
            this.analysisPanel.SuspendLayout();
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
            this.rec.TabIndex = 2;
            this.rec.Text = "Start Recording";
            this.rec.Click += new System.EventHandler(this.rec_Click);
            // 
            // status
            // 
            this.status.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.status.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.status.Location = new System.Drawing.Point(324, 5);
            this.status.Name = "status";
            this.status.Size = new System.Drawing.Size(646, 17);
            this.status.TabIndex = 1;
            this.status.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // split
            // 
            this.split.Dock = System.Windows.Forms.DockStyle.Fill;
            this.split.Location = new System.Drawing.Point(0, 24);
            this.split.Margin = new System.Windows.Forms.Padding(0);
            this.split.Name = "split";
            this.split.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // split.Panel1
            // 
            this.split.Panel1.Controls.Add(this.tlpCharts);
            this.split.Panel1.Controls.Add(this.analysisPanel);
            this.split.Panel1.Padding = new System.Windows.Forms.Padding(0, 3, 0, 2);
            this.split.Panel1MinSize = 400;
            // 
            // split.Panel2
            // 
            this.split.Panel2.Controls.Add(this.screen);
            this.split.Panel2.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.split.Panel2MinSize = 125;
            this.split.Size = new System.Drawing.Size(1294, 812);
            this.split.SplitterDistance = 553;
            this.split.TabIndex = 8;
            this.split.TabStop = false;
            this.split.Visible = false;
            this.split.Paint += new System.Windows.Forms.PaintEventHandler(this.split_Paint);
            this.split.Layout += new System.Windows.Forms.LayoutEventHandler(this.split_Layout);
            // 
            // tlpCharts
            // 
            this.tlpCharts.ColumnCount = 5;
            this.tlpCharts.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tlpCharts.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 3F));
            this.tlpCharts.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.tlpCharts.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 3F));
            this.tlpCharts.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tlpCharts.Controls.Add(this.tDiffs, 0, 0);
            this.tlpCharts.Controls.Add(this.tCompound, 2, 0);
            this.tlpCharts.Controls.Add(this.tCircular, 4, 0);
            this.tlpCharts.Controls.Add(this.fDiffs, 0, 2);
            this.tlpCharts.Controls.Add(this.fCompound, 2, 2);
            this.tlpCharts.Controls.Add(this.fCircular, 4, 2);
            this.tlpCharts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpCharts.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.FixedSize;
            this.tlpCharts.Location = new System.Drawing.Point(0, 3);
            this.tlpCharts.Margin = new System.Windows.Forms.Padding(0);
            this.tlpCharts.Name = "tlpCharts";
            this.tlpCharts.RowCount = 3;
            this.tlpCharts.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpCharts.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 3F));
            this.tlpCharts.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpCharts.Size = new System.Drawing.Size(1294, 523);
            this.tlpCharts.TabIndex = 0;
            // 
            // tDiffs
            // 
            this.tDiffs.AllowDrop = true;
            this.tDiffs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tDiffs.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(140)))), ((int)(((byte)(240)))));
            this.tDiffs.Location = new System.Drawing.Point(0, 0);
            this.tDiffs.Margin = new System.Windows.Forms.Padding(0);
            this.tDiffs.Name = "tDiffs";
            this.tDiffs.Size = new System.Drawing.Size(429, 260);
            this.tDiffs.TabIndex = 0;
            this.tDiffs.TabStop = false;
            // 
            // tCompound
            // 
            this.tCompound.AllowDrop = true;
            this.tCompound.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tCompound.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(140)))), ((int)(((byte)(240)))));
            this.tCompound.Location = new System.Drawing.Point(432, 0);
            this.tCompound.Margin = new System.Windows.Forms.Padding(0);
            this.tCompound.Name = "tCompound";
            this.tCompound.Size = new System.Drawing.Size(429, 260);
            this.tCompound.TabIndex = 1;
            this.tCompound.TabStop = false;
            // 
            // tCircular
            // 
            this.tCircular.AllowDrop = true;
            this.tCircular.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tCircular.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(140)))), ((int)(((byte)(240)))));
            this.tCircular.Location = new System.Drawing.Point(864, 0);
            this.tCircular.Margin = new System.Windows.Forms.Padding(0);
            this.tCircular.Name = "tCircular";
            this.tCircular.Size = new System.Drawing.Size(430, 260);
            this.tCircular.TabIndex = 2;
            this.tCircular.TabStop = false;
            // 
            // fDiffs
            // 
            this.fDiffs.AllowDrop = true;
            this.fDiffs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fDiffs.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(120)))), ((int)(((byte)(0)))));
            this.fDiffs.Location = new System.Drawing.Point(0, 263);
            this.fDiffs.Margin = new System.Windows.Forms.Padding(0);
            this.fDiffs.Name = "fDiffs";
            this.fDiffs.Size = new System.Drawing.Size(429, 260);
            this.fDiffs.TabIndex = 3;
            this.fDiffs.TabStop = false;
            // 
            // fCompound
            // 
            this.fCompound.AllowDrop = true;
            this.fCompound.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fCompound.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(120)))), ((int)(((byte)(0)))));
            this.fCompound.Location = new System.Drawing.Point(432, 263);
            this.fCompound.Margin = new System.Windows.Forms.Padding(0);
            this.fCompound.Name = "fCompound";
            this.fCompound.Size = new System.Drawing.Size(429, 260);
            this.fCompound.TabIndex = 4;
            this.fCompound.TabStop = false;
            // 
            // fCircular
            // 
            this.fCircular.AllowDrop = true;
            this.fCircular.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fCircular.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(120)))), ((int)(((byte)(0)))));
            this.fCircular.Location = new System.Drawing.Point(864, 263);
            this.fCircular.Margin = new System.Windows.Forms.Padding(0);
            this.fCircular.Name = "fCircular";
            this.fCircular.Size = new System.Drawing.Size(430, 260);
            this.fCircular.TabIndex = 5;
            this.fCircular.TabStop = false;
            // 
            // analysisPanel
            // 
            this.analysisPanel.Controls.Add(this.labelN);
            this.analysisPanel.Controls.Add(this.partials);
            this.analysisPanel.Controls.Add(this.hps);
            this.analysisPanel.Controls.Add(this.lblHPS);
            this.analysisPanel.Controls.Add(this.lowCut);
            this.analysisPanel.Controls.Add(this.precisionDouble);
            this.analysisPanel.Controls.Add(this.precisionHalf);
            this.analysisPanel.Controls.Add(this.tbPrecision);
            this.analysisPanel.Controls.Add(this.lblPrecision);
            this.analysisPanel.Controls.Add(this.lblHz);
            this.analysisPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.analysisPanel.Location = new System.Drawing.Point(0, 526);
            this.analysisPanel.Name = "analysisPanel";
            this.analysisPanel.Size = new System.Drawing.Size(1294, 25);
            this.analysisPanel.TabIndex = 5;
            // 
            // labelN
            // 
            this.labelN.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelN.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.labelN.Location = new System.Drawing.Point(1105, 5);
            this.labelN.Name = "labelN";
            this.labelN.Size = new System.Drawing.Size(177, 13);
            this.labelN.TabIndex = 4;
            this.labelN.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // partials
            // 
            this.partials.AutoSize = true;
            this.partials.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.partials.Location = new System.Drawing.Point(592, 5);
            this.partials.Name = "partials";
            this.partials.Size = new System.Drawing.Size(40, 13);
            this.partials.TabIndex = 12;
            this.partials.Text = "partials";
            this.partials.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // hps
            // 
            this.hps.Location = new System.Drawing.Point(520, 3);
            this.hps.Maximum = new decimal(new int[] {
            9,
            0,
            0,
            0});
            this.hps.Name = "hps";
            this.hps.Size = new System.Drawing.Size(71, 20);
            this.hps.TabIndex = 7;
            this.hps.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.hps.ValueChanged += new System.EventHandler(this.hps_ValueChanged);
            // 
            // lblHPS
            // 
            this.lblHPS.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.lblHPS.Location = new System.Drawing.Point(396, 5);
            this.lblHPS.Name = "lblHPS";
            this.lblHPS.Size = new System.Drawing.Size(123, 13);
            this.lblHPS.TabIndex = 9;
            this.lblHPS.Text = "HPS Partial Elimination:";
            this.lblHPS.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lowCut
            // 
            this.lowCut.AutoSize = true;
            this.lowCut.Checked = true;
            this.lowCut.CheckState = System.Windows.Forms.CheckState.Checked;
            this.lowCut.Location = new System.Drawing.Point(287, 3);
            this.lowCut.Name = "lowCut";
            this.lowCut.Size = new System.Drawing.Size(90, 17);
            this.lowCut.TabIndex = 6;
            this.lowCut.Text = "Low Cut Filter";
            this.toolTip1.SetToolTip(this.lowCut, resources.GetString("lowCut.ToolTip"));
            this.lowCut.UseMnemonic = false;
            this.lowCut.CheckedChanged += new System.EventHandler(this.lowCut_CheckedChanged);
            // 
            // precisionDouble
            // 
            this.precisionDouble.Location = new System.Drawing.Point(221, 3);
            this.precisionDouble.Name = "precisionDouble";
            this.precisionDouble.Padding = new System.Windows.Forms.Padding(5, 3, 5, 5);
            this.precisionDouble.Size = new System.Drawing.Size(29, 20);
            this.precisionDouble.TabIndex = 5;
            this.precisionDouble.Tag = "";
            this.precisionDouble.Text = "* 2";
            this.precisionDouble.Click += new System.EventHandler(this.precisionDouble_Click);
            // 
            // precisionHalf
            // 
            this.precisionHalf.Location = new System.Drawing.Point(192, 3);
            this.precisionHalf.Name = "precisionHalf";
            this.precisionHalf.Padding = new System.Windows.Forms.Padding(5, 3, 5, 5);
            this.precisionHalf.Size = new System.Drawing.Size(29, 20);
            this.precisionHalf.TabIndex = 4;
            this.precisionHalf.Tag = "";
            this.precisionHalf.Text = "/ 2";
            this.precisionHalf.Click += new System.EventHandler(this.precisionHalf_Click);
            // 
            // tbPrecision
            // 
            this.tbPrecision.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.tbPrecision.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbPrecision.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.tbPrecision.Location = new System.Drawing.Point(109, 3);
            this.tbPrecision.MaxLength = 7;
            this.tbPrecision.Name = "tbPrecision";
            this.tbPrecision.Size = new System.Drawing.Size(56, 20);
            this.tbPrecision.TabIndex = 3;
            this.tbPrecision.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.tbPrecision.TextChanged += new System.EventHandler(this.tbPrecision_TextChanged);
            // 
            // lblPrecision
            // 
            this.lblPrecision.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.lblPrecision.Location = new System.Drawing.Point(12, 5);
            this.lblPrecision.Name = "lblPrecision";
            this.lblPrecision.Size = new System.Drawing.Size(97, 13);
            this.lblPrecision.TabIndex = 4;
            this.lblPrecision.Text = "Analysis Precision:";
            this.lblPrecision.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblHz
            // 
            this.lblHz.AutoSize = true;
            this.lblHz.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.lblHz.Location = new System.Drawing.Point(166, 5);
            this.lblHz.Name = "lblHz";
            this.lblHz.Size = new System.Drawing.Size(20, 13);
            this.lblHz.TabIndex = 6;
            this.lblHz.Text = "Hz";
            this.lblHz.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // screen
            // 
            this.screen.AllowDrop = true;
            this.screen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.screen.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(140)))), ((int)(((byte)(240)))));
            this.screen.Location = new System.Drawing.Point(0, 4);
            this.screen.Margin = new System.Windows.Forms.Padding(0);
            this.screen.Name = "screen";
            this.screen.Size = new System.Drawing.Size(1294, 251);
            this.screen.TabIndex = 1;
            // 
            // recording
            // 
            this.recording.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.recording.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.open,
            this.save,
            this.import});
            this.recording.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.recording.Name = "recording";
            this.recording.Size = new System.Drawing.Size(35, 20);
            this.recording.Text = "&File";
            // 
            // open
            // 
            this.open.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.open.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.open.Name = "open";
            this.open.ShortcutKeyDisplayString = "";
            this.open.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.open.Size = new System.Drawing.Size(158, 22);
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
            this.save.Size = new System.Drawing.Size(158, 22);
            this.save.Text = "&Save As...";
            this.save.Click += new System.EventHandler(this.save_Click);
            // 
            // import
            // 
            this.import.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.import.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.import.Name = "import";
            this.import.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.O)));
            this.import.ShowShortcutKeys = false;
            this.import.Size = new System.Drawing.Size(158, 22);
            this.import.Text = "&Import from URL...";
            this.import.Click += new System.EventHandler(this.import_Click);
            // 
            // mainmenu
            // 
            this.mainmenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.mainmenu.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.mainmenu.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.mainmenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.recording,
            this.capture,
            this.help});
            this.mainmenu.Location = new System.Drawing.Point(0, 0);
            this.mainmenu.Name = "mainmenu";
            this.mainmenu.Padding = new System.Windows.Forms.Padding(3, 2, 0, 2);
            this.mainmenu.Size = new System.Drawing.Size(1294, 24);
            this.mainmenu.TabIndex = 0;
            this.mainmenu.TabStop = true;
            this.mainmenu.Text = "menuStrip1";
            // 
            // capture
            // 
            this.capture.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.capture.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.capture.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.captureKeyboard,
            this.captureGamepad,
            this.captureMouse});
            this.capture.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.capture.Margin = new System.Windows.Forms.Padding(0, 0, 200, 0);
            this.capture.Name = "capture";
            this.capture.Size = new System.Drawing.Size(56, 20);
            this.capture.Text = "Capture";
            // 
            // captureKeyboard
            // 
            this.captureKeyboard.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.captureKeyboard.Checked = true;
            this.captureKeyboard.CheckOnClick = true;
            this.captureKeyboard.CheckState = System.Windows.Forms.CheckState.Checked;
            this.captureKeyboard.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.captureKeyboard.Name = "captureKeyboard";
            this.captureKeyboard.Size = new System.Drawing.Size(163, 22);
            this.captureKeyboard.Text = "Keyboard";
            // 
            // captureGamepad
            // 
            this.captureGamepad.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.captureGamepad.Checked = true;
            this.captureGamepad.CheckOnClick = true;
            this.captureGamepad.CheckState = System.Windows.Forms.CheckState.Checked;
            this.captureGamepad.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.captureGamepad.Name = "captureGamepad";
            this.captureGamepad.Size = new System.Drawing.Size(163, 22);
            this.captureGamepad.Text = "Gamepad/Joystick";
            // 
            // captureMouse
            // 
            this.captureMouse.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.captureMouse.CheckOnClick = true;
            this.captureMouse.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.captureMouse.Name = "captureMouse";
            this.captureMouse.Size = new System.Drawing.Size(163, 22);
            this.captureMouse.Text = "Mouse";
            // 
            // help
            // 
            this.help.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.help.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.discord});
            this.help.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.help.Name = "help";
            this.help.Size = new System.Drawing.Size(41, 20);
            this.help.Text = "Help";
            // 
            // discord
            // 
            this.discord.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.discord.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.discord.Name = "discord";
            this.discord.Size = new System.Drawing.Size(144, 22);
            this.discord.Text = "Discord Server";
            this.discord.Click += new System.EventHandler(this.discord_Click);
            // 
            // frozen
            // 
            this.frozen.Location = new System.Drawing.Point(1102, 3);
            this.frozen.Name = "frozen";
            this.frozen.Size = new System.Drawing.Size(56, 17);
            this.frozen.TabIndex = 1;
            this.frozen.Text = "Freeze";
            this.frozen.UseMnemonic = false;
            this.frozen.CheckedChanged += new System.EventHandler(this.frozen_CheckedChanged);
            // 
            // MainForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1294, 836);
            this.Controls.Add(this.frozen);
            this.Controls.Add(this.split);
            this.Controls.Add(this.rec);
            this.Controls.Add(this.status);
            this.Controls.Add(this.mainmenu);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.mainmenu;
            this.MinimumSize = new System.Drawing.Size(1310, 875);
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
            this.analysisPanel.ResumeLayout(false);
            this.analysisPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.hps)).EndInit();
            this.mainmenu.ResumeLayout(false);
            this.mainmenu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private DarkUI.Controls.DarkButton rec;
        private DarkUI.Controls.DarkLabel status;
        private System.Windows.Forms.SplitContainer split;
        private System.Windows.Forms.TableLayoutPanel tlpCharts;
        private System.Windows.Forms.Panel analysisPanel;
        private DarkUI.Controls.DarkTextBox tbPrecision;
        private DarkUI.Controls.DarkLabel lblPrecision;
        private DarkUI.Controls.DarkLabel lblHz;
        private DarkUI.Controls.DarkButton precisionHalf;
        private DarkUI.Controls.DarkButton precisionDouble;
        private DarkUI.Controls.DarkLabel lblHPS;
        private DarkUI.Controls.DarkNumericUpDown hps;
        private DarkUI.Controls.DarkLabel partials;
        private System.Windows.Forms.ToolStripMenuItem recording;
        private System.Windows.Forms.ToolStripMenuItem open;
        private System.Windows.Forms.ToolStripMenuItem save;
        private DarkUI.Controls.DarkMenuStrip mainmenu;
        private DarkUI.Controls.DarkCheckBox lowCut;
        private System.Windows.Forms.ToolTip toolTip1;
        private Chart screen;
        public Chart tDiffs;
        public Chart tCompound;
        public Chart tCircular;
        public Chart fDiffs;
        public Chart fCompound;
        public Chart fCircular;
        private System.Windows.Forms.ToolStripMenuItem capture;
        public System.Windows.Forms.ToolStripMenuItem captureKeyboard;
        public System.Windows.Forms.ToolStripMenuItem captureGamepad;
        public System.Windows.Forms.ToolStripMenuItem captureMouse;
        private System.Windows.Forms.ToolStripMenuItem import;
        private DarkUI.Controls.DarkLabel labelN;
        private System.Windows.Forms.ToolStripMenuItem help;
        private System.Windows.Forms.ToolStripMenuItem discord;
        private DarkUI.Controls.DarkCheckBox frozen;
    }
}