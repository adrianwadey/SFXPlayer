﻿using AJW.General;
using static AJW.General.SVGResources;
using NAudio.CoreAudioApi;
using NAudio.CoreAudioApi.Interfaces;
using NAudio.Midi;
using NAudio.Wave;
using SFXPlayer.Properties;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
using System.Management;
using System.Runtime.InteropServices;
using Svg.FilterEffects;

namespace SFXPlayer
{
    public partial class SFXPlayer : Form
    {
        const int TOPPLACEHOLDER = -1;
        const int BOTTOMPLACEHOLDER = -2;
        const string FileExtensions = "SFX Cue Files (*.sfx)|*.sfx";
        private bool InitialisingDevices = false;
        private XMLFileHandler<Show> ShowFileHandler = new XMLFileHandler<Show>();
        private const int WM_DEVICECHANGE = 0x0219;

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_DEVICECHANGE)
            {
                DeviceChangeTimer.Stop();       //force a restart
                DeviceChangeTimer.Start();
            }
            base.WndProc(ref m);
        }

        private Show _CurrentShow;
        private Show CurrentShow
        {
            get
            {
                return _CurrentShow;
            }
            set
            {
                if (_CurrentShow != null)
                {
                    _CurrentShow.ShowFileBecameDirty -= ShowFileHandler.SetDirty;
                    _CurrentShow.UpdateShow -= UpdateDisplay;
                    foreach (SFX sfx in _CurrentShow.Cues)
                    {
                        sfx.SFXBecameDirty -= ShowFileHandler.SetDirty;
                    }
                }
                _CurrentShow = value;
                ResetDisplay();
                if (_CurrentShow != null)
                {
                    _CurrentShow.ShowFileBecameDirty += ShowFileHandler.SetDirty;
                    _CurrentShow.UpdateShow += UpdateDisplay;
                    foreach (SFX sfx in _CurrentShow.Cues)
                    {
                        sfx.SFXBecameDirty += ShowFileHandler.SetDirty;
                    }
                }
            }
        }
        private readonly int CueListSpacing = new PlayStrip().Height + new Spacer().Height;
        private readonly int PlayStripControlHeight = new PlayStrip().Height;
        private readonly int SpacerControlHeight = new Spacer().Height;
        private readonly int TOPGAP = 5 * (new PlayStrip().Height + new Spacer().Height);
        private readonly ObservableCollection<string> PlayDevices = new ObservableCollection<string>();
        private readonly ObservableCollection<string> PreviewDevices = new ObservableCollection<string>();
        private MMDeviceEnumerator deviceEnum = new MMDeviceEnumerator();
        public static Control lastFocused;
        string[] filters;

        public SFXPlayer()
        {
            // Initialize Forms Designer generated code.
            InitializeComponent();

            // Set up the status bar and other controls.
            InitializeControls();

            DeviceChangeTimer.Start();      //trigger a reload of the available audio/MIDI devices
        }

        // Sets up the status bar and other controls.
        private void InitializeControls()
        {
            // Set up the status bar.
            //CurrentShow.Panel = CueList;
            //StatusBarPanel panel = new StatusBarPanel();
            //panel.BorderStyle = StatusBarPanelBorderStyle.Sunken;
            //panel.Text = "Ready.";
            //panel.AutoSize = StatusBarPanelAutoSize.Spring;
            //this.statusBar.ShowPanels = true;
            //this.statusBar.Panels.Add(panel);
            //cbPlayback.DataSource = PlayDevices;
            //cbPreview.DataSource = PreviewDevices;


            ShowFileHandler.FileExtensions = FileExtensions;
            //cuelistFormSpacing = this.Height - CueList.Height;
            bnStopAll.Top = bnPrev.Top = CueList.Top + TOPGAP - bnPrev.Height;
            bnPlayNext.Top = CueList.Top + TOPGAP;
            bnPlayNext.Height = PlayStripControlHeight;
            bnPlayNext.BackColor = Settings.Default.ColourPlayerPlay;
            pictureBox1.BackColor = Settings.Default.ColourPlayerPlay;
            pictureBox2.BackColor = Settings.Default.ColourPlayerPlay;
            pictureBox1.Top = bnPlayNext.Top - pictureBox1.Height;
            pictureBox2.Top = bnPlayNext.Bottom;
            bnDeleteCue.Top = bnAddCue.Top = bnPlayNext.Top + (bnPlayNext.Height - bnAddCue.Height) / 2;
            bnNext.Top = CueList.Top + TOPGAP + bnPlayNext.Height;
            bnStopAll.Height = bnNext.Top + bnNext.Height - bnStopAll.Top;
            //bnNext.Height = PlayStripControlHeight;
            rtPrevMainText.Height = bnStopAll.Top - bnStopAll.Margin.Top - rtPrevMainText.Margin.Bottom - rtPrevMainText.Top;

            rtMainText.Top = bnStopAll.Bottom + rtMainText.Margin.Top + bnStopAll.Margin.Bottom;
            rtMainText.Height = Math.Min(statusStrip.Top - rtMainText.Margin.Bottom - rtMainText.Top, rtPrevMainText.Height);
            PlayStrip.OFD = dlgOpenAudioFile;
            //PlayStrip.Devices = cbPlayback;
            //PlayStrip.PreviewDevices = cbPreview;
            autoLoadLastsfxCuelistToolStripMenuItem.Checked = Settings.Default.AutoLoadLastSession;
        }

        // Convenience method for setting message text in 
        // the status bar.
        public void ReportStatus(string statusMessage)
        {
            // If the caller passed in a message...

            if (this.statusStrip.InvokeRequired)
            {
                Action<string> d = new Action<string>(ReportStatus);
                this.Invoke(d, new object[] { statusMessage });
            }
            else
            {
                //if (string.IsNullOrEmpty(statusMessage)) {

                //} else {
                this.statusBar.Text = statusMessage;
                //Status.Text = statusMessage;
                //statusBar.Refresh();
                //}
            }
        }

        public PlayStrip PrevPlayCue
        {
            get
            {
                Point pt = new Point(0, bnPlayNext.Top - CueList.Top - CueListSpacing);
                Control ctl = CueList.GetChildAtPoint(pt);
                if (ctl is Spacer)
                {
                    int row = CueList.GetRow(ctl);
                    ctl = CueList.GetControlFromPosition(0, Math.Max(0, row - 1));
                }
                return ctl as PlayStrip;
            }
        }

        public PlayStrip NextPlayCue
        {
            get
            {
                Point pt = new Point(0, bnPlayNext.Top - CueList.Top);
                Control ctl = CueList.GetChildAtPoint(pt);
                if (ctl is Spacer)
                {
                    int row = CueList.GetRow(ctl);
                    ctl = CueList.GetControlFromPosition(0, Math.Max(0, row - 1));
                }
                return ctl as PlayStrip;
            }
        }

        public int NextPlayCueIndex
        {
            get
            {
                return -CueList.AutoScrollPosition.Y / (CueListSpacing);
            }
            set
            {
                int NewValue = value * (CueListSpacing);
                NewValue = Math.Min(NewValue, CueList.VerticalScroll.Maximum);
                Point NewPos = new Point(0, NewValue);
                CueList.AutoScrollPosition = NewPos;
                CueList.AutoScrollPosition = NewPos;        //verticalscroll update only worked when called twice, need to check this.
                NextPlayCueChanged();
            }
        }

        private void NextPlayCueChanged()
        {
            rtPrevMainText.TextChanged -= rtPrevMainText_TextChanged;
            if (PrevPlayCue != null)
            {
                rtPrevMainText.Text = PrevPlayCue.SFX.MainText;
                rtPrevMainText.ReadOnly = false;
            }
            else
            {
                rtPrevMainText.Text = "";
                rtPrevMainText.ReadOnly = true;
            }
            rtPrevMainText.TextChanged += rtPrevMainText_TextChanged;
            rtMainText.TextChanged -= rtMainText_TextChanged;
            if (NextPlayCue != null)
            {
                rtMainText.Text = NextPlayCue.SFX.MainText;
                rtMainText.ReadOnly = false;
            }
            else
            {
                rtMainText.Text = "";
                rtMainText.ReadOnly = true;
            }
            rtMainText.TextChanged += rtMainText_TextChanged;
            CurrentShow.NextPlayCueIndex = NextPlayCueIndex;
            UpdateWebApp();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Debug.WriteLine("Form1_Load");
            StartWebApp();
            ShowFileHandler.FileTitleUpdate += UpdateTitleBar;
            //Insert = new PlayStrip() { Width = 100, BackColor = Color.Blue, isPlaceholder = false };
            bnMIDI.Image = FromSvgResource("midi_port_icon_135398.svg");
            bnPreview.Image = FromSvgResource("headphones.svg");
            bnPlayback.Image = FromSvgResource("volume-up-fill.svg");
            UpdateDevices();

            ProgressTimer.Enabled = true;
            string[] args = Environment.GetCommandLineArgs();
            foreach (string cmd in args)
            {
                Debug.WriteLine(cmd);
            }
            FileNew();
            string StartFile = "";
            if (args.Length == 2)
            {
                StartFile = args[1];
            }
            else
            {
                if (autoLoadLastsfxCuelistToolStripMenuItem.Checked)
                {
                    StartFile = Settings.Default.LastSession;
                }
            }
            if (File.Exists(StartFile))
            {
                Show newShow;
                newShow = ShowFileHandler.LoadFromFile(StartFile);
                if (newShow != null)
                {
                    int tempNextPlayCueIndex = newShow.NextPlayCueIndex;
                    CurrentShow = newShow;
                    NextPlayCueIndex = tempNextPlayCueIndex;
                }
            }
            ResetDisplay();
            PreloadAll();

            //Form1_Resize(this, new EventArgs());
            //MouseWheel += CueList_MouseWheel;
            CueList.MouseWheel += CueList_MouseWheel;     //used to reset scrolling to whole units
            //CueList.ControlAdded += CueList_ControlAdded;


            FocusTrackLowestControls(Controls);     //used for pop-up volume control
            //ShowContainerControls(Controls);
            UpdateWebApp();
        }

        List<string> AudioOutDevices = new List<string>();
        List<string> CurrentAudioOutDevices = new List<string>();
        List<string> CurrentMIDIOutDevices = new List<string>();
        public static int CurrentPlaybackDeviceIdx = -1;
        public static int CurrentPreviewDeviceIdx = -1;
        public static int CurrentMIDIDeviceIdx = -1;

        private void UpdateDevices()
        {
            //something changed, so get a new list of devices
            //check whether output audio device is still in the list

            //WaveOut gives us truncated names but the index we need to open the device
            //combine these with the full names from mmdevices to populate the device list
            //add/remove devices so that we don't reset the list

            AudioOutDevices.Clear();
            {   //fill in AudioOutDevices
                List<WaveOutCapabilities> WaveOutDevices = new List<WaveOutCapabilities>();

                for (int n = 0; n < WaveOut.DeviceCount; n++)
                {
                    WaveOutDevices.Add(WaveOut.GetCapabilities(n));
                }

                var mmdeviceCollection = deviceEnum.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active);
                foreach (var device in WaveOutDevices)
                {
                    string FN = mmdeviceCollection.Where(d => d.FriendlyName.Contains(device.ProductName)).Select(dd => dd.FriendlyName).FirstOrDefault();
                    if (string.IsNullOrEmpty(FN)) FN = device.ProductName;
                    AudioOutDevices.Add(FN);
                    Debug.WriteLine(FN);
                }
            }

            if (!AudioOutDevices.SequenceEqual(CurrentAudioOutDevices))
            {
                CurrentAudioOutDevices = new List<string>(AudioOutDevices);
                cbPlayback.Items.Clear();
                cbPlayback.Items.AddRange(CurrentAudioOutDevices.ToArray());
                cbPreview.Items.Clear();
                cbPreview.Items.AddRange(CurrentAudioOutDevices.ToArray());
            }

            //MAIN OUTPUT
            int currentSelection = CurrentPlaybackDeviceIdx;
            if (CurrentAudioOutDevices.Contains(Settings.Default.LastPlaybackDevice))
            {
                CurrentPlaybackDeviceIdx = CurrentAudioOutDevices.IndexOf(Settings.Default.LastPlaybackDevice);
            }
            else
            {
                CurrentPlaybackDeviceIdx = -1;      //device not available
            }
            if (CurrentPlaybackDeviceIdx >= 0)
            {
                //Handle any device change stuff here!
                bnPlayback.BackColor = Color.FromArgb(0, 192, 0);
                bnPlayback.ToolTipText = Settings.Default.LastPlaybackDevice;
                cbPlayback.SelectedIndex = CurrentPlaybackDeviceIdx;
            }
            else
            {
                //Handle any device change stuff here!
                bnPlayback.BackColor = Color.Red;
                bnPlayback.ToolTipText = $"Not connected ({Settings.Default.LastPlaybackDevice})";
                cbPlayback.SelectedIndex = CurrentPlaybackDeviceIdx;
            }
            if (currentSelection != CurrentPlaybackDeviceIdx)
            {
                //update playstrips with new output device
                PreloadAll();
            }

            //PREVIEW OUTPUT
            currentSelection = CurrentPreviewDeviceIdx;
            if (CurrentAudioOutDevices.Contains(Settings.Default.LastPreviewDevice))
            {
                CurrentPreviewDeviceIdx = CurrentAudioOutDevices.IndexOf(Settings.Default.LastPreviewDevice);
            }
            else
            {
                CurrentPreviewDeviceIdx = -1;      //device not available
            }
            if (CurrentPreviewDeviceIdx >= 0)
            {
                //Handle any device change stuff here!
                bnPreview.BackColor = Color.FromArgb(0, 192, 0);
                bnPreview.ToolTipText = Settings.Default.LastPreviewDevice;
                cbPreview.SelectedIndex = CurrentPreviewDeviceIdx;
            }
            else
            {
                //Handle any device change stuff here!
                bnPreview.BackColor = Color.Red;
                bnPreview.ToolTipText = $"Not connected ({Settings.Default.LastPreviewDevice})";
                cbPreview.SelectedIndex = CurrentPreviewDeviceIdx;
            }
            if (currentSelection != CurrentPreviewDeviceIdx)
            {
                //update playstrips with new preview device
                //not needed
            }

            var MIDIOutDevices = new List<string>();
            for (int i = 0; i < MidiOut.NumberOfDevices; i++)
            {
                MIDIOutDevices.Add(MidiOut.DeviceInfo(i).ProductName);
                Debug.WriteLine(MidiOut.DeviceInfo(i).ProductName);
            }

            if (!MIDIOutDevices.SequenceEqual(CurrentMIDIOutDevices))
            {
                CurrentMIDIOutDevices = new List<string>(MIDIOutDevices);
                cbMIDI.Items.Clear();
                cbMIDI.Items.AddRange(CurrentMIDIOutDevices.ToArray());
            }

            //MIDI OUTPUT
            currentSelection = CurrentMIDIDeviceIdx;
            if (CurrentMIDIOutDevices.Contains(Settings.Default.LastMidiDevice))
            {
                CurrentMIDIDeviceIdx = CurrentMIDIOutDevices.IndexOf(Settings.Default.LastMidiDevice);
            }
            else
            {
                CurrentMIDIDeviceIdx = -1;      //device not available
            }

            if (CurrentMIDIDeviceIdx >= 0)
            {
                //Handle any device change stuff here!
                bnMIDI.BackColor = Color.FromArgb(0, 192, 0);
                bnMIDI.ToolTipText = Settings.Default.LastMidiDevice;
                cbMIDI.SelectedIndex = CurrentMIDIDeviceIdx;
            }
            else
            {
                //Handle any device change stuff here!
                bnMIDI.BackColor = Color.Red;
                bnMIDI.ToolTipText = $"Not connected ({Settings.Default.LastMidiDevice})";
                cbMIDI.SelectedIndex = CurrentMIDIDeviceIdx;
            }
            if (currentSelection != CurrentMIDIDeviceIdx)
            {
                //update playstrips with new MIDI Out device
                if (CurrentMIDIDeviceIdx == -1)
                {
                    if (MIDIOut != null)
                    {
                        MIDIOut.Close();
                        MIDIOut.Dispose();
                        MIDIOut = null;
                    }
                }
                if (CurrentMIDIDeviceIdx != -1)
                {
                    MIDIOut = new MidiOut(CurrentMIDIDeviceIdx);
                }
            }
            InitialisingDevices = false;
        }
        public MidiOut MIDIOut;
        private void StartWebApp()
        {
            Debug.WriteLine("MouseWheelScrollLines = " + SystemInformation.MouseWheelScrollLines);
            WebApp.Start();
            string localIP;
            using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
            {
                socket.Connect("8.8.8.8", 65530);
                IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
                localIP = endPoint.Address.ToString();
            }

            if (WebApp.Serving)
            {
                WebLink.IsLink = true;
                WebLink.Text = "http://" + localIP + ":" + WebApp.wsPort + "/";
            }
            else
            {
                WebLink.IsLink = false;
                WebLink.Text = "Web-App not available (run as admin)";
            }
        }

        private void AudioDeviceNotifications_AudioDevicesChanged(object sender, EventArgs e)
        {
            PlayDevices.Clear();
            PreviewDevices.Clear();
            var mmdeviceCollection = deviceEnum.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active);
            {
                foreach (var device in mmdeviceCollection)
                {
                    PlayDevices.Add(device.FriendlyName);
                    PreviewDevices.Add(device.FriendlyName);
                    Debug.WriteLine(device.ToString());
                }
            }


        }

        private void UpdateTitleBar(object sender, EventArgs e)
        {
            string Title;
            Title = ShowFileHandler.DisplayFileName;
            if (ShowFileHandler.Dirty) Title += "*";
            Title += " - ";
            Title += Application.ProductName;
            Text = Title;
        }

        private void ShowContainerControls(Control.ControlCollection controls)
        {
            foreach (Control ctl in controls)
            {
                if (ctl.Controls.Count > 0)
                {
                    Debug.WriteLine("  ** {0} {1}", ctl.Name, ctl);
                    ShowContainerControls(ctl.Controls);
                }
            }
        }

        private void FocusTrackLowestControls(Control control)
        {
            if (control.Controls.Count > 0)
            {
                FocusTrackLowestControls(control.Controls);
            }
            else
            {
                control.Enter += TrackFocused;
                //Debug.WriteLine(" *+ {0} {1}", control.Name, control);
            }
        }

        private void FocusTrackLowestControls(Control.ControlCollection controls)
        {
            foreach (Control ctl in controls)
            {
                if (ctl.Controls.Count > 0)
                {
                    FocusTrackLowestControls(ctl.Controls);
                }
                else
                {
                    ctl.Enter += TrackFocused;
                    //Debug.WriteLine(" *+ {0} {1}", ctl.Name, ctl);
                }
            }
        }

        private void FocusUntrackLowestControls(Control control)
        {
            if (control.Controls.Count > 0)
            {
                FocusUntrackLowestControls(control.Controls);
            }
            else
            {
                control.Enter -= TrackFocused;
                //Debug.WriteLine(" *- {0} {1}", control.Name, control);
            }
        }

        private void FocusUntrackLowestControls(Control.ControlCollection controls)
        {
            foreach (Control ctl in controls)
            {
                if (ctl.Controls.Count > 0)
                {
                    FocusUntrackLowestControls(ctl.Controls);
                }
                else
                {
                    ctl.Enter -= TrackFocused;
                    //Debug.WriteLine(" *- {0} {1}", ctl.Name, ctl);
                }
            }
        }

        //private void CueList_ControlAdded(object sender, ControlEventArgs e) {
        //    e.Control.MouseWheel += CueList_MouseWheel;
        //}

        private void CueList_MouseWheel(object sender, MouseEventArgs e)
        {
            //e = new HandledMouseEventArgs(e.Button, e.Clicks, e.X, e.Y, e.Delta, true);
            //ReportStatus(((Control)sender).Name + " MouseWheel " + e.Delta.ToString("D"));
            Debug.WriteLine(((Control)sender).Name + " MouseWheel " + e.Delta.ToString("D"));
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

        void FileNew()
        {
            CurrentShow = new Show();
            ShowFileHandler.NewFile();
        }

        void FileOpen()
        {
            Show newShow;
            newShow = ShowFileHandler.LoadFromFile();
            if (newShow != null)
            {
                int tempNextPlayCueIndex = newShow.NextPlayCueIndex;
                CurrentShow = newShow;
                NextPlayCueIndex = tempNextPlayCueIndex;
            }
        }

        void FileOpen(string FileName)
        {
            Show oldShow = CurrentShow;
            if (oldShow != null) oldShow.ShowFileBecameDirty -= ShowFileHandler.SetDirty;
            CurrentShow = ShowFileHandler.LoadFromFile(FileName);
            if (CurrentShow != null)
            {
                int tempNextPlayCueIndex = CurrentShow.NextPlayCueIndex;
                CurrentShow.UpdateShow += UpdateDisplay;
                ResetDisplay();
                NextPlayCueIndex = tempNextPlayCueIndex;
            }
            else
            {
                CurrentShow = oldShow;
            }
            CurrentShow.ShowFileBecameDirty += ShowFileHandler.SetDirty;
        }

        void UpdateDisplay()
        {
        }

        void ResetDisplay()
        {
            ShowFileHandler.PushDirty();
            CueList.SuspendLayout();
            CueList.Controls.Clear();
            CueList.RowStyles.Clear();
            CueList.RowCount = 0;
            //add padding and spacers top and bottom
            Spacer sp;
            sp = new Spacer { Width = CueList.ClientSize.Width, Name = "Top" };
            sp.Paint += Highlight_Paint;
            //sp.BackColor = Color.LightCoral;
            CueList.RowCount++;
            CueList.Controls.Add(sp, 0, 0);
            sp = new Spacer { Width = CueList.ClientSize.Width, Name = "Top spacer" };
            sp.Paint += Highlight_Paint;
            //sp.BackColor = Color.LightSteelBlue;
            CueList.RowCount++;
            CueList.Controls.Add(sp, 0, 1);

            if (CurrentShow != null)
            {
                foreach (SFX sfx in CurrentShow.Cues)
                {
                    AddPlaystrip(sfx, CurrentShow.Cues.IndexOf(sfx));
                }
            }

            sp = new Spacer { Width = CueList.ClientSize.Width, Name = "Bottom" };
            sp.Paint += Highlight_Paint;
            //sp.BackColor = Color.LightSeaGreen;
            CueList.RowCount++;
            CueList.Controls.Add(sp, 0, CueList.RowCount - 1);

            CueList.ResumeLayout();
            PadCueList();
            if (CurrentShow != null)
            {
                NextPlayCueChanged();
            }

            CueList.VerticalScroll.SmallChange = CueListSpacing;
            CueList.VerticalScroll.LargeChange = 3 * CueList.VerticalScroll.SmallChange;
            ShowFileHandler.PopDirty();
            UpdateTitleBar(this, new EventArgs());
        }

        /// <summary>
        /// Add Playstrip to the CueList control
        /// </summary>
        /// <param name="sfx">null = placeholder</param>
        /// <param name="rowIndex">sfx Index or TOPPLACEHOLDER or BOTTOMPLACEHOLDER</param>
        private void AddPlaystrip(SFX sfx, int cueIndex)
        {
            PlayStrip ps = new PlayStrip(sfx) { Width = CueList.ClientSize.Width, PlayStripIndex = cueIndex };
            Spacer sp = new Spacer { Width = CueList.ClientSize.Width };
            sp.Paint += Highlight_Paint;
            int rowIndex = TableRowFromCueIndex(cueIndex);
            CueList.RowCount++;
            CueList.Controls.Add(ps, 0, rowIndex);
            rowIndex++;
            CueList.RowCount++;
            CueList.Controls.Add(sp, 0, rowIndex);
        }

        private PlayStrip InsertPlaystrip(SFX sfx, int cueIndex)
        {
            PlayStrip ps;
            CueList.SuspendLayout();
            int rowIndex = TableRowFromCueIndex(cueIndex);
            CueList.RowCount += 2;      //add the 2 new rows
            foreach (Control ctl in CueList.Controls)
            {
                if (CueList.GetRow(ctl) >= rowIndex)
                {
                    CueList.SetRow(ctl, CueList.GetRow(ctl) + 2);
                    ps = ctl as PlayStrip;
                    if (ps != null)
                    {
                        ps.PlayStripIndex += 1;
                    }
                }
            }
            ps = new PlayStrip(sfx) { Width = CueList.ClientSize.Width, PlayStripIndex = cueIndex };
            Spacer sp = new Spacer { Width = CueList.ClientSize.Width };
            CueList.Controls.Add(ps, 0, rowIndex);
            CueList.Controls.Add(sp, 0, rowIndex + 1);
            CueList.ResumeLayout();
            return ps;
        }

        private void RemovePlaystrip(int cueIndex)
        {
            int rowIndex = TableRowFromCueIndex(cueIndex);
            CueList.SuspendLayout();
            CueList.Controls.Remove(CueList.GetControlFromPosition(0, rowIndex));
            CueList.Controls.Remove(CueList.GetControlFromPosition(0, rowIndex + 1));
            foreach (Control ctl in CueList.Controls)
            {
                if (CueList.GetRow(ctl) >= rowIndex)
                {
                    CueList.SetRow(ctl, CueList.GetRow(ctl) - 2);
                    PlayStrip ps = ctl as PlayStrip;
                    if (ps != null)
                    {
                        ps.PlayStripIndex -= 1;
                    }
                }
            }
            CueList.RowCount -= 2;
            CueList.ResumeLayout();
        }

        private static int TableRowFromCueIndex(int rowIndex)
        {
            return 2 * (rowIndex) + 2;  //+2 is spacers at top
        }

        private void StopAll(object sender, EventArgs e)
        {
            foreach (PlayStrip Player in CueList.Controls.OfType<PlayStrip>())
            {
                Player.StopOthers(sender, e);
            }
        }

        private void StopPreviews(object sender, EventArgs e)
        {
            foreach (PlayStrip Player in CueList.Controls.OfType<PlayStrip>())
            {
                Player.StopPreviews(sender, e);
            }
        }

        private void PreloadAll()
        {
            Cursor prev = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;
            foreach (PlayStrip Player in CueList.Controls.OfType<PlayStrip>())
            {
                Player.PreloadFile();
            }
            Cursor.Current = prev;
        }

        /// <summary>
        /// Add invisible CueList controls to set the correct scroll limits
        /// </summary>
        private void PadCueList()
        {
            Debug.WriteLine("PadCueList");
            if (CueList.GetControlFromPosition(0, 0) == null) return;
            CueList.GetControlFromPosition(0, 0).Height = TOPGAP - SpacerControlHeight;
            CueList.GetControlFromPosition(0, CueList.RowCount - 1).Height = CueList.ClientSize.Height - TOPGAP;
            //BottomPlaceholders = CueList.Height / (cueListSpacing) - TOP_PLACEHOLDERS + 1;
            //while (paddedTop > TOP_PLACEHOLDERS) {
            //    RemovePlaystrip(TOPPLACEHOLDER);
            //    paddedTop--;
            //}
            //while (paddedTop < TOP_PLACEHOLDERS) {
            //    AddPlaystrip(null, TOPPLACEHOLDER);
            //    paddedTop++;
            //}
            //while (paddedBottom > BottomPlaceholders) {
            //    RemovePlaystrip(BOTTOMPLACEHOLDER);
            //    paddedBottom--;
            //}
            //while (paddedBottom < BottomPlaceholders) {
            //    AddPlaystrip(null, BOTTOMPLACEHOLDER);
            //    paddedBottom++;
            //}

            //CueList.VerticalScroll.Enabled = true;
            //CueList.VerticalScroll.Visible = true;
            //CueList.VerticalScroll.Minimum = 0;
            //CueList.VerticalScroll.Maximum = (CueList.Controls.Count - TOP_PLACEHOLDERS - BottomPlaceholders) * cueListSpacing - 1;
            //CueList.VerticalScroll.LargeChange = cueListSpacing;
            //CueList.VerticalScroll.SmallChange = cueListSpacing;
            //CueList.AutoScrollOffset = new Point(0, TOP_PLACEHOLDERS * cueListSpacing);
            foreach (Control ctl in CueList.Controls)
            {
                //Debug.WriteLine(ctl.ToString());
            }

            //UpdateDisplayedIndexes();
        }

        private void CueList_Scroll(object sender, ScrollEventArgs e)
        {
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
            //    ", Min " + CueList.AutoScrollPosition.Minimum +
            //    ", Max " + CueList.AutoScrollPosition.Maximum +
            //    ", Lg " + CueList.AutoScrollPosition.LargeChange +
            //    ", Sm " + CueList.AutoScrollPosition.SmallChange
            //);
            if (e.NewValue > e.OldValue)
            {
                e.NewValue = ((e.NewValue /*+ cueListSpacing/2*/) / CueListSpacing) * CueListSpacing;
            }
            else
            {
                e.NewValue = ((e.NewValue /*+ cueListSpacing / 2*/) / CueListSpacing) * CueListSpacing;
            }
            CueList.VerticalScroll.Value = Math.Min(CueList.VerticalScroll.Maximum, e.NewValue);
            NextPlayCueChanged();
            //ReportStatus("Scrolled to " + e.NewValue.ToString("D"));
        }

        private void bnStopAll_Click(object sender, EventArgs e)
        {
            StopAll(sender, e);
            StopPreviews(sender, e);
        }

        private void ProgressTimer_Tick(object sender, EventArgs e)
        {
            foreach (Control ctl in CueList.Controls)
            {
                if (ctl.GetType() == typeof(PlayStrip))
                {
                    if (((PlayStrip)ctl).IsPlaying)
                    {
                        ((PlayStrip)ctl).ProgressUpdate(sender, e);
                    }
                }
            }
        }

        private void bnPlayNext_Click(object sender, EventArgs e)
        {
            if (NextPlayCue != null)
            {
                NextPlayCue.Play();
                NextPlayCueIndex += 1;
            }
        }

        private void bnAddCue_Click(object sender, EventArgs e)
        {
            int newPosition = Math.Min(NextPlayCueIndex, CurrentShow.Cues.Count);
            SFX sfx = new SFX();
            InsertPlaystrip(sfx, newPosition);
            PadCueList();
            //Add to the show once the controls are in place so they can be updated
            CurrentShow.AddCue(sfx, newPosition);
            NextPlayCueChanged();
        }

        private void bnDeleteCue_Click(object sender, EventArgs e)
        {
            PlayStrip ps = NextPlayCue;
            if (ps == null) return;
            DialogResult Response = MessageBox.Show(string.Format("Delete Cue {0}?\r\n{1}", ps.PlayStripIndex + 1, ps.SFX.Description), "Cue List", MessageBoxButtons.YesNo);
            if (Response == DialogResult.Yes)
            {
                RemovePlaystrip(NextPlayCueIndex);
                CurrentShow.RemoveCue(ps.SFX);
                PadCueList();
                NextPlayCueChanged();
            }
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ShowFileHandler.CheckSave(CurrentShow) != DialogResult.OK) return;
            FileNew();
            //NextPlayCueChanged();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.TaskManagerClosing) return;    //allow close from task manager
            if (ShowFileHandler.CheckSave(CurrentShow) != DialogResult.OK)
            {
                e.Cancel = true;
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowFileHandler.Save(CurrentShow);
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowFileHandler.SaveAs(CurrentShow);
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ShowFileHandler.CheckSave(CurrentShow) != DialogResult.OK) return;
            FileOpen();
        }

        private void ScrollTimer_Tick(object sender, EventArgs e)
        {
            ScrollTimer.Enabled = false;
            CueList.VerticalScroll.Value = CueList.VerticalScroll.Value / (CueListSpacing) * (CueListSpacing);
            NextPlayCueChanged();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized) return;
            Debug.WriteLine("CueList_Resize");
            //cuelistrows = (Height - cuelistFormSpacing) / cueListSpacing;
            //CueList.Height = cuelistrows * cueListSpacing - 8;
            //this.statusBar.Panels[0].Text = "NumberOfPlaceholders = " + (BottomPlaceholders + TOP_PLACEHOLDERS).ToString();
            PadCueList();

            rtMainText.Height = Math.Min(statusStrip.Top - rtMainText.Margin.Bottom - rtMainText.Top, rtPrevMainText.Height);
            pictureBox1.Top = bnPlayNext.Top - pictureBox1.Height;
            pictureBox2.Top = bnPlayNext.Bottom;
        }

        private void CueList_ClientSizeChanged(object sender, EventArgs e)
        {

            foreach (PlayStrip ctl in CueList.Controls.OfType<PlayStrip>())
            {
                ctl.Width = CueList.ClientSize.Width - 1;
            }
            foreach (Spacer ctl in CueList.Controls.OfType<Spacer>())
            {
                ctl.Width = CueList.ClientSize.Width - 1;
            }
            //Debug.WriteLine("Client size changed {0}x{1}", CueList.Width, CueList.Height);
        }

        private void rtMainText_TextChanged(object sender, EventArgs e)
        {
            NextPlayCue.SFX.MainText = rtMainText.Text;
        }

        private void rtPrevMainText_TextChanged(object sender, EventArgs e)
        {
            PrevPlayCue.SFX.MainText = rtPrevMainText.Text;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void exportShowFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string archiveFile = CurrentShow.CreateArchive(ShowFileHandler.CurrentFileName);
            SaveFileDialog sfdArch = new SaveFileDialog();
            sfdArch.FileName = Path.GetFileName(archiveFile);
            if (Directory.Exists(Settings.Default.ArchiveFolder))
            {
                sfdArch.InitialDirectory = Settings.Default.ArchiveFolder;
            }
            else
            {
                sfdArch.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            }
            if (sfdArch.ShowDialog() == DialogResult.OK)
            {
                File.Move(archiveFile, sfdArch.FileName);
                Settings.Default.ArchiveFolder = Path.GetDirectoryName(sfdArch.FileName);
                Settings.Default.Save();
            }
            else
            {
                File.Delete(archiveFile);
            }
        }

        private void importShowFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //choose file
            if (ShowFileHandler.CheckSave(CurrentShow) != DialogResult.OK) return;
            OpenFileDialog ofdArch = new OpenFileDialog();
            if (Directory.Exists(Settings.Default.ArchiveFolder))
            {
                ofdArch.InitialDirectory = Settings.Default.ArchiveFolder;
            }
            else
            {
                ofdArch.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            }
            ofdArch.Filter = "Show Archive|*.show|All files|*.*";
            ofdArch.FileName = "";
            ofdArch.Title = "Choose show archive to extract";
            if (ofdArch.ShowDialog() == DialogResult.OK)
            {
                //choose where to put it
                FolderBrowserDialog fbdArchive = new FolderBrowserDialog();
                fbdArchive.ShowNewFolderButton = true;
                if (!string.IsNullOrEmpty(Settings.Default.LastProjectFolder))
                {
                    fbdArchive.SelectedPath = new FileInfo(Path.GetDirectoryName(Settings.Default.LastProjectFolder)).Directory.FullName;
                }
                else
                {
                    fbdArchive.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                }
                fbdArchive.Description = "Please choose a folder for the show";
                while (fbdArchive.ShowDialog() == DialogResult.OK)
                {
                    string ShowFolder = fbdArchive.SelectedPath;
                    if (new DirectoryInfo(ShowFolder).GetFiles().Count() != 0)
                    {
                        MessageBox.Show("The chosen folder is not empty, please choose another one", "Open show archive");
                        continue;
                        //"Files found = " + new DirectoryInfo(ShowFolder).GetFiles().Count() + ". Creating show folder");
                        //ShowFolder = Path.Combine(ShowFolder, Path.GetFileNameWithoutExtension(ofdArch.FileName));
                        //Directory.CreateDirectory(ShowFolder);
                    }
                    string ExtractedShow = global::SFXPlayer.Show.ExtractArchive(ofdArch.FileName, ShowFolder);
                    if (!string.IsNullOrEmpty(ExtractedShow) && File.Exists(ExtractedShow))
                    {
                        ReportStatus("Show extracted to " + ExtractedShow);
                        FileOpen(ExtractedShow);
                    }
                    break;
                }
            }
        }

        private void CueList_ControlAdded(object sender, ControlEventArgs e)
        {
            if (e.Control is PlayStrip ps)
            {
                ps.StopAll += StopAll;
                ps.ReportStatus += Ps_ReportStatus;
            }
            FocusTrackLowestControls(e.Control);
        }

        private void Ps_ReportStatus(object sender, StatusEventArgs e)
        {
            if (e.Clear)
            {
                if (statusBar.Text == e.Status)
                {
                    ReportStatus("");
                }
            }
            else
            {
                ReportStatus(e.Status);
            }
        }

        private void CueList_ControlRemoved(object sender, ControlEventArgs e)
        {
            if (e.Control is PlayStrip ps)
            {
                ps.Stop();
                ps.StopAll -= StopAll;
                ps.ReportStatus -= Ps_ReportStatus;
            }
            FocusUntrackLowestControls(e.Control);
        }

        private void TrackFocused(object sender, EventArgs e)
        {
            lastFocused = sender as Control;
            Debug.WriteLine("lastFocused = {0}", lastFocused);
        }

        private void playToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bnPlayNext_Click(sender, e);
        }

        private void stopAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bnStopAll_Click(sender, e);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                bnStopAll_Click(sender, e);
            }
        }

        private bool CheckAllFilesAreAudio(string[] files)
        {
            bool FileOK;
            foreach (string file in files)
            {
                FileOK = false;
                Console.WriteLine(file);
                foreach (string filter in filters)
                {
                    if (Path.GetExtension(file).ToUpper() == filter)
                    {
                        FileOK = true;
                        break;
                    }
                }
                if (!FileOK)
                {
                    //Debug.WriteLine("Not all files are audio");
                    return false;
                }
            }
            //Debug.WriteLine("All files are audio");
            return true;
        }

        Control LastHovered;
        Color LastColor;
        private void HighlightControl(Control ctl)
        {
            if (LastHovered == ctl) return;
            UnHighlightControl();
            if (ctl == null) return;
            LastHovered = ctl;
            LastColor = LastHovered.BackColor;
            LastHovered.BackColor = SystemColors.Highlight;
        }

        private void UnHighlightControl()
        {
            if (LastHovered != null)
            {
                LastHovered.BackColor = LastColor;
                LastHovered = null;
            }
        }


        /// <summary>True if data is suitable to drop on an existing control</summary>
        bool ReplaceOK;
        /// <summary>True if data is suitable to drop between existing controls</summary>
        bool AddOK;

        private void CueList_DragEnter(object sender, DragEventArgs e)
        {
            ReplaceOK = AddOK = false;
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                Debug.WriteLine("File(s) dragged");
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (CheckAllFilesAreAudio(files))
                {
                    AddOK = true;
                    ReplaceOK = (files.Count() == 1);
                }
            }
            else if (e.Data.GetDataPresent(typeof(PlayStrip)))
            {
                AddOK = true;
            }
        }

        bool AddZone = false;
        bool ReplaceZone = false;

        private void CueList_DragOver(object sender, DragEventArgs e)
        {
            AddZone = false;
            ReplaceZone = false;

            //find where we are
            Point CueListMousePos = ScreenToChild(new Point(e.X, e.Y), CueList);
            int Ypos = CueListMousePos.Y - CueList.AutoScrollPosition.Y;
            Control ctl = CueList.GetChildAtPoint(CueListMousePos);
            PlayStrip ps = ctl as PlayStrip;
            Spacer sp = ctl as Spacer;
            if (ps != null)
            {
                ReplaceZone = true;
            }
            else
            {
                if (e.Data.GetDataPresent(typeof(PlayStrip)))
                {
                    int newIndex = (CueListMousePos.Y - CueList.AutoScrollPosition.Y - TOPGAP) / CueListSpacing + 1;
                    newIndex = Math.Max(newIndex, 0);
                    newIndex = Math.Min(newIndex, CurrentShow.Cues.Count);
                    int src = ((PlayStrip)e.Data.GetData(typeof(PlayStrip))).PlayStripIndex;
                    if (newIndex > src) newIndex--;
                    if (newIndex == src)
                    {
                        AddZone = false;
                    }
                    else
                    {
                        AddZone = true;
                    }
                }
                else
                {
                    AddZone = true;
                }
            }
            //update the cursor
            if (AddZone && AddOK)
            {
                if (e.Data.GetDataPresent(typeof(PlayStrip)))
                {
                    e.Effect = DragDropEffects.Move;
                }
                else
                {
                    e.Effect = DragDropEffects.Copy;
                }
            }
            else if (ReplaceZone && ReplaceOK)
            {
                e.Effect = DragDropEffects.Move;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }

            //highlight the control
            if (ReplaceZone && ReplaceOK)
            {
                HighlightControl(ctl);
            }
            else if (AddZone && AddOK)
            {
                if (CueList.GetControlFromPosition(0, 0) == ctl)
                {
                    ctl = CueList.GetControlFromPosition(0, 1);  //spacer above first cue
                }
                else if (CueList.GetControlFromPosition(0, CueList.RowCount - 1) == ctl)
                {
                    ctl = CueList.GetControlFromPosition(0, CueList.RowCount - 2);  //spacer below last cue
                }
                HighlightControl(ctl);
            }
            else
            {
                UnHighlightControl();
            }
        }

        private void CueList_DragLeave(object sender, EventArgs e)
        {
            UnHighlightControl();
        }

        private void CueList_DragDrop(object sender, DragEventArgs e)
        {
            Point CueListMousePos = ScreenToChild(new Point(e.X, e.Y), CueList);
            int index = (CueListMousePos.Y - CueList.AutoScrollPosition.Y - TOPGAP) / CueListSpacing + 1;
            index = Math.Max(index, 0);
            index = Math.Min(index, CurrentShow.Cues.Count);

            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                Debug.WriteLine("File(s) dropped");
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                PlayStrip ps = LastHovered as PlayStrip;
                if (ps != null)
                {
                    //we're over a PlayStrip and there's only one file
                    string msg = "do you wish to replace the file" + Environment.NewLine;
                    msg += ps.SFX.ShortFileNameOnly + Environment.NewLine;
                    msg += "with" + Environment.NewLine;
                    msg += Path.GetFileName(files[0]) + Environment.NewLine;
                    msg += "in cue " + (ps.PlayStripIndex + 1).ToString("D3") + "?" + Environment.NewLine;
                    if (string.IsNullOrEmpty(ps.SFX.FileName) ||
                        MessageBox.Show(msg, "Replace File", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                    {
                        Debug.WriteLine("replacing file at index {0}", ps.PlayStripIndex);
                        ps.SelectFile(files[0]);
                        //ps.PreloadFile(this, new EventArgs());
                        ps = null;     //backcolor reset by loading above
                    }
                }
                else if (AddZone)
                {
                    //insert new cue for each file
                    //foreach (string file in files) Console.WriteLine(file);
                    //Debug.WriteLine("Inserting {0} cues starting at index {1}", files.Count(), index + 1);
                    foreach (string file in files)
                    {
                        SFX sfx = new SFX();
                        InsertPlaystrip(sfx, index).SelectFile(file);
                        //ps.PreloadFile(this, new EventArgs());
                        CurrentShow.AddCue(sfx, index++);
                    }
                    PadCueList();
                    NextPlayCueChanged();
                }
            }
            else if (e.Data.GetDataPresent(typeof(PlayStrip)))
            {
                if (AddZone)
                {
                    PlayStrip ps = ((PlayStrip)e.Data.GetData(typeof(PlayStrip)));
                    int src = ps.PlayStripIndex;
                    int dest = index;
                    if (dest > src) dest--;
                    if (dest != src)
                    {
                        Debug.WriteLine("Moving PlayStrip[{0}] to index {1}", src, dest);
                        CurrentShow.Cues.Move(src, dest);
                        RemovePlaystrip(src);
                        InsertPlaystrip(ps.SFX, dest);
                    }
                    else
                    {
                        Debug.WriteLine("Same position");
                    }
                }
            }
            UnHighlightControl();
        }

        //don't think this gets hit!
        private void CueList_MouseDown(object sender, MouseEventArgs e)
        {
            Point CueListMousePos = ScreenToChild(e.Location, CueList);
            Control selectedControl = CueList.GetChildAtPoint(CueListMousePos);
            Debug.WriteLine("CueList_MouseDown");
            if (selectedControl != null)
            {
                if (selectedControl.GetType() == typeof(PlayStrip))
                {
                    DoDragDrop(selectedControl, DragDropEffects.Move | DragDropEffects.Scroll);
                }
                else
                {
                    Debug.WriteLine("Drag control was {0}", selectedControl.ToString());
                }
            }
        }

        private Point ScreenToChild(Point pt, Control Child)
        {
            return new Point(pt.X - RectangleToScreen(ClientRectangle).Left - Child.Left, pt.Y - RectangleToScreen(ClientRectangle).Top - Child.Top);
        }

        private void Form1_ControlAdded(object sender, ControlEventArgs e)
        {
            FocusTrackLowestControls(e.Control);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CueList.VerticalScroll.SmallChange = CueListSpacing;
            //CueList.AutoScrollOffset = new Point(0, TOPGAP);
            CueList.AutoScrollOffset = new Point(0, 0);
            CueList.AutoScrollPosition = new Point(0, TOPGAP);
            //CueList.AutoScrollMinSize = new Size(5, CueListSpacing);
            CueList.AutoScrollMinSize = new Size(0, 40);
            CueList.SetAutoScrollMargin(0, CueListSpacing);
            CueList.ScrollControlIntoView(CueList.GetControlFromPosition(0, TableRowFromCueIndex(int.Parse(((Control)sender).Text) - 1)));
        }

        private void bnPrev_Click(object sender, EventArgs e)
        {
            NextPlayCueIndex -= 1;
        }

        private void bnNext_Click(object sender, EventArgs e)
        {
            NextPlayCueIndex += 1;
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("It is recommended to disable system sounds\r\n" +
                            "right click on the windows speaker icon and choose \"Sounds\"\r\n" +
                            "then choose Sound Scheme: No Sounds", Application.ProductName);
        }

        private void autoLoadLastsfxCuelistToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Settings.Default.AutoLoadLastSession = autoLoadLastsfxCuelistToolStripMenuItem.Checked;
            Settings.Default.Save();
        }

        private void previousCueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bnPrev_Click(sender, e);
        }

        private void nextCueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bnNext_Click(sender, e);
        }

        private void Highlight_Paint(object sender, PaintEventArgs e)
        {
            base.OnPaint(e);
            PlayStrip ps = NextPlayCue;
            if (ps != null)
            {
                if (((Control)sender).Bottom == ps.Top)
                {
                    e.Graphics.DrawLine(Pens.Black, 0, ((Control)sender).Height - 1, ((Control)sender).Width, ((Control)sender).Height - 1);
                }
                if (((Control)sender).Top == ps.Bottom)
                {
                    e.Graphics.DrawLine(Pens.Black, 0, 0, ((Control)sender).Width, 0);
                }
            }
        }

        private delegate void SafeCommandDelegate();

        internal void PlayNextCue()
        {
            if (CueList.InvokeRequired)
            {
                var d = new SafeCommandDelegate(PlayNextCue);
                CueList.Invoke(d);
            }
            else
            {
                bnPlayNext_Click(null, null);
            }
        }

        internal void StopAll()
        {
            if (CueList.InvokeRequired)
            {
                var d = new SafeCommandDelegate(StopAll);
                CueList.Invoke(d);
            }
            else
            {
                bnStopAll_Click(null, null);
            }
        }

        internal void PreviousCue()
        {
            if (CueList.InvokeRequired)
            {
                var d = new SafeCommandDelegate(PreviousCue);
                CueList.Invoke(d);
            }
            else
            {
                bnPrev_Click(null, null);
            }
        }

        internal void NextCue()
        {
            if (CueList.InvokeRequired)
            {
                var d = new SafeCommandDelegate(NextCue);
                CueList.Invoke(d);
            }
            else
            {
                bnNext_Click(null, null);
            }
        }

        public event EventHandler<DisplaySettings> DisplayChanged;

        protected virtual void OnDisplayChanged(DisplaySettings e)
        {
            DisplayChanged?.Invoke(this, e);
        }

        private void UpdateWebApp()
        {
            DisplaySettings disp = new DisplaySettings()
            {
                Title = Text,
                PrevMainText = rtPrevMainText.Text,
                MainText = rtMainText.Text,
                TrackName = Path.GetFileName(NextPlayCue?.SFX.FileName)
            };
            OnDisplayChanged(disp);
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            WebApp.StopAsync();
        }

        private void WebLink_Click(object sender, EventArgs e)
        {
            Process.Start(WebLink.Text);
        }

        private void bnMIDI_Click(object sender, EventArgs e)
        {

        }

        //private void DeviceInsertedEvent(object sender, EventArrivedEventArgs e)
        //{
        //    ManagementBaseObject instance = (ManagementBaseObject)e.NewEvent["TargetInstance"];
        //    foreach (var property in instance.Properties)
        //    {
        //        Console.WriteLine(property.Name + " = " + property.Value);
        //    }
        //}

        //private void DeviceRemovedEvent(object sender, EventArrivedEventArgs e)
        //{
        //    ManagementBaseObject instance = (ManagementBaseObject)e.NewEvent["TargetInstance"];
        //    foreach (var property in instance.Properties)
        //    {
        //        Console.WriteLine(property.Name + " = " + property.Value);
        //    }
        //}

        //private void bwDeviceMonitor_DoWork(object sender, DoWorkEventArgs e)
        //{
        //    WqlEventQuery insertQuery = new WqlEventQuery("SELECT * FROM __InstanceCreationEvent WITHIN 2 WHERE TargetInstance ISA 'Win32_USBHub'");

        //    ManagementEventWatcher insertWatcher = new ManagementEventWatcher(insertQuery);
        //    insertWatcher.EventArrived += new EventArrivedEventHandler(DeviceInsertedEvent);
        //    insertWatcher.Start();

        //    WqlEventQuery removeQuery = new WqlEventQuery("SELECT * FROM __InstanceDeletionEvent WITHIN 2 WHERE TargetInstance ISA 'Win32_USBHub'");
        //    ManagementEventWatcher removeWatcher = new ManagementEventWatcher(removeQuery);
        //    removeWatcher.EventArrived += new EventArrivedEventHandler(DeviceRemovedEvent);
        //    removeWatcher.Start();

        //    // Do something while waiting for events
        //    System.Threading.Thread.Sleep(20000000);
        //}

        private void DeviceChangeTimer_Tick(object sender, EventArgs e)
        {
            DeviceChangeTimer.Enabled = false;
            //a device has changed, check the audio & midi interfaces
            Debug.WriteLine("Device Changed");
            UpdateDevices();
        }

        private void cbPlayback_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CurrentPlaybackDeviceIdx != cbPlayback.SelectedIndex)
            {
                if (cbPlayback.SelectedIndex != -1)
                {
                    if (Settings.Default.LastPlaybackDevice != (string)cbPlayback.SelectedItem)
                    {
                        Settings.Default.LastPlaybackDevice = (string)cbPlayback.SelectedItem;
                        Settings.Default.Save();
                        UpdateDevices();
                    }
                }
                else
                {
                    UpdateDevices();
                }
            }
        }

        private void cbPreview_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CurrentPreviewDeviceIdx != cbPreview.SelectedIndex)
            {
                if (cbPreview.SelectedIndex != -1)
                {
                    if (Settings.Default.LastPreviewDevice != (string)cbPreview.SelectedItem)
                    {
                        Settings.Default.LastPreviewDevice = (string)cbPreview.SelectedItem;
                        Settings.Default.Save();
                        UpdateDevices();
                    }
                }
                else
                {
                    UpdateDevices();
                }
            }
        }

        private void cbMIDI_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CurrentMIDIDeviceIdx != cbMIDI.SelectedIndex)
            {
                if (cbMIDI.SelectedIndex != -1)
                {
                    if (Settings.Default.LastMidiDevice != (string)cbMIDI.SelectedItem)
                    {
                        Settings.Default.LastMidiDevice = (string)cbMIDI.SelectedItem;
                        Settings.Default.Save();
                        UpdateDevices();
                    }
                }
                else
                {
                    UpdateDevices();
                }
            }
        }
    }

    public class DisplaySettings
    {
        [DefaultValue("")]
        public string PrevMainText = "";
        [DefaultValue("")]
        public string MainText = "";
        [DefaultValue(null)]
        public string TrackName = null;
        [DefaultValue("")]
        public string Title = "";
    }
}
