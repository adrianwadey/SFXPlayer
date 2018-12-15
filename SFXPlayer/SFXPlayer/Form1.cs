using CSCore.CoreAudioAPI;
using SFXPlayer.Properties;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AJW.General;

namespace SFXPlayer {
    public partial class Form1 : Form {
        const string FileExtensions = "Show Files (*.sfx)|*.sfx";
        private XMLFileHandler<Show> FileHandler = new XMLFileHandler<Show>();
        private Show CurrentShow;
        private const int cueListSpacing = 35;
        private readonly ObservableCollection<MMDevice> _devices = new ObservableCollection<MMDevice>();

        public Form1() {
            // Initialize Forms Designer generated code.
            InitializeComponent();

            // Set up the status bar and other controls.
            InitializeControls();

            // Set up the SoundPlayer object.
            InitializeSound();
        }

        // Sets up the status bar and other controls.
        private void InitializeControls() {
            // Set up the status bar.
            //CurrentShow.Panel = CueList;
            StatusBarPanel panel = new StatusBarPanel();
            panel.BorderStyle = StatusBarPanelBorderStyle.Sunken;
            panel.Text = "Ready.";
            panel.AutoSize = StatusBarPanelAutoSize.Spring;
            this.statusBar.ShowPanels = true;
            this.statusBar.Panels.Add(panel);

            FileHandler.FileExtensions = FileExtensions;
            cuelistFormSpacing = this.Height - CueList.Height;
            bnPlayNext.Top = TOP_PLACEHOLDERS * cueListSpacing + CueList.Top;
            bnPlayNext.Height = new PlayStrip().Height;
            bnPlayNext.BackColor = Settings.Default.ColourPlayerPlay;
            bnDeleteCue.Top = bnAddCue.Top = bnPlayNext.Top + (bnPlayNext.Height - bnAddCue.Height) / 2;
            rtMainText.Top = bnPlayNext.Bottom + rtMainText.Margin.Top + bnPlayNext.Margin.Bottom;
            PlayStrip.OFD = openFileDialog1;
            PlayStrip.Devices = comboBox1;
            PlayStrip.PreviewDevices = comboBox2;
        }

        // Sets up the SoundPlayer object.
        private void InitializeSound() {
            // Create an instance of the SoundPlayer class.
            //player = new SoundPlayer();

            // Listen for the LoadCompleted event.
            //player.LoadCompleted += new AsyncCompletedEventHandler(player_LoadCompleted);

            // Listen for the SoundLocationChanged event.
            //player.SoundLocationChanged += new EventHandler(player_LocationChanged);
        }

        
        // Convenience method for setting message text in 
        // the status bar.
        private void ReportStatus(string statusMessage) {
            // If the caller passed in a message...
            if ((statusMessage != null) && (statusMessage != String.Empty)) {
                // ...post the caller's message to the status bar.
                this.statusBar.Panels[0].Text = statusMessage;
            }
        }

        const int TOP_PLACEHOLDERS = 5;
        int BottomPlaceholders = 0;
        int paddedBottom = 0;
        int paddedTop = 0;
        int cuelistFormSpacing = 0;

        public PlayStrip NextPlayCue {
            get {
                return ((PlayStrip)CueList.Controls[paddedTop + NextPlayCueIndex]);
            }
        }

        public int NextPlayCueIndex {
            get {
                return CueList.VerticalScroll.Value / cueListSpacing;
            }
            set {
                int NewValue = value * cueListSpacing;
                NewValue = Math.Min(NewValue, CueList.VerticalScroll.Maximum);
                CueList.VerticalScroll.Value = NewValue;
                CueList.VerticalScroll.Value = NewValue;        //update only seems to work when called twice.
                NextPlayCueChanged();
            }
        }

        private void NextPlayCueChanged() {
            rtMainText.Text = NextPlayCue.SFX.MainText;
        }

        private void Form1_Load(object sender, EventArgs e) {
            Debug.WriteLine("Form1_Load");
            Debug.WriteLine("Scroll Lines = " + SystemInformation.MouseWheelScrollLines);

            //get the sound devices
            using (var mmdeviceEnumerator = new MMDeviceEnumerator()) {
                using (
                    var mmdeviceCollection = mmdeviceEnumerator.EnumAudioEndpoints(DataFlow.Render, DeviceState.Active)) {
                    foreach (var device in mmdeviceCollection) {
                        _devices.Add(device);
                    }
                }
            }
            //Normal playback device
            comboBox1.DataSource = _devices;
            comboBox1.DisplayMember = "FriendlyName";
            comboBox1.ValueMember = "DeviceID";

            //Preview playback device
            comboBox2.DataSource = _devices;
            comboBox2.DisplayMember = "FriendlyName";
            comboBox2.ValueMember = "DeviceID";

            ProgressTimer.Enabled = true;
            FileNew();
            ResetDisplay();

            Form1_Resize(this, new EventArgs());

            CueList.MouseWheel += CueList_MouseWheel;
            CueList.ControlAdded += CueList_ControlAdded;
        }

