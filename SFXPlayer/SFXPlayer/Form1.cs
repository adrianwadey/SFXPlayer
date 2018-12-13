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

namespace SFXPlayer {
    public partial class Form1 : Form {
        //private SoundPlayer player;

        private Show CurrentShow = new Show();
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
            CurrentShow.Panel = CueList;
            StatusBarPanel panel = new StatusBarPanel();
            panel.BorderStyle = StatusBarPanelBorderStyle.Sunken;
            panel.Text = "Ready.";
            panel.AutoSize = StatusBarPanelAutoSize.Spring;
            this.statusBar.ShowPanels = true;
            this.statusBar.Panels.Add(panel);

            cuelistFormSpacing = this.Height - CueList.Height;
            pbPlaying.Top = TOP_PLACEHOLDERS * cueListSpacing + CueList.Top;
            pbPlaying.Height = new PlayStrip().Height;
            pbPlaying.BackColor = Settings.Default.ColourPlayerPlay;
            bnAddCue.Top = pbPlaying.Top + cueListSpacing;
            bnDeleteCue.Top = pbPlaying.Top + cueListSpacing;
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

        private void selectFileButton_Click(object sender,
            System.EventArgs e) {
            // Create a new OpenFileDialog.
            OpenFileDialog dlg = new OpenFileDialog();

            // Make sure the dialog checks for existence of the 
            // selected file.
            dlg.CheckFileExists = true;

            // Allow selection of .wav files only.
            dlg.Filter = "WAV files (*.wav)|*.wav";
            dlg.DefaultExt = ".wav";

            // Activate the file selection dialog.
            if (dlg.ShowDialog() == DialogResult.OK) {
                // Get the selected file's path from the dialog.
                this.filepathTextbox.Text = dlg.FileName;

                // Assign the selected file's path to 
                // the SoundPlayer object.  
                //player.SoundLocation = filepathTextbox.Text;
            }
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
        private int CurrentIndex;

        private void Form1_Load(object sender, EventArgs e) {
            Debug.WriteLine("Form1_Load");

            //get the sound devices
            using (var mmdeviceEnumerator = new MMDeviceEnumerator()) {
                using (
                    var mmdeviceCollection = mmdeviceEnumerator.EnumAudioEndpoints(DataFlow.Render, DeviceState.Active)) {
                    foreach (var device in mmdeviceCollection) {
                        _devices.Add(device);
                    }
                }
            }
            comboBox1.DataSource = _devices;
            comboBox1.DisplayMember = "FriendlyName";
            comboBox1.ValueMember = "DeviceID";

            comboBox2.DataSource = _devices;
            comboBox2.DisplayMember = "FriendlyName";
            comboBox2.ValueMember = "DeviceID";


            //for (int i = 0; i < 7; i++) {
            //    PlayStrip ps = new PlayStrip {
            //        Index = i + 1,
            //    };
            //    ps.StopAll += StopAll;
            //    CueList.Controls.Add(ps);
            //}
            //PadCueList();
            timer1.Enabled = true;
            FileNew();
        }

        void FileNew() {
            CueList.Controls.Clear();
            PadCueList();
            CurrentShow = new Show();
            CurrentIndex = 0;
            CurrentShow.AddCue(new SFX(), CurrentIndex);
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
            while (paddedTop > TOP_PLACEHOLDERS) {
                CueList.Controls.RemoveAt(paddedTop--);
            }
            while (paddedTop < TOP_PLACEHOLDERS) {
                PlayStrip ph = new PlayStrip { Placeholder = true };
                CueList.Controls.Add(ph);
                CueList.Controls.SetChildIndex(ph, 0);
                paddedTop++;
            }
            while (paddedBottom > BottomPlaceholders) {
                CueList.Controls.RemoveAt(CueList.Controls.Count - 1);
                paddedBottom--;
            }
            while (paddedBottom < BottomPlaceholders) {
                PlayStrip ph = new PlayStrip { Placeholder = true };
                CueList.Controls.Add(ph);
                //CueList.Controls.SetChildIndex(ph, );
                paddedBottom++;
            }

            int t = 0;
            foreach (Control cue in CueList.Controls) {
                cue.Top = t;
                t += cueListSpacing;
            }

            CueList.VerticalScroll.Enabled = true;
            CueList.VerticalScroll.Visible = true;
            CueList.VerticalScroll.Minimum = 0;
            CueList.VerticalScroll.Maximum = (CueList.Controls.Count - TOP_PLACEHOLDERS  - BottomPlaceholders) * cueListSpacing - 1;
            CueList.VerticalScroll.LargeChange = cueListSpacing;
            CueList.VerticalScroll.SmallChange = cueListSpacing;

        }

        private void CueList_Resize(object sender, EventArgs e) {
            int cuelistrows;
            Debug.WriteLine("CueList_Resize");
            cuelistrows = ((this.Height - cuelistFormSpacing) / cueListSpacing);
            CueList.Height = cuelistrows * cueListSpacing - 8;
            BottomPlaceholders = cuelistrows - 1 - TOP_PLACEHOLDERS;
            //this.statusBar.Panels[0].Text = "NumberOfPlaceholders = " + (BottomPlaceholders + TOP_PLACEHOLDERS).ToString();
            PadCueList();
        }

        private void CueList_Scroll(object sender, ScrollEventArgs e) {
            //Debug.WriteLine("Scroll");
            //Debug.WriteLine("Type " + e.Type);
            //Debug.WriteLine("Orientation " + e.ScrollOrientation);
            //Debug.WriteLine("Old=" + e.OldValue);
            //Debug.WriteLine("New=" + e.NewValue);
            //Debug.WriteLine("Min " + CueList.VerticalScroll.Minimum);
            //Debug.WriteLine("Max " + CueList.VerticalScroll.Maximum);
            //Debug.WriteLine("Lg " + CueList.VerticalScroll.LargeChange);
            //Debug.WriteLine("Sm " + CueList.VerticalScroll.SmallChange);
            ////((System.Windows.Forms.FlowLayoutPanel..ScrollBar)sender).Value = e.NewValue;
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
        }

        private void button1_Click(object sender, EventArgs e) {
            StopAll(sender, e);
        }

        private void button2_Click(object sender, EventArgs e) {
            PreloadAll(sender, e);
        }

        private void timer1_Tick(object sender, EventArgs e) {
            foreach (PlayStrip Player in CueList.Controls) {
                if (Player.IsPlaying) {
                    Player.ProgressUpdate(sender, e);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e) {
            int cue = CueList.VerticalScroll.Value / cueListSpacing;
            int NewValue = (cue + 1) * cueListSpacing;
            CueList.VerticalScroll.Value = Math.Min(NewValue, CueList.VerticalScroll.Maximum);
            ReportStatus("Playing Cue " + (cue + 1).ToString() + ", " + NewValue.ToString("D"));
        }

        private void bnAddCue_Click(object sender, EventArgs e) {
            CurrentShow.AddCue(new SFX(), CurrentIndex+1);
        }
    }
}
