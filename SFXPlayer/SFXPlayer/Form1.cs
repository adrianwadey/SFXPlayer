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
using static System.Reflection.MethodBase;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AJW.General;
using System.IO;
using CSCore.Codecs;

namespace SFXPlayer {
    public partial class Form1 : Form {
        const string FileExtensions = "Show Files (*.sfx)|*.sfx";
        private XMLFileHandler<Show> ShowFileHandler = new XMLFileHandler<Show>();
        private Show _CurrentShow;
        private Show CurrentShow {
            get {
                return _CurrentShow;
            }
            set {
                if (_CurrentShow != null) {
                    _CurrentShow.ShowFileBecameDirty -= ShowFileHandler.SetDirty;
                    _CurrentShow.UpdateShow -= UpdateDisplay;
                    foreach (SFX sfx in _CurrentShow.Cues) {
                        sfx.SFXBecameDirty -= ShowFileHandler.SetDirty;
                    }
                }
                _CurrentShow = value;
                ResetDisplay();
                if (_CurrentShow != null) {
                    _CurrentShow.ShowFileBecameDirty += ShowFileHandler.SetDirty;
                    _CurrentShow.UpdateShow += UpdateDisplay;
                    foreach (SFX sfx in _CurrentShow.Cues) {
                        sfx.SFXBecameDirty += ShowFileHandler.SetDirty;
                    }
                }
            }
        }
        private const int cueListSpacing = 35;
        private readonly int PlayStripControlHeight = new PlayStrip().Height;
        private readonly ObservableCollection<MMDevice> _devices = new ObservableCollection<MMDevice>();
        public static Control lastFocused;
        string[] filters;

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

            ShowFileHandler.FileExtensions = FileExtensions;
            cuelistFormSpacing = this.Height - CueList.Height;
            bnPlayNext.Top = TOP_PLACEHOLDERS * cueListSpacing + CueList.Top;
            bnPlayNext.Height = new PlayStrip().Height;
            bnPlayNext.BackColor = Settings.Default.ColourPlayerPlay;
            bnDeleteCue.Top = bnAddCue.Top = bnPlayNext.Top + (bnPlayNext.Height - bnAddCue.Height) / 2;
            rtMainText.Top = bnPlayNext.Bottom + rtMainText.Margin.Top + bnPlayNext.Margin.Bottom;
            PlayStrip.OFD = dlgOpenAudioFile;
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

            //set up list of file extensions for checking dragndrop files
            filters = CodecFactory.SupportedFilesFilterEn.Split(new char[] { '|' });
            filters = filters[1].Split(new char[] { ';' });
            for (int i = 0; i < filters.Length; i++) {
                filters[i] = filters[i].Substring(1).ToUpper();
            }
        }