        private void CueList_ControlAdded(object sender, ControlEventArgs e) {
            e.Control.MouseWheel += CueList_MouseWheel;
        }

        private void CueList_MouseWheel(object sender, MouseEventArgs e) {
            //e = new HandledMouseEventArgs(e.Button, e.Clicks, e.X, e.Y, e.Delta, true);
            ReportStatus(((Control)sender).Name + " MouseWheel " + e.Delta.ToString("D"));
            ScrollTimer.Enabled = true;
        }

        //private void Ctl_MouseWheel(object sender, MouseEventArgs e) {
        //    ReportStatus(((Control)sender).Name + " MouseWheel " + e.Delta.ToString("D"));
        //    e.Handled = true;
        //    ((HandledMouseEventArgs)e).Handled = true;
        //}

        //private void Ctl_MouseWheel(object sender, EventArgs e) {
        //    ReportStatus(((Control)sender).Name + " " + ((Control)sender).GetType().ToString());
        //}

        void FileNew() {
            CueList.Controls.Clear();
            CurrentShow = new Show();
            CurrentShow.UpdateShow += UpdateDisplay;
        }

        void FileOpen() {
            Show oldShow = CurrentShow;
            CurrentShow = FileHandler.LoadFromFile();
            if (CurrentShow != null) {
                CurrentShow.UpdateShow += UpdateDisplay;
                ResetDisplay();
            } else {
                CurrentShow = oldShow;
            }
        }

        void UpdateDisplay() {

        }

        void ResetDisplay() {
            CueList.Controls.Clear();
            foreach (SFX sfx in CurrentShow.Cues) {
                PlayStrip ps = new PlayStrip(sfx);
                ps.StopAll += StopAll;
                CueList.Controls.Add(ps);
            }
            paddedTop = paddedBottom = 0;
            PadCueList();
            NextPlayCueChanged();
        }

        private void StopAll(object sender, EventArgs e) {
            foreach (PlayStrip Player in CueList.Controls) {
                Player.StopOthers(sender, e);
            }
        }

        private void PreloadAll(object sender, EventArgs e) {
            foreach (PlayStrip Player in CueList.Controls) {
                Player.PreloadFile(sender, e);
            }
        }

        /// <summary>
        /// Add invisible CueList controls to set the correct scroll limits
        /// </summary>
        private void PadCueList() {
            Debug.WriteLine("PadCueList");
            BottomPlaceholders = (CueList.Height - 8) / cueListSpacing - TOP_PLACEHOLDERS + 1;
            while (paddedTop > TOP_PLACEHOLDERS) {
                CueList.Controls.RemoveAt(paddedTop--);
            }
            while (paddedTop < TOP_PLACEHOLDERS) {
                PlayStrip ph = new PlayStrip { isPlaceholder = true };
                CueList.Controls.Add(ph);
                CueList.Controls.SetChildIndex(ph, 0);
                paddedTop++;
            }
            while (paddedBottom > BottomPlaceholders) {
                CueList.Controls.RemoveAt(CueList.Controls.Count - 1);
                paddedBottom--;
            }
            while (paddedBottom < BottomPlaceholders) {
                PlayStrip ph = new PlayStrip { isPlaceholder = true };
                CueList.Controls.Add(ph);
                //CueList.Controls.SetChildIndex(ph, );
                paddedBottom++;
            }

            int temppos = NextPlayCueIndex;
            NextPlayCueIndex = 0;
            int t = 0;
            foreach (Control cue in CueList.Controls) {
                cue.Top = t;
                t += cueListSpacing;
            }
            NextPlayCueIndex = temppos;

            CueList.VerticalScroll.Enabled = true;
            CueList.VerticalScroll.Visible = true;
            CueList.VerticalScroll.Minimum = 0;
            CueList.VerticalScroll.Maximum = (CueList.Controls.Count - TOP_PLACEHOLDERS - BottomPlaceholders) * cueListSpacing - 1;
            CueList.VerticalScroll.LargeChange = cueListSpacing;
            CueList.VerticalScroll.SmallChange = cueListSpacing;
            //CueList.AutoScrollOffset = new Point(0, TOP_PLACEHOLDERS * cueListSpacing);
        }

