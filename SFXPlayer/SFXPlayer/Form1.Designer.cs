namespace SFXPlayer {
    partial class Form1 {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.CueList = new System.Windows.Forms.TableLayoutPanel();
            this.dlgOpenAudioFile = new System.Windows.Forms.OpenFileDialog();
            this.cbPlayback = new System.Windows.Forms.ComboBox();
            this.rtMainText = new System.Windows.Forms.RichTextBox();
            this.cbPreview = new System.Windows.Forms.ComboBox();
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
            this.mnuPreloadAll = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ScrollTimer = new System.Windows.Forms.Timer(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.bnPrev = new System.Windows.Forms.Button();
            this.bnNext = new System.Windows.Forms.Button();
            this.rtPrevMainText = new System.Windows.Forms.RichTextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.statusBar = new System.Windows.Forms.ToolStripStatusLabel();
            this.WebLink = new System.Windows.Forms.ToolStripStatusLabel();
            this.button1 = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.statusStrip.SuspendLayout();
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
            this.CueList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 741F));
            this.CueList.Location = new System.Drawing.Point(407, 33);
            this.CueList.Margin = new System.Windows.Forms.Padding(4);
            this.CueList.Name = "CueList";
            this.CueList.Size = new System.Drawing.Size(741, 494);
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
            // cbPlayback
            // 
            this.cbPlayback.FormattingEnabled = true;
            this.cbPlayback.Location = new System.Drawing.Point(460, 4);
            this.cbPlayback.Margin = new System.Windows.Forms.Padding(4);
            this.cbPlayback.Name = "cbPlayback";
            this.cbPlayback.Size = new System.Drawing.Size(243, 24);
            this.cbPlayback.TabIndex = 9;
            this.cbPlayback.SelectedIndexChanged += new System.EventHandler(this.cbPlayback_SelectedIndexChanged);
            // 
            // rtMainText
            // 
            this.rtMainText.Location = new System.Drawing.Point(16, 300);
            this.rtMainText.Margin = new System.Windows.Forms.Padding(4);
            this.rtMainText.Name = "rtMainText";
            this.rtMainText.Size = new System.Drawing.Size(381, 218);
            this.rtMainText.TabIndex = 2;
            this.rtMainText.Text = "";
            this.rtMainText.TextChanged += new System.EventHandler(this.rtMainText_TextChanged);
            // 
            // cbPreview
            // 
            this.cbPreview.FormattingEnabled = true;
            this.cbPreview.Location = new System.Drawing.Point(873, 4);
            this.cbPreview.Margin = new System.Windows.Forms.Padding(4);
            this.cbPreview.Name = "cbPreview";
            this.cbPreview.Size = new System.Drawing.Size(243, 24);
            this.cbPreview.TabIndex = 8;
            this.cbPreview.SelectedIndexChanged += new System.EventHandler(this.cbPreview_SelectedIndexChanged);
            // 
            // bnStopAll
            // 
            this.bnStopAll.BackColor = System.Drawing.Color.Red;
            this.bnStopAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bnStopAll.Location = new System.Drawing.Point(16, 175);
            this.bnStopAll.Margin = new System.Windows.Forms.Padding(4);
            this.bnStopAll.Name = "bnStopAll";
            this.bnStopAll.Size = new System.Drawing.Size(236, 100);
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
            this.bnPlayNext.Location = new System.Drawing.Point(341, 212);
            this.bnPlayNext.Margin = new System.Windows.Forms.Padding(4);
            this.bnPlayNext.Name = "bnPlayNext";
            this.bnPlayNext.Size = new System.Drawing.Size(64, 26);
            this.bnPlayNext.TabIndex = 3;
            this.bnPlayNext.Text = "&Play ˃";
            this.toolTip1.SetToolTip(this.bnPlayNext, "F5");
            this.bnPlayNext.UseVisualStyleBackColor = false;
            this.bnPlayNext.Click += new System.EventHandler(this.bnPlayNext_Click);
            // 
            // bnAddCue
            // 
            this.bnAddCue.Location = new System.Drawing.Point(309, 214);
            this.bnAddCue.Margin = new System.Windows.Forms.Padding(4);
            this.bnAddCue.Name = "bnAddCue";
            this.bnAddCue.Size = new System.Drawing.Size(24, 23);
            this.bnAddCue.TabIndex = 6;
            this.bnAddCue.Text = "+";
            this.toolTip1.SetToolTip(this.bnAddCue, "Add Cue");
            this.bnAddCue.UseVisualStyleBackColor = true;
            this.bnAddCue.Click += new System.EventHandler(this.bnAddCue_Click);
            // 
            // bnDeleteCue
            // 
            this.bnDeleteCue.Location = new System.Drawing.Point(287, 214);
            this.bnDeleteCue.Margin = new System.Windows.Forms.Padding(4);
            this.bnDeleteCue.Name = "bnDeleteCue";
            this.bnDeleteCue.Size = new System.Drawing.Size(24, 23);
            this.bnDeleteCue.TabIndex = 7;
            this.bnDeleteCue.Text = "-";
            this.toolTip1.SetToolTip(this.bnDeleteCue, "Delete Cue");
            this.bnDeleteCue.UseVisualStyleBackColor = true;
            this.bnDeleteCue.Click += new System.EventHandler(this.bnDeleteCue_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.transportToolStripMenuItem,
            this.settingsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(8, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(1148, 28);
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
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(44, 24);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.newToolStripMenuItem.Size = new System.Drawing.Size(196, 26);
            this.newToolStripMenuItem.Text = "&New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openToolStripMenuItem.Size = new System.Drawing.Size(196, 26);
            this.openToolStripMenuItem.Text = "&Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(196, 26);
            this.saveToolStripMenuItem.Text = "&Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(196, 26);
            this.saveAsToolStripMenuItem.Text = "Save &As";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(193, 6);
            // 
            // exportShowFileToolStripMenuItem
            // 
            this.exportShowFileToolStripMenuItem.Name = "exportShowFileToolStripMenuItem";
            this.exportShowFileToolStripMenuItem.Size = new System.Drawing.Size(196, 26);
            this.exportShowFileToolStripMenuItem.Text = "&Export Show File";
            this.exportShowFileToolStripMenuItem.Click += new System.EventHandler(this.exportShowFileToolStripMenuItem_Click);
            // 
            // importShowFileToolStripMenuItem
            // 
            this.importShowFileToolStripMenuItem.Name = "importShowFileToolStripMenuItem";
            this.importShowFileToolStripMenuItem.Size = new System.Drawing.Size(196, 26);
            this.importShowFileToolStripMenuItem.Text = "&Import Show File";
            this.importShowFileToolStripMenuItem.Click += new System.EventHandler(this.importShowFileToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(193, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(196, 26);
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
            this.transportToolStripMenuItem.Size = new System.Drawing.Size(83, 24);
            this.transportToolStripMenuItem.Text = "&Transport";
            // 
            // playToolStripMenuItem
            // 
            this.playToolStripMenuItem.Name = "playToolStripMenuItem";
            this.playToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.playToolStripMenuItem.Size = new System.Drawing.Size(192, 26);
            this.playToolStripMenuItem.Text = "&Play";
            this.playToolStripMenuItem.Click += new System.EventHandler(this.playToolStripMenuItem_Click);
            // 
            // stopAllToolStripMenuItem
            // 
            this.stopAllToolStripMenuItem.Name = "stopAllToolStripMenuItem";
            this.stopAllToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F4;
            this.stopAllToolStripMenuItem.Size = new System.Drawing.Size(192, 26);
            this.stopAllToolStripMenuItem.Text = "Stop &All";
            this.stopAllToolStripMenuItem.Click += new System.EventHandler(this.stopAllToolStripMenuItem_Click);
            // 
            // previousCueToolStripMenuItem
            // 
            this.previousCueToolStripMenuItem.Name = "previousCueToolStripMenuItem";
            this.previousCueToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F6;
            this.previousCueToolStripMenuItem.Size = new System.Drawing.Size(192, 26);
            this.previousCueToolStripMenuItem.Text = "Previous Cue";
            this.previousCueToolStripMenuItem.Click += new System.EventHandler(this.previousCueToolStripMenuItem_Click);
            // 
            // nextCueToolStripMenuItem
            // 
            this.nextCueToolStripMenuItem.Name = "nextCueToolStripMenuItem";
            this.nextCueToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F7;
            this.nextCueToolStripMenuItem.Size = new System.Drawing.Size(192, 26);
            this.nextCueToolStripMenuItem.Text = "Next Cue";
            this.nextCueToolStripMenuItem.Click += new System.EventHandler(this.nextCueToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(189, 6);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.autoLoadLastsfxCuelistToolStripMenuItem,
            this.mnuPreloadAll});
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(74, 24);
            this.settingsToolStripMenuItem.Text = "Settings";
            // 
            // autoLoadLastsfxCuelistToolStripMenuItem
            // 
            this.autoLoadLastsfxCuelistToolStripMenuItem.Checked = true;
            this.autoLoadLastsfxCuelistToolStripMenuItem.CheckOnClick = true;
            this.autoLoadLastsfxCuelistToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.autoLoadLastsfxCuelistToolStripMenuItem.Name = "autoLoadLastsfxCuelistToolStripMenuItem";
            this.autoLoadLastsfxCuelistToolStripMenuItem.Size = new System.Drawing.Size(254, 26);
            this.autoLoadLastsfxCuelistToolStripMenuItem.Text = "Auto load last .sfx cue-list";
            this.autoLoadLastsfxCuelistToolStripMenuItem.Click += new System.EventHandler(this.autoLoadLastsfxCuelistToolStripMenuItem_Click);
            // 
            // mnuPreloadAll
            // 
            this.mnuPreloadAll.Checked = true;
            this.mnuPreloadAll.CheckOnClick = true;
            this.mnuPreloadAll.CheckState = System.Windows.Forms.CheckState.Checked;
            this.mnuPreloadAll.Name = "mnuPreloadAll";
            this.mnuPreloadAll.Size = new System.Drawing.Size(254, 26);
            this.mnuPreloadAll.Text = "Pre&load All";
            this.mnuPreloadAll.Click += new System.EventHandler(this.mnuPreloadAll_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(53, 24);
            this.helpToolStripMenuItem.Text = "Help";
            this.helpToolStripMenuItem.Click += new System.EventHandler(this.helpToolStripMenuItem_Click);
            // 
            // ScrollTimer
            // 
            this.ScrollTimer.Interval = 50;
            this.ScrollTimer.Tick += new System.EventHandler(this.ScrollTimer_Tick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(384, 7);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 17);
            this.label1.TabIndex = 24;
            this.label1.Text = "Playback";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(712, 7);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(152, 17);
            this.label2.TabIndex = 25;
            this.label2.Text = "Preview (Headphones)";
            // 
            // bnPrev
            // 
            this.bnPrev.Location = new System.Drawing.Point(341, 180);
            this.bnPrev.Margin = new System.Windows.Forms.Padding(4);
            this.bnPrev.Name = "bnPrev";
            this.bnPrev.Size = new System.Drawing.Size(31, 28);
            this.bnPrev.TabIndex = 26;
            this.bnPrev.Text = "˄";
            this.bnPrev.UseVisualStyleBackColor = true;
            this.bnPrev.Click += new System.EventHandler(this.bnPrev_Click);
            // 
            // bnNext
            // 
            this.bnNext.Location = new System.Drawing.Point(341, 245);
            this.bnNext.Margin = new System.Windows.Forms.Padding(4);
            this.bnNext.Name = "bnNext";
            this.bnNext.Size = new System.Drawing.Size(31, 28);
            this.bnNext.TabIndex = 27;
            this.bnNext.Text = "˅";
            this.bnNext.UseVisualStyleBackColor = true;
            this.bnNext.Click += new System.EventHandler(this.bnNext_Click);
            // 
            // rtPrevMainText
            // 
            this.rtPrevMainText.Location = new System.Drawing.Point(16, 42);
            this.rtPrevMainText.Margin = new System.Windows.Forms.Padding(4);
            this.rtPrevMainText.Name = "rtPrevMainText";
            this.rtPrevMainText.Size = new System.Drawing.Size(381, 125);
            this.rtPrevMainText.TabIndex = 28;
            this.rtPrevMainText.Text = "";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.pictureBox1.Location = new System.Drawing.Point(379, 202);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(768, 4);
            this.pictureBox1.TabIndex = 29;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Visible = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.pictureBox2.Location = new System.Drawing.Point(379, 238);
            this.pictureBox2.Margin = new System.Windows.Forms.Padding(4);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(768, 4);
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
            this.statusStrip.Location = new System.Drawing.Point(0, 529);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Padding = new System.Windows.Forms.Padding(1, 0, 19, 0);
            this.statusStrip.Size = new System.Drawing.Size(1148, 25);
            this.statusStrip.TabIndex = 31;
            this.statusStrip.Text = "statusStrip1";
            // 
            // statusBar
            // 
            this.statusBar.Name = "statusBar";
            this.statusBar.Size = new System.Drawing.Size(1067, 20);
            this.statusBar.Spring = true;
            this.statusBar.Text = "toolStripStatusLabel1";
            this.statusBar.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // WebLink
            // 
            this.WebLink.Name = "WebLink";
            this.WebLink.Size = new System.Drawing.Size(61, 20);
            this.WebLink.Text = "Remote";
            this.WebLink.Click += new System.EventHandler(this.WebLink_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(270, 181);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 32;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // Form1
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1148, 554);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.rtPrevMainText);
            this.Controls.Add(this.bnNext);
            this.Controls.Add(this.bnPrev);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.bnDeleteCue);
            this.Controls.Add(this.bnAddCue);
            this.Controls.Add(this.bnPlayNext);
            this.Controls.Add(this.bnStopAll);
            this.Controls.Add(this.cbPreview);
            this.Controls.Add(this.rtMainText);
            this.Controls.Add(this.cbPlayback);
            this.Controls.Add(this.CueList);
            this.Controls.Add(this.menuStrip1);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MinimumSize = new System.Drawing.Size(1161, 584);
            this.Name = "Form1";
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
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel CueList;
        private System.Windows.Forms.OpenFileDialog dlgOpenAudioFile;
        private System.Windows.Forms.ComboBox cbPlayback;
        private System.Windows.Forms.RichTextBox rtMainText;
        private System.Windows.Forms.ComboBox cbPreview;
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
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
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
        private System.Windows.Forms.ToolStripMenuItem mnuPreloadAll;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel statusBar;
        private System.Windows.Forms.ToolStripStatusLabel WebLink;
        private System.Windows.Forms.Button button1;
    }
}

