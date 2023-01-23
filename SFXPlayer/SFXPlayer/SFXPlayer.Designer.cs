namespace SFXPlayer
{
    partial class SFXPlayer
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SFXPlayer));
            this.CueList = new System.Windows.Forms.TableLayoutPanel();
            this.dlgOpenAudioFile = new System.Windows.Forms.OpenFileDialog();
            this.rtMainText = new System.Windows.Forms.RichTextBox();
            this.bnStopAll = new System.Windows.Forms.Button();
            this.ProgressTimer = new System.Windows.Forms.Timer(this.components);
            this.bnPlayNext = new System.Windows.Forms.Button();
            this.bnAddCue = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.bnDeleteCue = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exportShowFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importShowFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.transportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.playToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stopAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.previousCueToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nextCueToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.autoLoadLastsfxCuelistToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ScrollTimer = new System.Windows.Forms.Timer(this.components);
            this.bnPrev = new System.Windows.Forms.Button();
            this.bnNext = new System.Windows.Forms.Button();
            this.rtPrevMainText = new System.Windows.Forms.RichTextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.statusBar = new System.Windows.Forms.ToolStripStatusLabel();
            this.WebLink = new System.Windows.Forms.ToolStripStatusLabel();
            this.DeviceChangeTimer = new System.Windows.Forms.Timer(this.components);
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.bnMIDI = new System.Windows.Forms.ToolStripDropDownButton();
            this.cbMIDI = new System.Windows.Forms.ToolStripComboBox();
            this.bnPreview = new System.Windows.Forms.ToolStripDropDownButton();
            this.cbPreview = new System.Windows.Forms.ToolStripComboBox();
            this.bnPlayback = new System.Windows.Forms.ToolStripDropDownButton();
            this.cbPlayback = new System.Windows.Forms.ToolStripComboBox();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.statusStrip.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // CueList
            // 
            this.CueList.AllowDrop = true;
            this.CueList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CueList.AutoScroll = true;
            this.CueList.BackColor = System.Drawing.SystemColors.ControlDark;
            this.CueList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 556F));
            this.CueList.Location = new System.Drawing.Point(305, 34);
            this.CueList.Name = "CueList";
            this.CueList.Size = new System.Drawing.Size(556, 394);
            this.CueList.TabIndex = 4;
            this.CueList.Scroll += new System.Windows.Forms.ScrollEventHandler(this.CueList_Scroll);
            this.CueList.ClientSizeChanged += new System.EventHandler(this.CueList_ClientSizeChanged);
            this.CueList.ControlAdded += new System.Windows.Forms.ControlEventHandler(this.CueList_ControlAdded);
            this.CueList.ControlRemoved += new System.Windows.Forms.ControlEventHandler(this.CueList_ControlRemoved);
            this.CueList.DragDrop += new System.Windows.Forms.DragEventHandler(this.CueList_DragDrop);
            this.CueList.DragEnter += new System.Windows.Forms.DragEventHandler(this.CueList_DragEnter);
            this.CueList.DragOver += new System.Windows.Forms.DragEventHandler(this.CueList_DragOver);
            this.CueList.DragLeave += new System.EventHandler(this.CueList_DragLeave);
            this.CueList.MouseDown += new System.Windows.Forms.MouseEventHandler(this.CueList_MouseDown);
            // 
            // dlgOpenAudioFile
            // 
            this.dlgOpenAudioFile.FileName = "openFileDialog1";
            // 
            // rtMainText
            // 
            this.rtMainText.Location = new System.Drawing.Point(12, 244);
            this.rtMainText.Name = "rtMainText";
            this.rtMainText.Size = new System.Drawing.Size(287, 178);
            this.rtMainText.TabIndex = 2;
            this.rtMainText.Text = "";
            this.rtMainText.TextChanged += new System.EventHandler(this.rtMainText_TextChanged);
            // 
            // bnStopAll
            // 
            this.bnStopAll.BackColor = System.Drawing.Color.Red;
            this.bnStopAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bnStopAll.Location = new System.Drawing.Point(12, 142);
            this.bnStopAll.Name = "bnStopAll";
            this.bnStopAll.Size = new System.Drawing.Size(177, 81);
            this.bnStopAll.TabIndex = 17;
            this.bnStopAll.Text = "STOP";
            this.toolTip1.SetToolTip(this.bnStopAll, "Esc");
            this.bnStopAll.UseVisualStyleBackColor = false;
            this.bnStopAll.Click += new System.EventHandler(this.bnStopAll_Click);
            // 
            // ProgressTimer
            // 
            this.ProgressTimer.Tick += new System.EventHandler(this.ProgressTimer_Tick);
            // 
            // bnPlayNext
            // 
            this.bnPlayNext.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.bnPlayNext.Location = new System.Drawing.Point(256, 172);
            this.bnPlayNext.Name = "bnPlayNext";
            this.bnPlayNext.Size = new System.Drawing.Size(48, 21);
            this.bnPlayNext.TabIndex = 3;
            this.bnPlayNext.Text = "&Go ˃";
            this.toolTip1.SetToolTip(this.bnPlayNext, "F5");
            this.bnPlayNext.UseVisualStyleBackColor = false;
            this.bnPlayNext.Click += new System.EventHandler(this.bnPlayNext_Click);
            // 
            // bnAddCue
            // 
            this.bnAddCue.Location = new System.Drawing.Point(232, 174);
            this.bnAddCue.Name = "bnAddCue";
            this.bnAddCue.Size = new System.Drawing.Size(18, 19);
            this.bnAddCue.TabIndex = 6;
            this.bnAddCue.Text = "+";
            this.toolTip1.SetToolTip(this.bnAddCue, "Add Cue");
            this.bnAddCue.UseVisualStyleBackColor = true;
            this.bnAddCue.Click += new System.EventHandler(this.bnAddCue_Click);
            // 
            // bnDeleteCue
            // 
            this.bnDeleteCue.Location = new System.Drawing.Point(215, 174);
            this.bnDeleteCue.Name = "bnDeleteCue";
            this.bnDeleteCue.Size = new System.Drawing.Size(18, 19);
            this.bnDeleteCue.TabIndex = 7;
            this.bnDeleteCue.Text = "-";
            this.toolTip1.SetToolTip(this.bnDeleteCue, "Delete Cue");
            this.bnDeleteCue.UseVisualStyleBackColor = true;
            this.bnDeleteCue.Click += new System.EventHandler(this.bnDeleteCue_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.transportToolStripMenuItem,
            this.settingsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(338, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.toolStripSeparator1,
            this.exportShowFileToolStripMenuItem,
            this.importShowFileToolStripMenuItem,
            this.toolStripSeparator2,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.newToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.newToolStripMenuItem.Text = "&New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.openToolStripMenuItem.Text = "&Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.saveToolStripMenuItem.Text = "&Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.saveAsToolStripMenuItem.Text = "Save &As";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(160, 6);
            // 
            // exportShowFileToolStripMenuItem
            // 
            this.exportShowFileToolStripMenuItem.Name = "exportShowFileToolStripMenuItem";
            this.exportShowFileToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.exportShowFileToolStripMenuItem.Text = "&Export Show File";
            this.exportShowFileToolStripMenuItem.Click += new System.EventHandler(this.exportShowFileToolStripMenuItem_Click);
            // 
            // importShowFileToolStripMenuItem
            // 
            this.importShowFileToolStripMenuItem.Name = "importShowFileToolStripMenuItem";
            this.importShowFileToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.importShowFileToolStripMenuItem.Text = "&Import Show File";
            this.importShowFileToolStripMenuItem.Click += new System.EventHandler(this.importShowFileToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(160, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // transportToolStripMenuItem
            // 
            this.transportToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.playToolStripMenuItem,
            this.stopAllToolStripMenuItem,
            this.previousCueToolStripMenuItem,
            this.nextCueToolStripMenuItem,
            this.toolStripSeparator3});
            this.transportToolStripMenuItem.Name = "transportToolStripMenuItem";
            this.transportToolStripMenuItem.Size = new System.Drawing.Size(68, 20);
            this.transportToolStripMenuItem.Text = "&Transport";
            // 
            // playToolStripMenuItem
            // 
            this.playToolStripMenuItem.Name = "playToolStripMenuItem";
            this.playToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.playToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.playToolStripMenuItem.Text = "&Play";
            this.playToolStripMenuItem.Click += new System.EventHandler(this.playToolStripMenuItem_Click);
            // 
            // stopAllToolStripMenuItem
            // 
            this.stopAllToolStripMenuItem.Name = "stopAllToolStripMenuItem";
            this.stopAllToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F4;
            this.stopAllToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.stopAllToolStripMenuItem.Text = "Stop &All";
            this.stopAllToolStripMenuItem.Click += new System.EventHandler(this.stopAllToolStripMenuItem_Click);
            // 
            // previousCueToolStripMenuItem
            // 
            this.previousCueToolStripMenuItem.Name = "previousCueToolStripMenuItem";
            this.previousCueToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F6;
            this.previousCueToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.previousCueToolStripMenuItem.Text = "Previous Cue";
            this.previousCueToolStripMenuItem.Click += new System.EventHandler(this.previousCueToolStripMenuItem_Click);
            // 
            // nextCueToolStripMenuItem
            // 
            this.nextCueToolStripMenuItem.Name = "nextCueToolStripMenuItem";
            this.nextCueToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F7;
            this.nextCueToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.nextCueToolStripMenuItem.Text = "Next Cue";
            this.nextCueToolStripMenuItem.Click += new System.EventHandler(this.nextCueToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(159, 6);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.autoLoadLastsfxCuelistToolStripMenuItem});
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.settingsToolStripMenuItem.Text = "Settings";
            // 
            // autoLoadLastsfxCuelistToolStripMenuItem
            // 
            this.autoLoadLastsfxCuelistToolStripMenuItem.Checked = true;
            this.autoLoadLastsfxCuelistToolStripMenuItem.CheckOnClick = true;
            this.autoLoadLastsfxCuelistToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.autoLoadLastsfxCuelistToolStripMenuItem.Name = "autoLoadLastsfxCuelistToolStripMenuItem";
            this.autoLoadLastsfxCuelistToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
            this.autoLoadLastsfxCuelistToolStripMenuItem.Text = "Auto load last .sfx cue-list";
            this.autoLoadLastsfxCuelistToolStripMenuItem.Click += new System.EventHandler(this.autoLoadLastsfxCuelistToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            this.helpToolStripMenuItem.Click += new System.EventHandler(this.helpToolStripMenuItem_Click);
            // 
            // ScrollTimer
            // 
            this.ScrollTimer.Interval = 50;
            this.ScrollTimer.Tick += new System.EventHandler(this.ScrollTimer_Tick);
            // 
            // bnPrev
            // 
            this.bnPrev.Location = new System.Drawing.Point(256, 146);
            this.bnPrev.Name = "bnPrev";
            this.bnPrev.Size = new System.Drawing.Size(23, 23);
            this.bnPrev.TabIndex = 26;
            this.bnPrev.Text = "˄";
            this.bnPrev.UseVisualStyleBackColor = true;
            this.bnPrev.Click += new System.EventHandler(this.bnPrev_Click);
            // 
            // bnNext
            // 
            this.bnNext.Location = new System.Drawing.Point(256, 199);
            this.bnNext.Name = "bnNext";
            this.bnNext.Size = new System.Drawing.Size(23, 23);
            this.bnNext.TabIndex = 27;
            this.bnNext.Text = "˅";
            this.bnNext.UseVisualStyleBackColor = true;
            this.bnNext.Click += new System.EventHandler(this.bnNext_Click);
            // 
            // rtPrevMainText
            // 
            this.rtPrevMainText.Location = new System.Drawing.Point(12, 34);
            this.rtPrevMainText.Name = "rtPrevMainText";
            this.rtPrevMainText.Size = new System.Drawing.Size(287, 102);
            this.rtPrevMainText.TabIndex = 28;
            this.rtPrevMainText.Text = "";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.pictureBox1.Location = new System.Drawing.Point(284, 164);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(576, 3);
            this.pictureBox1.TabIndex = 29;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Visible = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.pictureBox2.Location = new System.Drawing.Point(284, 193);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(576, 3);
            this.pictureBox2.TabIndex = 30;
            this.pictureBox2.TabStop = false;
            this.pictureBox2.Visible = false;
            // 
            // statusStrip
            // 
            this.statusStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusBar,
            this.WebLink});
            this.statusStrip.Location = new System.Drawing.Point(0, 428);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(861, 22);
            this.statusStrip.TabIndex = 31;
            this.statusStrip.Text = "statusStrip1";
            // 
            // statusBar
            // 
            this.statusBar.Name = "statusBar";
            this.statusBar.Size = new System.Drawing.Size(798, 17);
            this.statusBar.Spring = true;
            this.statusBar.Text = "toolStripStatusLabel1";
            this.statusBar.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // WebLink
            // 
            this.WebLink.Name = "WebLink";
            this.WebLink.Size = new System.Drawing.Size(48, 17);
            this.WebLink.Text = "Remote";
            this.WebLink.Click += new System.EventHandler(this.WebLink_Click);
            // 
            // DeviceChangeTimer
            // 
            this.DeviceChangeTimer.Interval = 25;
            this.DeviceChangeTimer.Tick += new System.EventHandler(this.DeviceChangeTimer_Tick);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bnMIDI,
            this.bnPreview,
            this.bnPlayback});
            this.toolStrip1.Location = new System.Drawing.Point(299, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(87, 31);
            this.toolStrip1.TabIndex = 36;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // bnMIDI
            // 
            this.bnMIDI.BackColor = System.Drawing.Color.Red;
            this.bnMIDI.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bnMIDI.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cbMIDI});
            this.bnMIDI.Image = ((System.Drawing.Image)(resources.GetObject("bnMIDI.Image")));
            this.bnMIDI.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bnMIDI.Name = "bnMIDI";
            this.bnMIDI.ShowDropDownArrow = false;
            this.bnMIDI.Size = new System.Drawing.Size(28, 28);
            this.bnMIDI.Text = "toolStripDropDownButton1";
            this.bnMIDI.ToolTipText = "MIDI Out";
            // 
            // cbMIDI
            // 
            this.cbMIDI.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMIDI.DropDownWidth = 221;
            this.cbMIDI.Name = "cbMIDI";
            this.cbMIDI.Size = new System.Drawing.Size(121, 23);
            this.cbMIDI.SelectedIndexChanged += new System.EventHandler(this.cbMIDI_SelectedIndexChanged);
            // 
            // bnPreview
            // 
            this.bnPreview.BackColor = System.Drawing.Color.Red;
            this.bnPreview.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bnPreview.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cbPreview});
            this.bnPreview.Image = ((System.Drawing.Image)(resources.GetObject("bnPreview.Image")));
            this.bnPreview.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bnPreview.Name = "bnPreview";
            this.bnPreview.ShowDropDownArrow = false;
            this.bnPreview.Size = new System.Drawing.Size(28, 28);
            this.bnPreview.Text = "toolStripDropDownButton2";
            this.bnPreview.ToolTipText = "Preview Device";
            // 
            // cbPreview
            // 
            this.cbPreview.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPreview.DropDownWidth = 221;
            this.cbPreview.Name = "cbPreview";
            this.cbPreview.Size = new System.Drawing.Size(121, 23);
            this.cbPreview.SelectedIndexChanged += new System.EventHandler(this.cbPreview_SelectedIndexChanged);
            // 
            // bnPlayback
            // 
            this.bnPlayback.BackColor = System.Drawing.Color.Red;
            this.bnPlayback.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bnPlayback.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cbPlayback});
            this.bnPlayback.Image = ((System.Drawing.Image)(resources.GetObject("bnPlayback.Image")));
            this.bnPlayback.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bnPlayback.Name = "bnPlayback";
            this.bnPlayback.ShowDropDownArrow = false;
            this.bnPlayback.Size = new System.Drawing.Size(28, 28);
            this.bnPlayback.Text = "toolStripDropDownButton3";
            this.bnPlayback.ToolTipText = "Playback Device";
            // 
            // cbPlayback
            // 
            this.cbPlayback.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPlayback.DropDownWidth = 221;
            this.cbPlayback.Name = "cbPlayback";
            this.cbPlayback.Size = new System.Drawing.Size(121, 23);
            this.cbPlayback.SelectedIndexChanged += new System.EventHandler(this.cbPlayback_SelectedIndexChanged);
            // 
            // SFXPlayer
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(861, 450);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.rtPrevMainText);
            this.Controls.Add(this.bnNext);
            this.Controls.Add(this.bnPrev);
            this.Controls.Add(this.bnDeleteCue);
            this.Controls.Add(this.bnAddCue);
            this.Controls.Add(this.bnPlayNext);
            this.Controls.Add(this.bnStopAll);
            this.Controls.Add(this.rtMainText);
            this.Controls.Add(this.CueList);
            this.Controls.Add(this.menuStrip1);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(875, 482);
            this.Name = "SFXPlayer";
            this.RightToLeftLayout = true;
            this.Text = "Form1";
            this.toolTip1.SetToolTip(this, "Playback");
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ControlAdded += new System.Windows.Forms.ControlEventHandler(this.Form1_ControlAdded);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel CueList;
        private System.Windows.Forms.OpenFileDialog dlgOpenAudioFile;
        private System.Windows.Forms.RichTextBox rtMainText;
        private System.Windows.Forms.Button bnStopAll;
        private System.Windows.Forms.Timer ProgressTimer;
        private System.Windows.Forms.Button bnPlayNext;
        private System.Windows.Forms.Button bnAddCue;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button bnDeleteCue;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem exportShowFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importShowFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        internal System.Windows.Forms.Timer ScrollTimer;
        private System.Windows.Forms.ToolStripMenuItem transportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem playToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stopAllToolStripMenuItem;
        private System.Windows.Forms.Button bnPrev;
        private System.Windows.Forms.Button bnNext;
        private System.Windows.Forms.RichTextBox rtPrevMainText;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem autoLoadLastsfxCuelistToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem previousCueToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem nextCueToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel statusBar;
        private System.Windows.Forms.ToolStripStatusLabel WebLink;
        private System.Windows.Forms.Timer DeviceChangeTimer;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripDropDownButton bnMIDI;
        private System.Windows.Forms.ToolStripComboBox cbMIDI;
        private System.Windows.Forms.ToolStripDropDownButton bnPreview;
        private System.Windows.Forms.ToolStripComboBox cbPreview;
        private System.Windows.Forms.ToolStripDropDownButton bnPlayback;
        private System.Windows.Forms.ToolStripComboBox cbPlayback;
    }
}