        private void CueList_Scroll(object sender, ScrollEventArgs e) {
            Debug.WriteLine("Scroll");
            Debug.WriteLine("Type " + e.Type);
            Debug.WriteLine("Orientation " + e.ScrollOrientation);
            Debug.WriteLine("Old=" + e.OldValue);
            Debug.WriteLine("New=" + e.NewValue);
            Debug.WriteLine("Min " + CueList.VerticalScroll.Minimum);
            Debug.WriteLine("Max " + CueList.VerticalScroll.Maximum);
            Debug.WriteLine("Lg " + CueList.VerticalScroll.LargeChange);
            Debug.WriteLine("Sm " + CueList.VerticalScroll.SmallChange);
            //((System.Windows.Forms.FlowLayoutPanel..ScrollBar)sender).Value = e.NewValue;
            //ReportStatus(
            //    "Type " + e.Type +
            //    ", Orientation " + e.ScrollOrientation +
            //    ", Old=" + e.OldValue +
            //    ", New=" + e.NewValue +
            //    ", Min " + CueList.VerticalScroll.Minimum +
            //    ", Max " + CueList.VerticalScroll.Maximum +
            //    ", Lg " + CueList.VerticalScroll.LargeChange +
            //    ", Sm " + CueList.VerticalScroll.SmallChange
            //);
            if (e.NewValue > e.OldValue) {
                e.NewValue = ((e.NewValue /*+ cueListSpacing/2*/) / cueListSpacing) * cueListSpacing;
            } else {
                e.NewValue = ((e.NewValue /*+ cueListSpacing / 2*/) / cueListSpacing) * cueListSpacing;
            }
            CueList.VerticalScroll.Value = Math.Min(CueList.VerticalScroll.Maximum, e.NewValue);
            NextPlayCueChanged();
            //ReportStatus("Scrolled to " + e.NewValue.ToString("D"));
        }

        private void button1_Click(object sender, EventArgs e) {
            StopAll(sender, e);
        }

        private void button2_Click(object sender, EventArgs e) {
            PreloadAll(sender, e);
        }

        private void ProgressTimer_Tick(object sender, EventArgs e) {
            foreach (PlayStrip Player in CueList.Controls) {
                if (Player.IsPlaying) {
                    Player.ProgressUpdate(sender, e);
                }
            }
        }

        private void bnPlayNext_Click(object sender, EventArgs e) {
            NextPlayCue.Play();
            NextPlayCueIndex += 1;
        }

        private void bnAddCue_Click(object sender, EventArgs e) {
            SFX sfx = new SFX();
            CurrentShow.AddCue(sfx, Math.Min(NextPlayCueIndex, CurrentShow.Cues.Count));
            PlayStrip ps = new PlayStrip(sfx);
            CueList.Controls.Add(ps);
            CueList.Controls.SetChildIndex(ps, paddedTop + CurrentShow.Cues.IndexOf(sfx));
            //CueList.Refresh();
            PadCueList();
            NextPlayCueChanged();
        }

        private void bnDeleteCue_Click(object sender, EventArgs e) {
            if (((PlayStrip)CueList.Controls[NextPlayCueIndex + paddedTop]).isPlaceholder) return;
            if (CurrentShow.DeleteCue(NextPlayCueIndex) == DialogResult.Yes) {
                CueList.Controls.RemoveAt(NextPlayCueIndex + paddedTop);
                PadCueList();
                NextPlayCueChanged();
            }
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e) {
            if (FileHandler.CheckSave(CurrentShow) != DialogResult.OK) return;
            FileNew();
            NextPlayCueChanged();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e) {
            if (e.CloseReason == CloseReason.TaskManagerClosing) return;    //allow close from task manager
            if (FileHandler.CheckSave(CurrentShow) != DialogResult.OK) {
                e.Cancel = true;
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e) {
            FileHandler.Save(CurrentShow);
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e) {
            FileHandler.SaveAs(CurrentShow);
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e) {
            if (FileHandler.CheckSave(CurrentShow) != DialogResult.OK) return;
            FileOpen();
        }

        private void ScrollTimer_Tick(object sender, EventArgs e) {
            ScrollTimer.Enabled = false;
            CueList.VerticalScroll.Value = CueList.VerticalScroll.Value / cueListSpacing * cueListSpacing;
            NextPlayCueChanged();
        }

        private void Form1_Resize(object sender, EventArgs e) {
            if (WindowState == FormWindowState.Minimized) return;
            int cuelistrows;
            Debug.WriteLine("CueList_Resize");
            cuelistrows = (Height - cuelistFormSpacing) / cueListSpacing;
            CueList.Height = cuelistrows * cueListSpacing - 8;
            //this.statusBar.Panels[0].Text = "NumberOfPlaceholders = " + (BottomPlaceholders + TOP_PLACEHOLDERS).ToString();
            PadCueList();

            rtMainText.Height = statusBar.Top - rtMainText.Margin.Bottom - rtMainText.Top;
        }

        private void CueList_ClientSizeChanged(object sender, EventArgs e) {
            foreach (PlayStrip playStrip in CueList.Controls) {
                playStrip.Width = CueList.ClientSize.Width;
            }
        }

        private void rtMainText_TextChanged(object sender, EventArgs e) {
            NextPlayCue.SFX.MainText = rtMainText.Text;
        }
    }
}