        // Convenience method for setting message text in 
        // the status bar.
        public void ReportStatus(string statusMessage) {
            // If the caller passed in a message...

            if (this.statusBar.InvokeRequired) {
                Action<string> d = new Action<string>(ReportStatus);
                this.Invoke(d, new object[] { statusMessage });
            } else {
                if (string.IsNullOrEmpty(statusMessage)) {

                } else {
                    this.statusBar.Panels[0].Text = statusMessage;
                }
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
            rtMainText.TextChanged -= rtMainText_TextChanged;
            rtMainText.Text = NextPlayCue.SFX.MainText;
            rtMainText.TextChanged += rtMainText_TextChanged;
            CurrentShow.NextPlayCueIndex = NextPlayCueIndex;
        }

        private void Form1_Load(object sender, EventArgs e) {
            Debug.WriteLine("Form1_Load");
            Debug.WriteLine("MouseWheelScrollLines = " + SystemInformation.MouseWheelScrollLines);

            ShowFileHandler.FileTitleUpdate += UpdateTitleBar;
            //Insert = new PlayStrip() { Width = 100, BackColor = Color.Blue, isPlaceholder = false };
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
            //CueList.ControlAdded += CueList_ControlAdded;

            FocusTrackLowestControls(Controls);
            //ShowContainerControls(Controls);
        }

        private void UpdateTitleBar(object sender, EventArgs e) {
            string Title;
            Title = ShowFileHandler.DisplayFileName;
            if (ShowFileHandler.Dirty) Title += "*";
            Title += " - ";
            Title += Application.ProductName;
            Text = Title;
        }

        private void ShowContainerControls(Control.ControlCollection controls) {
            foreach (Control ctl in controls) {
                if (ctl.Controls.Count > 0) {
                    Debug.WriteLine("  ** {0} {1}", ctl.Name, ctl);
                    ShowContainerControls(ctl.Controls);
                }
            }
        }

        private void FocusTrackLowestControls(Control control) {
            if (control.Controls.Count > 0) {
                FocusTrackLowestControls(control.Controls);
            } else {
                control.GotFocus += TrackFocused;
                //Debug.WriteLine(" *+ {0} {1}", control.Name, control);
            }
        }

        private void FocusTrackLowestControls(Control.ControlCollection controls) {
            foreach (Control ctl in controls) {
                if (ctl.Controls.Count > 0) {
                    FocusTrackLowestControls(ctl.Controls);
                } else {
                    ctl.GotFocus += TrackFocused;
                    //Debug.WriteLine(" *+ {0} {1}", ctl.Name, ctl);
                }
            }
        }

        private void FocusUntrackLowestControls(Control control) {
            if (control.Controls.Count > 0) {
                FocusUntrackLowestControls(control.Controls);
            } else {
                control.GotFocus -= TrackFocused;
                //Debug.WriteLine(" *- {0} {1}", control.Name, control);
            }
        }

        private void FocusUntrackLowestControls(Control.ControlCollection controls) {
            foreach (Control ctl in controls) {
                if (ctl.Controls.Count > 0) {
                    FocusUntrackLowestControls(ctl.Controls);
                } else {
                    ctl.GotFocus -= TrackFocused;
                    //Debug.WriteLine(" *- {0} {1}", ctl.Name, ctl);
                }
            }
        }

        //private void CueList_ControlAdded(object sender, ControlEventArgs e) {
        //    e.Control.MouseWheel += CueList_MouseWheel;
        //}

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
            CurrentShow = new Show();
            ShowFileHandler.NewFile();
        }

        void FileOpen() {
            Show newShow;
            newShow = ShowFileHandler.LoadFromFile();
            if (newShow != null) {
                int tempNextPlayCueIndex = newShow.NextPlayCueIndex;
                CurrentShow = newShow;
                NextPlayCueIndex = tempNextPlayCueIndex;
            }
        }

        void FileOpen(string FileName) {
            Show oldShow = CurrentShow;
            oldShow.ShowFileBecameDirty -= ShowFileHandler.SetDirty;
            CurrentShow = ShowFileHandler.LoadFromFile(FileName);
            if (CurrentShow != null) {
                int tempNextPlayCueIndex = CurrentShow.NextPlayCueIndex;
                CurrentShow.UpdateShow += UpdateDisplay;
                ResetDisplay();
                NextPlayCueIndex = tempNextPlayCueIndex;
            } else {
                CurrentShow = oldShow;
            }
            CurrentShow.ShowFileBecameDirty += ShowFileHandler.SetDirty;
        }

        void UpdateDisplay() {
        }

        void UpdateDisplayedIndexes() {
            //renumber the cues
            int Index = 1;
            foreach (Control ctl in CueList.Controls) {
                if (ctl.GetType() == typeof(PlayStrip)) {
                    var ps = ctl as PlayStrip;
                    if (!ps.isPlaceholder) {
                        ps.Index = Index++;
                    }
                }
            }
        }

