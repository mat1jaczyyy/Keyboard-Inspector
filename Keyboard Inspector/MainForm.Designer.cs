﻿
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Title title1 = new System.Windows.Forms.DataVisualization.Charting.Title();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Title title2 = new System.Windows.Forms.DataVisualization.Charting.Title();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Title title3 = new System.Windows.Forms.DataVisualization.Charting.Title();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea4 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series4 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Title title4 = new System.Windows.Forms.DataVisualization.Charting.Title();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea5 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series5 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Title title5 = new System.Windows.Forms.DataVisualization.Charting.Title();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea6 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series6 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Title title6 = new System.Windows.Forms.DataVisualization.Charting.Title();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.rec = new DarkUI.Controls.DarkButton();
            this.t = new System.Windows.Forms.Timer(this.components);
            this.screen = new System.Windows.Forms.PictureBox();
            this.scroll = new DarkUI.Controls.DarkScrollBar();
            this.keymenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.color = new System.Windows.Forms.ToolStripMenuItem();
            this.hide = new System.Windows.Forms.ToolStripMenuItem();
            this.status = new DarkUI.Controls.DarkLabel();
            this.split = new System.Windows.Forms.SplitContainer();
            this.tlp = new System.Windows.Forms.TableLayoutPanel();
            this.chartDiffs = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chartCompound = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chartCircular = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chartCircularFreq = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chartCompoundFreq = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chartDiffsFreq = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.fitter = new System.Windows.Forms.TableLayoutPanel();
            this.label3 = new DarkUI.Controls.DarkLabel();
            this.label2 = new DarkUI.Controls.DarkLabel();
            this.label1 = new DarkUI.Controls.DarkLabel();
            this.label5 = new DarkUI.Controls.DarkLabel();
            this.label6 = new DarkUI.Controls.DarkLabel();
            this.label7 = new DarkUI.Controls.DarkLabel();
            this.label8 = new DarkUI.Controls.DarkLabel();
            this.fitterCustomHz = new DarkUI.Controls.DarkTextBox();
            this.label4 = new DarkUI.Controls.DarkLabel();
            this.label9 = new DarkUI.Controls.DarkLabel();
            this.label10 = new DarkUI.Controls.DarkLabel();
            this.label11 = new DarkUI.Controls.DarkLabel();
            this.label12 = new DarkUI.Controls.DarkLabel();
            this.label13 = new DarkUI.Controls.DarkLabel();
            this.label14 = new DarkUI.Controls.DarkLabel();
            this.label15 = new DarkUI.Controls.DarkLabel();
            this.label16 = new DarkUI.Controls.DarkLabel();
            this.label17 = new DarkUI.Controls.DarkLabel();
            this.label18 = new DarkUI.Controls.DarkLabel();
            this.label19 = new DarkUI.Controls.DarkLabel();
            this.label20 = new DarkUI.Controls.DarkLabel();
            this.precisionPanel = new System.Windows.Forms.Panel();
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
            this.recording = new System.Windows.Forms.ToolStripMenuItem();
            this.open = new System.Windows.Forms.ToolStripMenuItem();
            this.save = new System.Windows.Forms.ToolStripMenuItem();
            this.key = new System.Windows.Forms.ToolStripMenuItem();
            this.freeze = new System.Windows.Forms.ToolStripMenuItem();
            this.unhide = new System.Windows.Forms.ToolStripMenuItem();
            this.mainmenu = new DarkUI.Controls.DarkMenuStrip();
            ((System.ComponentModel.ISupportInitialize)(this.screen)).BeginInit();
            this.keymenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.split)).BeginInit();
            this.split.Panel1.SuspendLayout();
            this.split.Panel2.SuspendLayout();
            this.split.SuspendLayout();
            this.tlp.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartDiffs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartCompound)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartCircular)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartCircularFreq)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartCompoundFreq)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartDiffsFreq)).BeginInit();
            this.fitter.SuspendLayout();
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
            // screen
            // 
            this.screen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.screen.Location = new System.Drawing.Point(0, 0);
            this.screen.Name = "screen";
            this.screen.Size = new System.Drawing.Size(1294, 205);
            this.screen.TabIndex = 2;
            this.screen.TabStop = false;
            this.screen.SizeChanged += new System.EventHandler(this.screen_SizeChanged);
            this.screen.DragDrop += new System.Windows.Forms.DragEventHandler(this.screen_DragDrop);
            this.screen.DragOver += new System.Windows.Forms.DragEventHandler(this.screen_DragOver);
            this.screen.MouseClick += new System.Windows.Forms.MouseEventHandler(this.screen_MouseClick);
            this.screen.MouseDown += new System.Windows.Forms.MouseEventHandler(this.screen_MouseDown);
            this.screen.MouseMove += new System.Windows.Forms.MouseEventHandler(this.screen_MouseMove);
            this.screen.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.screen_MouseWheel);
            // 
            // scroll
            // 
            this.scroll.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.scroll.Location = new System.Drawing.Point(0, 205);
            this.scroll.Maximum = 1000000000;
            this.scroll.Name = "scroll";
            this.scroll.ScrollOrientation = DarkUI.Controls.DarkScrollOrientation.Horizontal;
            this.scroll.Size = new System.Drawing.Size(1294, 17);
            this.scroll.TabIndex = 3;
            this.scroll.ValueChanged += new System.EventHandler<DarkUI.Controls.ScrollValueEventArgs>(this.scroll_Scroll);
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
            this.split.Panel1.Controls.Add(this.tlp);
            this.split.Panel1MinSize = 400;
            // 
            // split.Panel2
            // 
            this.split.Panel2.Controls.Add(this.screen);
            this.split.Panel2.Controls.Add(this.scroll);
            this.split.Panel2MinSize = 125;
            this.split.Size = new System.Drawing.Size(1294, 717);
            this.split.SplitterDistance = 491;
            this.split.TabIndex = 8;
            // 
            // tlp
            // 
            this.tlp.ColumnCount = 3;
            this.tlp.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tlp.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tlp.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tlp.Controls.Add(this.chartDiffs, 0, 0);
            this.tlp.Controls.Add(this.chartCompound, 1, 0);
            this.tlp.Controls.Add(this.chartCircular, 2, 0);
            this.tlp.Controls.Add(this.chartCircularFreq, 2, 1);
            this.tlp.Controls.Add(this.chartCompoundFreq, 1, 1);
            this.tlp.Controls.Add(this.chartDiffsFreq, 0, 1);
            this.tlp.Controls.Add(this.fitter, 2, 2);
            this.tlp.Controls.Add(this.precisionPanel, 0, 2);
            this.tlp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlp.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.FixedSize;
            this.tlp.Location = new System.Drawing.Point(0, 0);
            this.tlp.Name = "tlp";
            this.tlp.RowCount = 3;
            this.tlp.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlp.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlp.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 75F));
            this.tlp.Size = new System.Drawing.Size(1294, 491);
            this.tlp.TabIndex = 0;
            this.tlp.Visible = false;
            // 
            // chartDiffs
            // 
            this.chartDiffs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chartDiffs.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            chartArea1.AxisX.Interval = 8D;
            chartArea1.AxisX.LabelStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            chartArea1.AxisX.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            chartArea1.AxisX.MajorGrid.Interval = 8D;
            chartArea1.AxisX.MajorGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            chartArea1.AxisX.MajorTickMark.Interval = 8D;
            chartArea1.AxisX.MajorTickMark.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            chartArea1.AxisX.Maximum = 100D;
            chartArea1.AxisX.Minimum = 0D;
            chartArea1.AxisX.MinorGrid.Enabled = true;
            chartArea1.AxisX.MinorGrid.Interval = 1D;
            chartArea1.AxisX.MinorGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            chartArea1.AxisY.LabelStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            chartArea1.AxisY.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            chartArea1.AxisY.MajorGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            chartArea1.AxisY.MajorTickMark.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            chartArea1.AxisY.Minimum = 0D;
            chartArea1.AxisY.MinorGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            chartArea1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            chartArea1.Name = "area";
            this.chartDiffs.ChartAreas.Add(chartArea1);
            this.chartDiffs.Location = new System.Drawing.Point(3, 3);
            this.chartDiffs.Name = "chartDiffs";
            series1.ChartArea = "area";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series1.Name = "series";
            this.chartDiffs.Series.Add(series1);
            this.chartDiffs.Size = new System.Drawing.Size(425, 202);
            this.chartDiffs.TabIndex = 1;
            title1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            title1.Name = "Title1";
            this.chartDiffs.Titles.Add(title1);
            this.chartDiffs.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.chart_MouseWheel);
            // 
            // chartCompound
            // 
            this.chartCompound.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chartCompound.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            chartArea2.AxisX.Interval = 8D;
            chartArea2.AxisX.LabelStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            chartArea2.AxisX.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            chartArea2.AxisX.MajorGrid.Interval = 8D;
            chartArea2.AxisX.MajorGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            chartArea2.AxisX.MajorTickMark.Interval = 8D;
            chartArea2.AxisX.MajorTickMark.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            chartArea2.AxisX.Maximum = 100D;
            chartArea2.AxisX.Minimum = 0D;
            chartArea2.AxisX.MinorGrid.Enabled = true;
            chartArea2.AxisX.MinorGrid.Interval = 1D;
            chartArea2.AxisX.MinorGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            chartArea2.AxisY.LabelStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            chartArea2.AxisY.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            chartArea2.AxisY.MajorGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            chartArea2.AxisY.MajorTickMark.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            chartArea2.AxisY.Minimum = 0D;
            chartArea2.AxisY.MinorGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            chartArea2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            chartArea2.Name = "area";
            this.chartCompound.ChartAreas.Add(chartArea2);
            this.chartCompound.Location = new System.Drawing.Point(434, 3);
            this.chartCompound.Name = "chartCompound";
            series2.ChartArea = "area";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series2.Name = "series";
            this.chartCompound.Series.Add(series2);
            this.chartCompound.Size = new System.Drawing.Size(425, 202);
            this.chartCompound.TabIndex = 1;
            title2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            title2.Name = "Title1";
            this.chartCompound.Titles.Add(title2);
            this.chartCompound.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.chart_MouseWheel);
            // 
            // chartCircular
            // 
            this.chartCircular.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chartCircular.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            chartArea3.AxisX.Interval = 8D;
            chartArea3.AxisX.LabelStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            chartArea3.AxisX.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            chartArea3.AxisX.MajorGrid.Interval = 8D;
            chartArea3.AxisX.MajorGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            chartArea3.AxisX.MajorTickMark.Interval = 8D;
            chartArea3.AxisX.MajorTickMark.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            chartArea3.AxisX.Maximum = 100D;
            chartArea3.AxisX.Minimum = 0D;
            chartArea3.AxisX.MinorGrid.Enabled = true;
            chartArea3.AxisX.MinorGrid.Interval = 1D;
            chartArea3.AxisX.MinorGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            chartArea3.AxisY.LabelStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            chartArea3.AxisY.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            chartArea3.AxisY.MajorGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            chartArea3.AxisY.MajorTickMark.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            chartArea3.AxisY.Minimum = 0D;
            chartArea3.AxisY.MinorGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            chartArea3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            chartArea3.Name = "area";
            this.chartCircular.ChartAreas.Add(chartArea3);
            this.chartCircular.Location = new System.Drawing.Point(865, 3);
            this.chartCircular.Name = "chartCircular";
            series3.ChartArea = "area";
            series3.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series3.Name = "series";
            this.chartCircular.Series.Add(series3);
            this.chartCircular.Size = new System.Drawing.Size(426, 202);
            this.chartCircular.TabIndex = 1;
            title3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            title3.Name = "Title1";
            this.chartCircular.Titles.Add(title3);
            this.chartCircular.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.chart_MouseWheel);
            // 
            // chartCircularFreq
            // 
            this.chartCircularFreq.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chartCircularFreq.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            chartArea4.AxisX.Interval = 50D;
            chartArea4.AxisX.LabelStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            chartArea4.AxisX.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            chartArea4.AxisX.MajorGrid.Interval = 50D;
            chartArea4.AxisX.MajorGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            chartArea4.AxisX.MajorTickMark.Interval = 50D;
            chartArea4.AxisX.MajorTickMark.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            chartArea4.AxisX.Maximum = 500D;
            chartArea4.AxisX.Minimum = 0D;
            chartArea4.AxisX.MinorGrid.Enabled = true;
            chartArea4.AxisX.MinorGrid.Interval = 10D;
            chartArea4.AxisX.MinorGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            chartArea4.AxisY.LabelStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            chartArea4.AxisY.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            chartArea4.AxisY.MajorGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            chartArea4.AxisY.MajorTickMark.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            chartArea4.AxisY.Maximum = 1D;
            chartArea4.AxisY.Minimum = 0D;
            chartArea4.AxisY.MinorGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            chartArea4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            chartArea4.Name = "area";
            this.chartCircularFreq.ChartAreas.Add(chartArea4);
            this.chartCircularFreq.Location = new System.Drawing.Point(865, 211);
            this.chartCircularFreq.Name = "chartCircularFreq";
            this.chartCircularFreq.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.EarthTones;
            series4.ChartArea = "area";
            series4.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series4.Name = "series";
            this.chartCircularFreq.Series.Add(series4);
            this.chartCircularFreq.Size = new System.Drawing.Size(426, 202);
            this.chartCircularFreq.TabIndex = 1;
            title4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            title4.Name = "Title1";
            this.chartCircularFreq.Titles.Add(title4);
            this.chartCircularFreq.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.chart_MouseWheel);
            // 
            // chartCompoundFreq
            // 
            this.chartCompoundFreq.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chartCompoundFreq.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            chartArea5.AxisX.Interval = 50D;
            chartArea5.AxisX.LabelStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            chartArea5.AxisX.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            chartArea5.AxisX.MajorGrid.Interval = 50D;
            chartArea5.AxisX.MajorGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            chartArea5.AxisX.MajorTickMark.Interval = 50D;
            chartArea5.AxisX.MajorTickMark.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            chartArea5.AxisX.Maximum = 500D;
            chartArea5.AxisX.Minimum = 0D;
            chartArea5.AxisX.MinorGrid.Enabled = true;
            chartArea5.AxisX.MinorGrid.Interval = 10D;
            chartArea5.AxisX.MinorGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            chartArea5.AxisY.LabelStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            chartArea5.AxisY.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            chartArea5.AxisY.MajorGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            chartArea5.AxisY.MajorTickMark.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            chartArea5.AxisY.Maximum = 1D;
            chartArea5.AxisY.Minimum = 0D;
            chartArea5.AxisY.MinorGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            chartArea5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            chartArea5.Name = "area";
            this.chartCompoundFreq.ChartAreas.Add(chartArea5);
            this.chartCompoundFreq.Location = new System.Drawing.Point(434, 211);
            this.chartCompoundFreq.Name = "chartCompoundFreq";
            this.chartCompoundFreq.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.EarthTones;
            series5.ChartArea = "area";
            series5.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series5.Name = "series";
            this.chartCompoundFreq.Series.Add(series5);
            this.chartCompoundFreq.Size = new System.Drawing.Size(425, 202);
            this.chartCompoundFreq.TabIndex = 1;
            title5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            title5.Name = "Title1";
            this.chartCompoundFreq.Titles.Add(title5);
            this.chartCompoundFreq.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.chart_MouseWheel);
            // 
            // chartDiffsFreq
            // 
            this.chartDiffsFreq.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chartDiffsFreq.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            chartArea6.AxisX.Interval = 50D;
            chartArea6.AxisX.LabelStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            chartArea6.AxisX.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            chartArea6.AxisX.MajorGrid.Interval = 50D;
            chartArea6.AxisX.MajorGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            chartArea6.AxisX.MajorTickMark.Interval = 50D;
            chartArea6.AxisX.MajorTickMark.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            chartArea6.AxisX.Maximum = 500D;
            chartArea6.AxisX.Minimum = 0D;
            chartArea6.AxisX.MinorGrid.Enabled = true;
            chartArea6.AxisX.MinorGrid.Interval = 10D;
            chartArea6.AxisX.MinorGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            chartArea6.AxisY.LabelStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            chartArea6.AxisY.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            chartArea6.AxisY.MajorGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            chartArea6.AxisY.MajorTickMark.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            chartArea6.AxisY.Maximum = 1D;
            chartArea6.AxisY.Minimum = 0D;
            chartArea6.AxisY.MinorGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            chartArea6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            chartArea6.Name = "area";
            this.chartDiffsFreq.ChartAreas.Add(chartArea6);
            this.chartDiffsFreq.Location = new System.Drawing.Point(3, 211);
            this.chartDiffsFreq.Name = "chartDiffsFreq";
            this.chartDiffsFreq.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.EarthTones;
            series6.ChartArea = "area";
            series6.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series6.Name = "series";
            this.chartDiffsFreq.Series.Add(series6);
            this.chartDiffsFreq.Size = new System.Drawing.Size(425, 202);
            this.chartDiffsFreq.TabIndex = 1;
            title6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            title6.Name = "Title1";
            this.chartDiffsFreq.Titles.Add(title6);
            this.chartDiffsFreq.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.chart_MouseWheel);
            // 
            // fitter
            // 
            this.fitter.ColumnCount = 7;
            this.fitter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.fitter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.fitter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.fitter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.fitter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.fitter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.fitter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.fitter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.fitter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.fitter.Controls.Add(this.label3, 0, 1);
            this.fitter.Controls.Add(this.label2, 0, 2);
            this.fitter.Controls.Add(this.label1, 0, 0);
            this.fitter.Controls.Add(this.label5, 2, 0);
            this.fitter.Controls.Add(this.label6, 3, 0);
            this.fitter.Controls.Add(this.label7, 4, 0);
            this.fitter.Controls.Add(this.label8, 5, 0);
            this.fitter.Controls.Add(this.fitterCustomHz, 6, 0);
            this.fitter.Controls.Add(this.label4, 1, 0);
            this.fitter.Controls.Add(this.label9, 1, 1);
            this.fitter.Controls.Add(this.label10, 2, 1);
            this.fitter.Controls.Add(this.label11, 3, 1);
            this.fitter.Controls.Add(this.label12, 4, 1);
            this.fitter.Controls.Add(this.label13, 5, 1);
            this.fitter.Controls.Add(this.label14, 6, 1);
            this.fitter.Controls.Add(this.label15, 6, 2);
            this.fitter.Controls.Add(this.label16, 5, 2);
            this.fitter.Controls.Add(this.label17, 4, 2);
            this.fitter.Controls.Add(this.label18, 3, 2);
            this.fitter.Controls.Add(this.label19, 2, 2);
            this.fitter.Controls.Add(this.label20, 1, 2);
            this.fitter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fitter.Location = new System.Drawing.Point(865, 419);
            this.fitter.Name = "fitter";
            this.fitter.RowCount = 3;
            this.fitter.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.fitter.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.fitter.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.fitter.Size = new System.Drawing.Size(426, 69);
            this.fitter.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.label3.Location = new System.Drawing.Point(3, 23);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(54, 23);
            this.label3.TabIndex = 2;
            this.label3.Text = "stddev";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.label2.Location = new System.Drawing.Point(3, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 23);
            this.label2.TabIndex = 1;
            this.label2.Text = "amount";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "Hz";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label5
            // 
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.label5.Location = new System.Drawing.Point(123, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(54, 23);
            this.label5.TabIndex = 3;
            this.label5.Text = "125";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label6
            // 
            this.label6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.label6.Location = new System.Drawing.Point(183, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(54, 23);
            this.label6.TabIndex = 3;
            this.label6.Text = "250";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label7
            // 
            this.label7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.label7.Location = new System.Drawing.Point(243, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(54, 23);
            this.label7.TabIndex = 3;
            this.label7.Text = "500";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label8
            // 
            this.label8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.label8.Location = new System.Drawing.Point(303, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(54, 23);
            this.label8.TabIndex = 3;
            this.label8.Text = "1000";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // fitterCustomHz
            // 
            this.fitterCustomHz.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.fitterCustomHz.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.fitterCustomHz.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fitterCustomHz.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.fitterCustomHz.Location = new System.Drawing.Point(363, 3);
            this.fitterCustomHz.Name = "fitterCustomHz";
            this.fitterCustomHz.Size = new System.Drawing.Size(60, 20);
            this.fitterCustomHz.TabIndex = 4;
            this.fitterCustomHz.Text = "2000";
            this.fitterCustomHz.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.fitterCustomHz.TextChanged += new System.EventHandler(this.fitterCustomHz_TextChanged);
            // 
            // label4
            // 
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.label4.Location = new System.Drawing.Point(63, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(54, 23);
            this.label4.TabIndex = 3;
            this.label4.Text = "62.5";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label9
            // 
            this.label9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.label9.Location = new System.Drawing.Point(63, 23);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(54, 23);
            this.label9.TabIndex = 3;
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label10
            // 
            this.label10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.label10.Location = new System.Drawing.Point(123, 23);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(54, 23);
            this.label10.TabIndex = 3;
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label11
            // 
            this.label11.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.label11.Location = new System.Drawing.Point(183, 23);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(54, 23);
            this.label11.TabIndex = 3;
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label12
            // 
            this.label12.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.label12.Location = new System.Drawing.Point(243, 23);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(54, 23);
            this.label12.TabIndex = 3;
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label13
            // 
            this.label13.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.label13.Location = new System.Drawing.Point(303, 23);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(54, 23);
            this.label13.TabIndex = 3;
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label14
            // 
            this.label14.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.label14.Location = new System.Drawing.Point(363, 23);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(60, 23);
            this.label14.TabIndex = 3;
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label15
            // 
            this.label15.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.label15.Location = new System.Drawing.Point(363, 46);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(60, 23);
            this.label15.TabIndex = 3;
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label16
            // 
            this.label16.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.label16.Location = new System.Drawing.Point(303, 46);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(54, 23);
            this.label16.TabIndex = 3;
            this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label17
            // 
            this.label17.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label17.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.label17.Location = new System.Drawing.Point(243, 46);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(54, 23);
            this.label17.TabIndex = 3;
            this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label18
            // 
            this.label18.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label18.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label18.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.label18.Location = new System.Drawing.Point(183, 46);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(54, 23);
            this.label18.TabIndex = 3;
            this.label18.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label19
            // 
            this.label19.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label19.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label19.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.label19.Location = new System.Drawing.Point(123, 46);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(54, 23);
            this.label19.TabIndex = 3;
            this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label20
            // 
            this.label20.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label20.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label20.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.label20.Location = new System.Drawing.Point(63, 46);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(54, 23);
            this.label20.TabIndex = 3;
            this.label20.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // precisionPanel
            // 
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
            this.precisionPanel.Location = new System.Drawing.Point(3, 419);
            this.precisionPanel.Name = "precisionPanel";
            this.precisionPanel.Size = new System.Drawing.Size(425, 69);
            this.precisionPanel.TabIndex = 5;
            // 
            // label25
            // 
            this.label25.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label25.AutoSize = true;
            this.label25.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.label25.Location = new System.Drawing.Point(234, 51);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(49, 13);
            this.label25.TabIndex = 12;
            this.label25.Text = "iterations";
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
            this.tbPrecision.MaxLength = 5;
            this.tbPrecision.Name = "tbPrecision";
            this.tbPrecision.Size = new System.Drawing.Size(56, 20);
            this.tbPrecision.TabIndex = 7;
            this.tbPrecision.Text = "4000";
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
            // recording
            // 
            this.recording.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.recording.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.open,
            this.save});
            this.recording.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.recording.Name = "recording";
            this.recording.Size = new System.Drawing.Size(73, 20);
            this.recording.Text = "&Recording";
            // 
            // open
            // 
            this.open.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.open.Name = "open";
            this.open.ShortcutKeyDisplayString = "";
            this.open.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.open.Size = new System.Drawing.Size(180, 22);
            this.open.Text = "&Open...";
            this.open.Click += new System.EventHandler(this.open_Click);
            // 
            // save
            // 
            this.save.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.save.Enabled = false;
            this.save.Name = "save";
            this.save.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.save.Size = new System.Drawing.Size(180, 22);
            this.save.Text = "&Save...";
            this.save.Click += new System.EventHandler(this.save_Click);
            // 
            // key
            // 
            this.key.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.key.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.freeze,
            this.unhide});
            this.key.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.key.Name = "key";
            this.key.Size = new System.Drawing.Size(95, 20);
            this.key.Text = "&Key Collection";
            // 
            // freeze
            // 
            this.freeze.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.freeze.CheckOnClick = true;
            this.freeze.Name = "freeze";
            this.freeze.Size = new System.Drawing.Size(180, 22);
            this.freeze.Text = "&Freeze Keys";
            // 
            // unhide
            // 
            this.unhide.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.unhide.Name = "unhide";
            this.unhide.Size = new System.Drawing.Size(180, 22);
            this.unhide.Text = "&Unhide All";
            this.unhide.Click += new System.EventHandler(this.unhide_Click);
            // 
            // mainmenu
            // 
            this.mainmenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.mainmenu.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.mainmenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.recording,
            this.key});
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
            this.ClientSize = new System.Drawing.Size(1294, 741);
            this.Controls.Add(this.split);
            this.Controls.Add(this.rec);
            this.Controls.Add(this.status);
            this.Controls.Add(this.mainmenu);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.mainmenu;
            this.MinimumSize = new System.Drawing.Size(1310, 780);
            this.Name = "MainForm";
            this.Text = "Keyboard Inspector";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.screen)).EndInit();
            this.keymenu.ResumeLayout(false);
            this.split.Panel1.ResumeLayout(false);
            this.split.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.split)).EndInit();
            this.split.ResumeLayout(false);
            this.tlp.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chartDiffs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartCompound)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartCircular)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartCircularFreq)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartCompoundFreq)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartDiffsFreq)).EndInit();
            this.fitter.ResumeLayout(false);
            this.fitter.PerformLayout();
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
        private System.Windows.Forms.PictureBox screen;
        private DarkUI.Controls.DarkScrollBar scroll;
        private System.Windows.Forms.ContextMenuStrip keymenu;
        private System.Windows.Forms.ToolStripMenuItem color;
        private System.Windows.Forms.ToolStripMenuItem hide;
        private DarkUI.Controls.DarkLabel status;
        private System.Windows.Forms.SplitContainer split;
        private System.Windows.Forms.TableLayoutPanel tlp;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartDiffs;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartCompound;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartCircular;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartCircularFreq;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartCompoundFreq;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartDiffsFreq;
        private System.Windows.Forms.TableLayoutPanel fitter;
        private DarkUI.Controls.DarkLabel label1;
        private DarkUI.Controls.DarkLabel label3;
        private DarkUI.Controls.DarkLabel label2;
        private DarkUI.Controls.DarkLabel label4;
        private DarkUI.Controls.DarkLabel label5;
        private DarkUI.Controls.DarkLabel label6;
        private DarkUI.Controls.DarkLabel label7;
        private DarkUI.Controls.DarkLabel label8;
        private DarkUI.Controls.DarkTextBox fitterCustomHz;
        private DarkUI.Controls.DarkLabel label9;
        private DarkUI.Controls.DarkLabel label10;
        private DarkUI.Controls.DarkLabel label11;
        private DarkUI.Controls.DarkLabel label12;
        private DarkUI.Controls.DarkLabel label13;
        private DarkUI.Controls.DarkLabel label14;
        private DarkUI.Controls.DarkLabel label15;
        private DarkUI.Controls.DarkLabel label16;
        private DarkUI.Controls.DarkLabel label17;
        private DarkUI.Controls.DarkLabel label18;
        private DarkUI.Controls.DarkLabel label19;
        private DarkUI.Controls.DarkLabel label20;
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
        private System.Windows.Forms.ToolStripMenuItem key;
        private System.Windows.Forms.ToolStripMenuItem freeze;
        private System.Windows.Forms.ToolStripMenuItem unhide;
        private DarkUI.Controls.DarkMenuStrip mainmenu;
    }
}