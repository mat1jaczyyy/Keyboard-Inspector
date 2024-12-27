
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
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            rec = new DarkUI.Controls.DarkButton();
            status = new DarkUI.Controls.DarkLabel();
            split = new System.Windows.Forms.SplitContainer();
            tlpCharts = new System.Windows.Forms.TableLayoutPanel();
            tDiffs = new Chart();
            tCompound = new Chart();
            tCircular = new Chart();
            fDiffs = new Chart();
            fCompound = new Chart();
            fCircular = new Chart();
            analysisPanel = new System.Windows.Forms.Panel();
            labelN = new DarkUI.Controls.DarkLabel();
            partials = new DarkUI.Controls.DarkLabel();
            hps = new DarkUI.Controls.DarkNumericUpDown();
            lblHPS = new DarkUI.Controls.DarkLabel();
            lowCut = new DarkUI.Controls.DarkCheckBox();
            binRateDouble = new DarkUI.Controls.DarkButton();
            binRateHalf = new DarkUI.Controls.DarkButton();
            tbBinRate = new DarkUI.Controls.DarkTextBox();
            lblBinRate = new DarkUI.Controls.DarkLabel();
            lblHz = new DarkUI.Controls.DarkLabel();
            screen = new Chart();
            recording = new System.Windows.Forms.ToolStripMenuItem();
            open = new System.Windows.Forms.ToolStripMenuItem();
            save = new System.Windows.Forms.ToolStripMenuItem();
            sep1 = new System.Windows.Forms.ToolStripSeparator();
            import = new System.Windows.Forms.ToolStripMenuItem();
            mainmenu = new DarkUI.Controls.DarkMenuStrip();
            capture = new System.Windows.Forms.ToolStripMenuItem();
            captureKeyboard = new System.Windows.Forms.ToolStripMenuItem();
            captureGamepad = new System.Windows.Forms.ToolStripMenuItem();
            captureMouse = new System.Windows.Forms.ToolStripMenuItem();
            help = new System.Windows.Forms.ToolStripMenuItem();
            docs = new System.Windows.Forms.ToolStripMenuItem();
            updates = new System.Windows.Forms.ToolStripMenuItem();
            discord = new System.Windows.Forms.ToolStripMenuItem();
            sep2 = new System.Windows.Forms.ToolStripSeparator();
            donate = new System.Windows.Forms.ToolStripMenuItem();
            toolTip1 = new System.Windows.Forms.ToolTip(components);
            frozen = new DarkUI.Controls.DarkCheckBox();
            ((System.ComponentModel.ISupportInitialize)split).BeginInit();
            split.Panel1.SuspendLayout();
            split.Panel2.SuspendLayout();
            split.SuspendLayout();
            tlpCharts.SuspendLayout();
            analysisPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)hps).BeginInit();
            mainmenu.SuspendLayout();
            SuspendLayout();
            // 
            // rec
            // 
            rec.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            rec.Location = new System.Drawing.Point(1367, 0);
            rec.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            rec.Name = "rec";
            rec.Padding = new System.Windows.Forms.Padding(6, 6, 6, 6);
            rec.Size = new System.Drawing.Size(142, 28);
            rec.TabIndex = 2;
            rec.Click += rec_Click;
            // 
            // status
            // 
            status.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            status.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            status.Location = new System.Drawing.Point(378, 6);
            status.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            status.Name = "status";
            status.Size = new System.Drawing.Size(754, 20);
            status.TabIndex = 1;
            status.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // split
            // 
            split.Dock = System.Windows.Forms.DockStyle.Fill;
            split.Location = new System.Drawing.Point(0, 24);
            split.Margin = new System.Windows.Forms.Padding(0);
            split.Name = "split";
            split.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // split.Panel1
            // 
            split.Panel1.Controls.Add(tlpCharts);
            split.Panel1.Controls.Add(analysisPanel);
            split.Panel1.Padding = new System.Windows.Forms.Padding(0, 3, 0, 2);
            split.Panel1MinSize = 400;
            // 
            // split.Panel2
            // 
            split.Panel2.Controls.Add(screen);
            split.Panel2.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            split.Panel2MinSize = 125;
            split.Size = new System.Drawing.Size(1510, 821);
            split.SplitterDistance = 559;
            split.SplitterWidth = 5;
            split.TabIndex = 8;
            split.TabStop = false;
            split.Visible = false;
            split.Paint += split_Paint;
            split.Layout += split_Layout;
            // 
            // tlpCharts
            // 
            tlpCharts.ColumnCount = 5;
            tlpCharts.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            tlpCharts.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 4F));
            tlpCharts.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            tlpCharts.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 4F));
            tlpCharts.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            tlpCharts.Controls.Add(tDiffs, 0, 0);
            tlpCharts.Controls.Add(tCompound, 2, 0);
            tlpCharts.Controls.Add(tCircular, 4, 0);
            tlpCharts.Controls.Add(fDiffs, 0, 2);
            tlpCharts.Controls.Add(fCompound, 2, 2);
            tlpCharts.Controls.Add(fCircular, 4, 2);
            tlpCharts.Dock = System.Windows.Forms.DockStyle.Fill;
            tlpCharts.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.FixedSize;
            tlpCharts.Location = new System.Drawing.Point(0, 3);
            tlpCharts.Margin = new System.Windows.Forms.Padding(0);
            tlpCharts.Name = "tlpCharts";
            tlpCharts.RowCount = 3;
            tlpCharts.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tlpCharts.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 3F));
            tlpCharts.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tlpCharts.Size = new System.Drawing.Size(1510, 525);
            tlpCharts.TabIndex = 0;
            // 
            // tDiffs
            // 
            tDiffs.AllowDrop = true;
            tDiffs.Dock = System.Windows.Forms.DockStyle.Fill;
            tDiffs.ForeColor = System.Drawing.Color.FromArgb(65, 140, 240);
            tDiffs.Location = new System.Drawing.Point(0, 0);
            tDiffs.Margin = new System.Windows.Forms.Padding(0);
            tDiffs.Name = "tDiffs";
            tDiffs.Size = new System.Drawing.Size(500, 261);
            tDiffs.TabIndex = 0;
            tDiffs.TabStop = false;
            tDiffs.Title = "";
            // 
            // tCompound
            // 
            tCompound.AllowDrop = true;
            tCompound.Dock = System.Windows.Forms.DockStyle.Fill;
            tCompound.ForeColor = System.Drawing.Color.FromArgb(65, 140, 240);
            tCompound.Location = new System.Drawing.Point(504, 0);
            tCompound.Margin = new System.Windows.Forms.Padding(0);
            tCompound.Name = "tCompound";
            tCompound.Size = new System.Drawing.Size(500, 261);
            tCompound.TabIndex = 1;
            tCompound.TabStop = false;
            tCompound.Title = "";
            // 
            // tCircular
            // 
            tCircular.AllowDrop = true;
            tCircular.Dock = System.Windows.Forms.DockStyle.Fill;
            tCircular.ForeColor = System.Drawing.Color.FromArgb(65, 140, 240);
            tCircular.Location = new System.Drawing.Point(1008, 0);
            tCircular.Margin = new System.Windows.Forms.Padding(0);
            tCircular.Name = "tCircular";
            tCircular.Size = new System.Drawing.Size(502, 261);
            tCircular.TabIndex = 2;
            tCircular.TabStop = false;
            tCircular.Title = "";
            // 
            // fDiffs
            // 
            fDiffs.AllowDrop = true;
            fDiffs.Dock = System.Windows.Forms.DockStyle.Fill;
            fDiffs.ForeColor = System.Drawing.Color.FromArgb(240, 120, 0);
            fDiffs.Location = new System.Drawing.Point(0, 264);
            fDiffs.Margin = new System.Windows.Forms.Padding(0);
            fDiffs.Name = "fDiffs";
            fDiffs.Size = new System.Drawing.Size(500, 261);
            fDiffs.TabIndex = 3;
            fDiffs.TabStop = false;
            fDiffs.Title = "";
            // 
            // fCompound
            // 
            fCompound.AllowDrop = true;
            fCompound.Dock = System.Windows.Forms.DockStyle.Fill;
            fCompound.ForeColor = System.Drawing.Color.FromArgb(240, 120, 0);
            fCompound.Location = new System.Drawing.Point(504, 264);
            fCompound.Margin = new System.Windows.Forms.Padding(0);
            fCompound.Name = "fCompound";
            fCompound.Size = new System.Drawing.Size(500, 261);
            fCompound.TabIndex = 4;
            fCompound.TabStop = false;
            fCompound.Title = "";
            // 
            // fCircular
            // 
            fCircular.AllowDrop = true;
            fCircular.Dock = System.Windows.Forms.DockStyle.Fill;
            fCircular.ForeColor = System.Drawing.Color.FromArgb(240, 120, 0);
            fCircular.Location = new System.Drawing.Point(1008, 264);
            fCircular.Margin = new System.Windows.Forms.Padding(0);
            fCircular.Name = "fCircular";
            fCircular.Size = new System.Drawing.Size(502, 261);
            fCircular.TabIndex = 5;
            fCircular.TabStop = false;
            fCircular.Title = "";
            // 
            // analysisPanel
            // 
            analysisPanel.Controls.Add(labelN);
            analysisPanel.Controls.Add(partials);
            analysisPanel.Controls.Add(hps);
            analysisPanel.Controls.Add(lblHPS);
            analysisPanel.Controls.Add(lowCut);
            analysisPanel.Controls.Add(binRateDouble);
            analysisPanel.Controls.Add(binRateHalf);
            analysisPanel.Controls.Add(tbBinRate);
            analysisPanel.Controls.Add(lblBinRate);
            analysisPanel.Controls.Add(lblHz);
            analysisPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            analysisPanel.Location = new System.Drawing.Point(0, 528);
            analysisPanel.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            analysisPanel.Name = "analysisPanel";
            analysisPanel.Size = new System.Drawing.Size(1510, 29);
            analysisPanel.TabIndex = 5;
            // 
            // labelN
            // 
            labelN.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            labelN.ForeColor = System.Drawing.Color.FromArgb(160, 160, 160);
            labelN.Location = new System.Drawing.Point(1289, 6);
            labelN.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelN.Name = "labelN";
            labelN.Size = new System.Drawing.Size(206, 15);
            labelN.TabIndex = 4;
            labelN.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // partials
            // 
            partials.AutoSize = true;
            partials.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            partials.Location = new System.Drawing.Point(660, 6);
            partials.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            partials.Name = "partials";
            partials.Size = new System.Drawing.Size(45, 15);
            partials.TabIndex = 12;
            partials.Text = "partials";
            partials.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // hps
            // 
            hps.Location = new System.Drawing.Point(576, 3);
            hps.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            hps.Maximum = new decimal(new int[] { 9, 0, 0, 0 });
            hps.Name = "hps";
            hps.Size = new System.Drawing.Size(83, 23);
            hps.TabIndex = 7;
            hps.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            toolTip1.SetToolTip(hps, resources.GetString("hps.ToolTip"));
            hps.ValueChanged += hps_ValueChanged;
            // 
            // lblHPS
            // 
            lblHPS.AutoSize = true;
            lblHPS.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            lblHPS.Location = new System.Drawing.Point(438, 6);
            lblHPS.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblHPS.Name = "lblHPS";
            lblHPS.Size = new System.Drawing.Size(131, 15);
            lblHPS.TabIndex = 9;
            lblHPS.Text = "HPS Partial Elimination:";
            // 
            // lowCut
            // 
            lowCut.AutoSize = true;
            lowCut.Checked = true;
            lowCut.CheckState = System.Windows.Forms.CheckState.Checked;
            lowCut.Location = new System.Drawing.Point(304, 3);
            lowCut.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            lowCut.Name = "lowCut";
            lowCut.Size = new System.Drawing.Size(99, 19);
            lowCut.TabIndex = 6;
            lowCut.Text = "Low Cut Filter";
            toolTip1.SetToolTip(lowCut, resources.GetString("lowCut.ToolTip"));
            lowCut.UseMnemonic = false;
            lowCut.CheckedChanged += lowCut_CheckedChanged;
            // 
            // binRateDouble
            // 
            binRateDouble.Location = new System.Drawing.Point(227, 3);
            binRateDouble.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            binRateDouble.Name = "binRateDouble";
            binRateDouble.Padding = new System.Windows.Forms.Padding(6, 3, 6, 6);
            binRateDouble.Size = new System.Drawing.Size(34, 23);
            binRateDouble.TabIndex = 5;
            binRateDouble.Tag = "";
            binRateDouble.Text = "* 2";
            binRateDouble.Click += binRateDouble_Click;
            // 
            // binRateHalf
            // 
            binRateHalf.Location = new System.Drawing.Point(194, 3);
            binRateHalf.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            binRateHalf.Name = "binRateHalf";
            binRateHalf.Padding = new System.Windows.Forms.Padding(6, 3, 6, 6);
            binRateHalf.Size = new System.Drawing.Size(34, 23);
            binRateHalf.TabIndex = 4;
            binRateHalf.Tag = "";
            binRateHalf.Text = "/ 2";
            binRateHalf.Click += binRateHalf_Click;
            // 
            // tbBinRate
            // 
            tbBinRate.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            tbBinRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            tbBinRate.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            tbBinRate.Location = new System.Drawing.Point(97, 3);
            tbBinRate.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tbBinRate.MaxLength = 7;
            tbBinRate.Name = "tbBinRate";
            tbBinRate.Size = new System.Drawing.Size(65, 23);
            tbBinRate.TabIndex = 3;
            tbBinRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            toolTip1.SetToolTip(tbBinRate, resources.GetString("tbBinRate.ToolTip"));
            tbBinRate.TextChanged += tbBinRate_TextChanged;
            // 
            // lblBinRate
            // 
            lblBinRate.AutoSize = true;
            lblBinRate.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            lblBinRate.Location = new System.Drawing.Point(14, 6);
            lblBinRate.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblBinRate.Name = "lblBinRate";
            lblBinRate.Size = new System.Drawing.Size(77, 15);
            lblBinRate.TabIndex = 4;
            lblBinRate.Text = "Binning Rate:";
            // 
            // lblHz
            // 
            lblHz.AutoSize = true;
            lblHz.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            lblHz.Location = new System.Drawing.Point(163, 6);
            lblHz.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblHz.Name = "lblHz";
            lblHz.Size = new System.Drawing.Size(21, 15);
            lblHz.TabIndex = 6;
            lblHz.Text = "Hz";
            lblHz.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // screen
            // 
            screen.AllowDrop = true;
            screen.Dock = System.Windows.Forms.DockStyle.Fill;
            screen.ForeColor = System.Drawing.Color.FromArgb(65, 140, 240);
            screen.Location = new System.Drawing.Point(0, 5);
            screen.Margin = new System.Windows.Forms.Padding(0);
            screen.Name = "screen";
            screen.Size = new System.Drawing.Size(1510, 252);
            screen.TabIndex = 1;
            screen.Title = "";
            // 
            // recording
            // 
            recording.BackColor = System.Drawing.Color.FromArgb(60, 63, 65);
            recording.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { open, save, sep1, import });
            recording.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            recording.Name = "recording";
            recording.Size = new System.Drawing.Size(35, 20);
            recording.Text = "&File";
            // 
            // open
            // 
            open.BackColor = System.Drawing.Color.FromArgb(60, 63, 65);
            open.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            open.Name = "open";
            open.ShortcutKeyDisplayString = "";
            open.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O;
            open.Size = new System.Drawing.Size(158, 22);
            open.Text = "&Open...";
            open.Click += open_Click;
            // 
            // save
            // 
            save.BackColor = System.Drawing.Color.FromArgb(60, 63, 65);
            save.Enabled = false;
            save.ForeColor = System.Drawing.Color.FromArgb(153, 153, 153);
            save.Name = "save";
            save.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S;
            save.Size = new System.Drawing.Size(158, 22);
            save.Text = "&Save As...";
            save.Click += save_Click;
            // 
            // sep1
            // 
            sep1.BackColor = System.Drawing.Color.FromArgb(60, 63, 65);
            sep1.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            sep1.Margin = new System.Windows.Forms.Padding(0, 0, 0, 1);
            sep1.Name = "sep1";
            sep1.Size = new System.Drawing.Size(155, 6);
            // 
            // import
            // 
            import.BackColor = System.Drawing.Color.FromArgb(60, 63, 65);
            import.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            import.Name = "import";
            import.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.O;
            import.ShowShortcutKeys = false;
            import.Size = new System.Drawing.Size(158, 22);
            import.Text = "&Import from URL...";
            import.Click += import_Click;
            // 
            // mainmenu
            // 
            mainmenu.BackColor = System.Drawing.Color.FromArgb(60, 63, 65);
            mainmenu.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            mainmenu.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            mainmenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { recording, capture, help });
            mainmenu.Location = new System.Drawing.Point(0, 0);
            mainmenu.Name = "mainmenu";
            mainmenu.Padding = new System.Windows.Forms.Padding(4, 2, 0, 2);
            mainmenu.Size = new System.Drawing.Size(1510, 24);
            mainmenu.TabIndex = 0;
            mainmenu.TabStop = true;
            mainmenu.Text = "menuStrip1";
            // 
            // capture
            // 
            capture.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            capture.BackColor = System.Drawing.Color.FromArgb(60, 63, 65);
            capture.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { captureKeyboard, captureGamepad, captureMouse });
            capture.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            capture.Margin = new System.Windows.Forms.Padding(0, 0, 240, 0);
            capture.Name = "capture";
            capture.Size = new System.Drawing.Size(56, 20);
            capture.Text = "Capture";
            // 
            // captureKeyboard
            // 
            captureKeyboard.BackColor = System.Drawing.Color.FromArgb(60, 63, 65);
            captureKeyboard.Checked = true;
            captureKeyboard.CheckOnClick = true;
            captureKeyboard.CheckState = System.Windows.Forms.CheckState.Checked;
            captureKeyboard.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            captureKeyboard.Name = "captureKeyboard";
            captureKeyboard.Size = new System.Drawing.Size(180, 22);
            captureKeyboard.Text = "Keyboard";
            // 
            // captureGamepad
            // 
            captureGamepad.BackColor = System.Drawing.Color.FromArgb(60, 63, 65);
            captureGamepad.Checked = true;
            captureGamepad.CheckOnClick = true;
            captureGamepad.CheckState = System.Windows.Forms.CheckState.Checked;
            captureGamepad.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            captureGamepad.Name = "captureGamepad";
            captureGamepad.Size = new System.Drawing.Size(180, 22);
            captureGamepad.Text = "Gamepad/Joystick";
            // 
            // captureMouse
            // 
            captureMouse.BackColor = System.Drawing.Color.FromArgb(60, 63, 65);
            captureMouse.CheckOnClick = true;
            captureMouse.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            captureMouse.Name = "captureMouse";
            captureMouse.Size = new System.Drawing.Size(180, 22);
            captureMouse.Text = "Mouse";
            // 
            // help
            // 
            help.BackColor = System.Drawing.Color.FromArgb(60, 63, 65);
            help.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { docs, updates, discord, sep2, donate });
            help.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            help.Name = "help";
            help.Size = new System.Drawing.Size(41, 20);
            help.Text = "Help";
            // 
            // docs
            // 
            docs.BackColor = System.Drawing.Color.FromArgb(60, 63, 65);
            docs.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            docs.Name = "docs";
            docs.Size = new System.Drawing.Size(181, 22);
            docs.Text = "Official &Documentation";
            docs.Click += docs_Click;
            // 
            // updates
            // 
            updates.BackColor = System.Drawing.Color.FromArgb(60, 63, 65);
            updates.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            updates.Name = "updates";
            updates.Size = new System.Drawing.Size(181, 22);
            updates.Text = "Check for &Updates";
            updates.Click += updates_Click;
            // 
            // discord
            // 
            discord.BackColor = System.Drawing.Color.FromArgb(60, 63, 65);
            discord.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            discord.Name = "discord";
            discord.Size = new System.Drawing.Size(181, 22);
            discord.Text = "Discord &Server";
            discord.Click += discord_Click;
            // 
            // sep2
            // 
            sep2.BackColor = System.Drawing.Color.FromArgb(60, 63, 65);
            sep2.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            sep2.Margin = new System.Windows.Forms.Padding(0, 0, 0, 1);
            sep2.Name = "sep2";
            sep2.Size = new System.Drawing.Size(178, 6);
            // 
            // donate
            // 
            donate.BackColor = System.Drawing.Color.FromArgb(60, 63, 65);
            donate.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            donate.Name = "donate";
            donate.Size = new System.Drawing.Size(181, 22);
            donate.Text = "Donate via &Ko-fi";
            donate.Click += donate_Click;
            // 
            // frozen
            // 
            frozen.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            frozen.Location = new System.Drawing.Point(1286, 3);
            frozen.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            frozen.Name = "frozen";
            frozen.Size = new System.Drawing.Size(65, 20);
            frozen.TabIndex = 1;
            frozen.Text = "Freeze";
            toolTip1.SetToolTip(frozen, "If checked, the currently visible inputs from the input history view will be preserved for the next recording.");
            frozen.UseMnemonic = false;
            frozen.CheckedChanged += frozen_CheckedChanged;
            // 
            // MainForm
            // 
            AllowDrop = true;
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1510, 845);
            Controls.Add(frozen);
            Controls.Add(split);
            Controls.Add(rec);
            Controls.Add(status);
            Controls.Add(mainmenu);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = mainmenu;
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            MinimumSize = new System.Drawing.Size(1526, 822);
            Name = "MainForm";
            FormClosing += MainForm_FormClosing;
            Shown += MainForm_Shown;
            DragDrop += MainForm_DragDrop;
            DragOver += MainForm_DragOver;
            split.Panel1.ResumeLayout(false);
            split.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)split).EndInit();
            split.ResumeLayout(false);
            tlpCharts.ResumeLayout(false);
            analysisPanel.ResumeLayout(false);
            analysisPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)hps).EndInit();
            mainmenu.ResumeLayout(false);
            mainmenu.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private DarkUI.Controls.DarkButton rec;
        private DarkUI.Controls.DarkLabel status;
        private System.Windows.Forms.SplitContainer split;
        private System.Windows.Forms.TableLayoutPanel tlpCharts;
        private System.Windows.Forms.Panel analysisPanel;
        private DarkUI.Controls.DarkTextBox tbBinRate;
        private DarkUI.Controls.DarkLabel lblBinRate;
        private DarkUI.Controls.DarkLabel lblHz;
        private DarkUI.Controls.DarkButton binRateHalf;
        private DarkUI.Controls.DarkButton binRateDouble;
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
        private System.Windows.Forms.ToolStripMenuItem updates;
        private System.Windows.Forms.ToolStripMenuItem docs;
        private System.Windows.Forms.ToolStripSeparator sep2;
        private System.Windows.Forms.ToolStripMenuItem donate;
        private System.Windows.Forms.ToolStripSeparator sep1;
    }
}