        void ResetDisplay() {
            CueList.Controls.Clear();
            foreach (SFX sfx in CurrentShow.Cues) {
                PlayStrip ps = new PlayStrip(sfx);
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
            foreach (Control ctl in CueList.Controls) {
                //Debug.WriteLine(ctl.ToString());
            }

            UpdateDisplayedIndexes();
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

        private void bnStopAll_Click(object sender, EventArgs e) {
            StopAll(sender, e);
        }

        private void button2_Click(object sender, EventArgs e) {
            PreloadAll(sender, e);
        }

        private void ProgressTimer_Tick(object sender, EventArgs e) {
            foreach (Control ctl in CueList.Controls) {
                if (ctl.GetType() == typeof(PlayStrip)) {
                    if (((PlayStrip)ctl).IsPlaying) {
                        ((PlayStrip)ctl).ProgressUpdate(sender, e);
                    }
                }
            }
        }

        private void bnPlayNext_Click(object sender, EventArgs e) {
            NextPlayCue.Play();
            NextPlayCueIndex += 1;
        }

        private void bnAddCue_Click(object sender, EventArgs e) {
            int newPosition = Math.Min(NextPlayCueIndex, CurrentShow.Cues.Count);
            SFX sfx = new SFX();
            PlayStrip ps = new PlayStrip(sfx);
            ps.Width = CueList.ClientSize.Width;
            CueList.Controls.Add(ps);
            CueList.Controls.SetChildIndex(ps, paddedTop + newPosition);
            //CueList.Refresh();
            PadCueList();
            //Add to the show once the controls are in place so they can be updated
            CurrentShow.AddCue(sfx, newPosition);
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
            if (ShowFileHandler.CheckSave(CurrentShow) != DialogResult.OK) return;
            FileNew();
            //NextPlayCueChanged();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e) {
            if (e.CloseReason == CloseReason.TaskManagerClosing) return;    //allow close from task manager
            if (ShowFileHandler.CheckSave(CurrentShow) != DialogResult.OK) {
                e.Cancel = true;
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e) {
            ShowFileHandler.Save(CurrentShow);
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e) {
            ShowFileHandler.SaveAs(CurrentShow);
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e) {
            if (ShowFileHandler.CheckSave(CurrentShow) != DialogResult.OK) return;
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
            foreach (Control ctl in CueList.Controls) {
                if (ctl.GetType() == typeof(PlayStrip)) {
                    ctl.Width = CueList.ClientSize.Width;
                }
            }
        }

        private void rtMainText_TextChanged(object sender, EventArgs e) {
            NextPlayCue.SFX.MainText = rtMainText.Text;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e) {
            Close();
        }

        private void exportShowFileToolStripMenuItem_Click(object sender, EventArgs e) {
            string archiveFile = CurrentShow.CreateArchive(ShowFileHandler.CurrentFileName);
            SaveFileDialog sfdArch = new SaveFileDialog();
            sfdArch.FileName = Path.GetFileName(archiveFile);
            if (Directory.Exists(Settings.Default.ArchiveFolder)) {
                sfdArch.InitialDirectory = Settings.Default.ArchiveFolder;
            } else {
                sfdArch.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            }
            if (sfdArch.ShowDialog() == DialogResult.OK) {
                File.Move(archiveFile, sfdArch.FileName);
                Settings.Default.ArchiveFolder = Path.GetDirectoryName(sfdArch.FileName);
                Settings.Default.Save();
            } else {
                File.Delete(archiveFile);
            }
        }

        private void importShowFileToolStripMenuItem_Click(object sender, EventArgs e) {
            //choose file
            if (ShowFileHandler.CheckSave(CurrentShow) != DialogResult.OK) return;
            OpenFileDialog ofdArch = new OpenFileDialog();
            if (Directory.Exists(Settings.Default.ArchiveFolder)) {
                ofdArch.InitialDirectory = Settings.Default.ArchiveFolder;
            } else {
                ofdArch.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            }
            if (ofdArch.ShowDialog() == DialogResult.OK) {
                //choose where to put it
                FolderBrowserDialog fbdArchive = new FolderBrowserDialog();
                fbdArchive.ShowNewFolderButton = true;
                if (!string.IsNullOrEmpty(Settings.Default.LastProjectFolder)) {
                    fbdArchive.SelectedPath = new FileInfo(Path.GetDirectoryName(Settings.Default.LastProjectFolder)).Directory.FullName;
                } else {
                    fbdArchive.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                }
                fbdArchive.Description = "Choose Folder for Show";
                if (fbdArchive.ShowDialog() == DialogResult.OK) {
                    string ShowFolder = fbdArchive.SelectedPath;
                    if (new DirectoryInfo(ShowFolder).GetFiles().Count() != 0) {
                        ReportStatus("Files found = " + new DirectoryInfo(ShowFolder).GetFiles().Count() + ". Creating show folder");
                        ShowFolder = Path.Combine(ShowFolder, Path.GetFileNameWithoutExtension(ofdArch.FileName));
                        Directory.CreateDirectory(ShowFolder);
                    }
                    string ExtractedShow = SFXPlayer.Show.ExtractArchive(ofdArch.FileName, ShowFolder);
                    if (!string.IsNullOrEmpty(ExtractedShow) && File.Exists(ExtractedShow)) {
                        ReportStatus("Show extracted to " + ExtractedShow);
                        FileOpen(ExtractedShow);
                    }
                }
            }
        }

        private void CueList_ControlAdded(object sender, ControlEventArgs e) {
            var ctl = e.Control as PlayStrip;
            if (ctl != null) ctl.StopAll += StopAll;
            FocusTrackLowestControls(e.Control);
        }

        private void CueList_ControlRemoved(object sender, ControlEventArgs e) {
            if (e.Control is PlayStrip ps) {
                ps.Stop();
                ps.StopAll -= StopAll;
            }
            FocusUntrackLowestControls(e.Control);
        }

        private void TrackFocused(object sender, EventArgs e) {
            lastFocused = sender as Control;
            //Debug.WriteLine("lastFocused = {0}", lastFocused);
        }

        private void playToolStripMenuItem_Click(object sender, EventArgs e) {
            bnPlayNext_Click(sender, e);
        }

        private void stopAllToolStripMenuItem_Click(object sender, EventArgs e) {
            bnStopAll_Click(sender, e);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Escape) {
                bnStopAll_Click(sender, e);
            }
        }

        private bool CheckAllFilesAreAudio(string[] files) {
            bool FileOK;
            foreach (string file in files) {
                FileOK = false;
                Console.WriteLine(file);
                foreach (string filter in filters) {
                    if (Path.GetExtension(file).ToUpper() == filter) {
                        FileOK = true;
                        break;
                    }
                }
                if (!FileOK) {
                    //Debug.WriteLine("Not all files are audio");
                    return false;
                }
            }
            //Debug.WriteLine("All files are audio");
            return true;
        }

        //PlayStrip Insert;
        PlayStrip LastHovered;
        Color LastHoveredColor;
        private void HighlightControl(int index) {
            UnHighlightControl();
            LastHovered = (PlayStrip)CueList.Controls[index + TOP_PLACEHOLDERS];
            LastHoveredColor = LastHovered.BackColor;
            LastHovered.BackColor = SystemColors.Highlight;
        }

        private void UnHighlightControl() {
            if (LastHovered != null) {
                LastHovered.BackColor = LastHoveredColor;
                LastHovered = null;
            }
        }


        /// <summary>True if data is suitable to drop on an existing control</summary>
        bool ReplaceOK;
        /// <summary>True if data is suitable to drop between existing controls</summary>
        bool AddOK;

        private void CueList_DragEnter(object sender, DragEventArgs e) {
            ReplaceOK = AddOK = false;
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) {
                Debug.WriteLine("File(s) dragged");
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (CheckAllFilesAreAudio(files)) {
                    AddOK = true;
                    ReplaceOK = (files.Count() == 1);
                }
            } else if (e.Data.GetDataPresent(typeof(PlayStrip))) {
                AddOK = true;
            }
        }

        bool AddZone = false;
        bool ReplaceZone = false;

        private void CueList_DragOver(object sender, DragEventArgs e) {
            AddZone = false;
            ReplaceZone = false;

            //find where we are
            Point CueListMousePos = new Point(e.X - RectangleToScreen(ClientRectangle).Left - CueList.Left, e.Y - RectangleToScreen(ClientRectangle).Top - CueList.Top);
            int Ypos = CueListMousePos.Y - CueList.AutoScrollPosition.Y;
            
            if (Ypos < TOP_PLACEHOLDERS * cueListSpacing) {                //in dead zone at top
                AddZone = true;
            } else if (Ypos > (TOP_PLACEHOLDERS + CurrentShow.Cues.Count) * cueListSpacing) {   //in dead zone at bottom
                AddZone = true;
            } else {
                if ((Ypos % cueListSpacing) > PlayStripControlHeight) {
                    AddZone = true;
                } else {
                    ReplaceZone = true;
                }
            }
            //update the cursor
            if (AddZone && AddOK) {
                if (e.Data.GetDataPresent(typeof(PlayStrip))) {
                    e.Effect = DragDropEffects.Move;
                } else {
                    e.Effect = DragDropEffects.Copy;
                }
            } else if (ReplaceZone && ReplaceOK) {
                e.Effect = DragDropEffects.Move;
            } else {
                e.Effect = DragDropEffects.None;
            }

            //highlight the control
            if (ReplaceZone && ReplaceOK) {
                int index = Ypos / cueListSpacing - TOP_PLACEHOLDERS;
                index = Math.Max(index, 0);
                index = Math.Min(index, CurrentShow.Cues.Count);
                if (CueList.Controls[index + TOP_PLACEHOLDERS] != LastHovered) {
                    HighlightControl(index);
                }
            } else {
                UnHighlightControl();
            }
        }

        private void CueList_DragLeave(object sender, EventArgs e) {
            UnHighlightControl();
        }

        private void CueList_DragDrop(object sender, DragEventArgs e) {
            Point CueListMousePos = new Point(e.X - RectangleToScreen(ClientRectangle).Left - CueList.Left, e.Y - RectangleToScreen(ClientRectangle).Top - CueList.Top);
            int index = (CueListMousePos.Y - CueList.AutoScrollPosition.Y) / cueListSpacing - TOP_PLACEHOLDERS + 1;
            index = Math.Max(index, 0);
            index = Math.Min(index, CurrentShow.Cues.Count);

            if (e.Data.GetDataPresent(DataFormats.FileDrop)) {
                Debug.WriteLine("File(s) dropped");
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (LastHovered != null) {
                    //we're over a PlayStrip and there's only one file
                    string msg = "do you wish to replace the file" + Environment.NewLine;
                    msg += LastHovered.SFX.ShortFileName + Environment.NewLine;
                    msg += "with" + Environment.NewLine;
                    msg += Path.GetFileName(files[0]) + Environment.NewLine;
                    msg += "in cue " + LastHovered.Index.ToString("D3") + "?" + Environment.NewLine;
                    if (string.IsNullOrEmpty(LastHovered.SFX.FileName) ||
                        MessageBox.Show(msg, "Replace File", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK) {
                        Debug.WriteLine("replacing file at index {0}", LastHovered.Index);
                        LastHovered.SetFile(files[0]);
                        //LastHovered.PreloadFile(this, new EventArgs());
                        LastHovered = null;     //backcolor reset by loading above
                    } else {
                        UnHighlightControl();
                    }
                } else if (AddZone) {
                    //insert new cue for each file
                    //foreach (string file in files) Console.WriteLine(file);
                    //Debug.WriteLine("Inserting {0} cues starting at index {1}", files.Count(), index + 1);
                    foreach (string file in files) {
                        SFX sfx = new SFX { FileName = file };
                        PlayStrip ps = new PlayStrip(sfx) { Width = CueList.ClientSize.Width };
                        CueList.Controls.Add(ps);
                        CueList.Controls.SetChildIndex(ps, paddedTop + index);
                        ps.SetFile(file);
                        //ps.PreloadFile(this, new EventArgs());
                        CurrentShow.AddCue(sfx, index++);
                    }
                    PadCueList();
                    NextPlayCueChanged();
                }
            } else if (e.Data.GetDataPresent(typeof(PlayStrip))) {
                if (AddZone) {
                    PlayStrip ps = ((PlayStrip)e.Data.GetData(typeof(PlayStrip)));
                    int src = ps.Index - 1;
                    int dest = index;
                    if (dest > src) dest--;
                    if (dest == src) {
                        Debug.WriteLine("Not moving");
                        return;
                    }
                    Debug.WriteLine("Moving PlayStrip[{0}] to index {1}", src, dest);
                    CurrentShow.Cues.Move(src, dest);
                    CueList.Controls.SetChildIndex(ps, dest + paddedTop);
                    Width += 1;     //fudge to force a redraw. The proper routes don't seem to work
                    Width -= 1;     //this doesn;t work when maximized
                }
            }
        }

        //don't think this gets hit!
        private void CueList_MouseDown(object sender, MouseEventArgs e) {
            Point CueListMousePos = new Point(e.X - RectangleToScreen(ClientRectangle).Left - CueList.Left, e.Y - RectangleToScreen(ClientRectangle).Top - CueList.Top);
            Control selectedControl = CueList.GetChildAtPoint(CueListMousePos);
            Debug.WriteLine("CueList_MouseDown");
            if (selectedControl != null) {
                if (selectedControl.GetType() == typeof(PlayStrip)) {
                    DoDragDrop(selectedControl, DragDropEffects.Move | DragDropEffects.Scroll);
                } else {
                    Debug.WriteLine("Drag control was {0}", selectedControl.ToString());
                }
            }
        }
    }
}
