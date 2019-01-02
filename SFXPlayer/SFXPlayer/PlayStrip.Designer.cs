﻿namespace SFXPlayer {
    partial class PlayStrip {
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
            this.components = new System.ComponentModel.Container();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lbIndex = new System.Windows.Forms.Label();
            this.tbDescription = new System.Windows.Forms.TextBox();
            this.bnPlay = new System.Windows.Forms.Button();
            this.bnPause = new System.Windows.Forms.Button();
            this.bnStopAll = new System.Windows.Forms.CheckBox();
            this.bnFile = new System.Windows.Forms.Button();
            this.bnVolume = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.previewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tableLayoutPanel1.SuspendLayout();
            this.contextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 7;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tableLayoutPanel1.Controls.Add(this.lbIndex, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tbDescription, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.bnPlay, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.bnPause, 4, 0);
            this.tableLayoutPanel1.Controls.Add(this.bnStopAll, 5, 0);
            this.tableLayoutPanel1.Controls.Add(this.bnFile, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.bnVolume, 6, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(386, 27);
            this.tableLayoutPanel1.TabIndex = 0;
            this.tableLayoutPanel1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.tableLayoutPanel1_MouseDoubleClick);
            // 
            // lbIndex
            // 
            this.lbIndex.AutoSize = true;
            this.lbIndex.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbIndex.Location = new System.Drawing.Point(3, 0);
            this.lbIndex.Name = "lbIndex";
            this.lbIndex.Size = new System.Drawing.Size(25, 27);
            this.lbIndex.TabIndex = 0;
            this.lbIndex.Text = "000";
            this.lbIndex.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbIndex.DoubleClick += new System.EventHandler(this.lbIndex_DoubleClick);
            // 
            // tbDescription
            // 
            this.tbDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbDescription.Location = new System.Drawing.Point(34, 3);
            this.tbDescription.Name = "tbDescription";
            this.tbDescription.Size = new System.Drawing.Size(219, 20);
            this.tbDescription.TabIndex = 1;
            this.tbDescription.TextChanged += new System.EventHandler(this.tbDescription_TextChanged);
            this.tbDescription.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.tbDescription_MouseDoubleClick);
            // 
            // bnPlay
            // 
            this.bnPlay.BackgroundImage = global::SFXPlayer.Properties.Resources.Play;
            this.bnPlay.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.bnPlay.Location = new System.Drawing.Point(285, 3);
            this.bnPlay.Name = "bnPlay";
            this.bnPlay.Size = new System.Drawing.Size(20, 20);
            this.bnPlay.TabIndex = 2;
            this.bnPlay.UseVisualStyleBackColor = true;
            this.bnPlay.Click += new System.EventHandler(this.bnPlay_Click);
            // 
            // bnPause
            // 
            this.bnPause.BackgroundImage = global::SFXPlayer.Properties.Resources.Stop;
            this.bnPause.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.bnPause.Location = new System.Drawing.Point(311, 3);
            this.bnPause.Name = "bnPause";
            this.bnPause.Size = new System.Drawing.Size(20, 20);
            this.bnPause.TabIndex = 3;
            this.bnPause.UseVisualStyleBackColor = true;
            this.bnPause.Click += new System.EventHandler(this.bnStop_Click);
            // 
            // bnStopAll
            // 
            this.bnStopAll.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.bnStopAll.Location = new System.Drawing.Point(340, 6);
            this.bnStopAll.Margin = new System.Windows.Forms.Padding(6);
            this.bnStopAll.Name = "bnStopAll";
            this.bnStopAll.Size = new System.Drawing.Size(14, 15);
            this.bnStopAll.TabIndex = 4;
            this.toolTip1.SetToolTip(this.bnStopAll, "Stop All Other Sounds");
            this.bnStopAll.UseVisualStyleBackColor = true;
            this.bnStopAll.CheckedChanged += new System.EventHandler(this.bnStopAll_CheckedChanged);
            // 
            // bnFile
            // 
            this.bnFile.Location = new System.Drawing.Point(259, 3);
            this.bnFile.Name = "bnFile";
            this.bnFile.Size = new System.Drawing.Size(20, 21);
            this.bnFile.TabIndex = 5;
            this.bnFile.Text = "File";
            this.toolTip1.SetToolTip(this.bnFile, "File");
            this.bnFile.UseVisualStyleBackColor = true;
            this.bnFile.Click += new System.EventHandler(this.bnFile_Click);
            // 
            // bnVolume
            // 
            this.bnVolume.Location = new System.Drawing.Point(363, 3);
            this.bnVolume.Name = "bnVolume";
            this.bnVolume.Size = new System.Drawing.Size(20, 20);
            this.bnVolume.TabIndex = 6;
            this.bnVolume.Text = "Volume";
            this.bnVolume.UseVisualStyleBackColor = true;
            this.bnVolume.Click += new System.EventHandler(this.bnVolume_Click);
            this.bnVolume.Enter += new System.EventHandler(this.bnVolume_Enter);
            // 
            // contextMenu
            // 
            this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.previewToolStripMenuItem});
            this.contextMenu.Name = "contextMenuStrip1";
            this.contextMenu.Size = new System.Drawing.Size(116, 26);
            this.contextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenu_Opening);
            // 
            // previewToolStripMenuItem
            // 
            this.previewToolStripMenuItem.Name = "previewToolStripMenuItem";
            this.previewToolStripMenuItem.Size = new System.Drawing.Size(115, 22);
            this.previewToolStripMenuItem.Text = "&Preview";
            this.previewToolStripMenuItem.Click += new System.EventHandler(this.previewToolStripMenuItem_Click);
            // 
            // PlayStrip
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ContextMenuStrip = this.contextMenu;
            this.Controls.Add(this.tableLayoutPanel1);
            this.DoubleBuffered = true;
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "PlayStrip";
            this.Size = new System.Drawing.Size(386, 27);
            this.Load += new System.EventHandler(this.PlayStrip_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MouseDownHandler);
            this.Resize += new System.EventHandler(this.PlayStrip_Resize);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.contextMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lbIndex;
        private System.Windows.Forms.TextBox tbDescription;
        private System.Windows.Forms.Button bnPlay;
        private System.Windows.Forms.Button bnPause;
        private System.Windows.Forms.CheckBox bnStopAll;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button bnFile;
        private System.Windows.Forms.Button bnVolume;
        private System.Windows.Forms.ContextMenuStrip contextMenu;
        private System.Windows.Forms.ToolStripMenuItem previewToolStripMenuItem;
    }
}
