using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.ObjectModel;
using CSCore.CoreAudioAPI;
using AudioPlayerSample;
using CSCore.SoundOut;
using SFXPlayer.Properties;
using System.Threading;
using System.IO;
using System.Diagnostics;

namespace SFXPlayer {
    enum PlayerState {
        uninitialised = PlaybackState.Stopped,
        play = PlaybackState.Playing,
        paused = PlaybackState.Paused,
        loading = 3,
        loaded = 4,
    }

    public partial class PlayStrip : UserControl {

        private Bitmap graph;
        private readonly MusicPlayer _musicPlayer = new MusicPlayer();
        ucVolume volume = new ucVolume();
        public event EventHandler StopAll;

        private MMDevice loadedDevice;

        private PlayerState PlayerState {
            get {
                return _playerstate;
            }
            set {
                _playerstate = value;
                UpdatePlayerState(_playerstate);
            }
        }

        public PlayStrip() {
            InitializeComponent();
            InitializeSound();
            SFX = new SFX();
        }

        public PlayStrip(SFX SFX) : this() {
            this.SFX = SFX;
            UpdateFileToolTip();
        }
        // Sets up the SoundPlayer object.
        private void InitializeSound() {
            //// Create an instance of the SoundPlayer class.
            //player = new SoundPlayer();

            //// Listen for the LoadCompleted event.
            //player.LoadCompleted += new AsyncCompletedEventHandler(player_LoadCompleted);

            //// Listen for the SoundLocationChanged event.
            //player.SoundLocationChanged += new EventHandler(player_LocationChanged);

            components = new Container();
            components.Add(_musicPlayer);
            _musicPlayer.PlaybackStopped += (s, args) => {
                //WasapiOut uses SynchronizationContext.Post to raise the event
                //There might be already a new WasapiOut-instance in the background when the async Post method brings the PlaybackStopped-Event to us.
                //if (_musicPlayer.PlaybackState != PlaybackState.Stopped)
                //    btnPlay.Enabled = btnStop.Enabled = btnPause.Enabled = false;
            };
            _musicPlayer.PlaybackStopped += _musicPlayer_PlaybackStopped;
            volume.VolumeChanged += Volume_VolumeChanged;
            volume.Done += Volume_Done;
            
        }

        private void Volume_VolumeChanged(object sender, EventArgs e) {
            SFX.Volume = volume.Volume;
            _musicPlayer.Volume = SFX.Volume;
        }

        private void _musicPlayer_PlaybackStopped(object sender, PlaybackStoppedEventArgs e) {
            PlayerState = PlayerState.loaded;
        }

        public static OpenFileDialog OFD;
        public static ComboBox Devices;
        //WAVSounds ws;
        //private SoundPlayer player;
        private bool _Placeholder;
        public bool isPlaceholder {
            get {
                return _Placeholder;
            }
            set {
                _Placeholder = value;
                foreach (Control control in Controls) {
#if DEBUG
                    control.Visible = true;
                    control.BackColor = Color.Brown;
#else
                    control.Visible = !_Placeholder;
#endif
                }
            }
        }

        private void UpdatePlayerState(PlayerState newstate) {
            if (isPlaceholder) {
                BackColor = SystemColors.ControlDark;
                return;
            }
            switch (newstate) {
                case PlayerState.uninitialised:
                    BackColor = Settings.Default.ColourPlayerIdle;
                    break;
                case PlayerState.loading:
                    BackColor = Settings.Default.ColourPlayerLoading;
                    break;
                case PlayerState.loaded:
                    BackColor = Settings.Default.ColourPlayerLoaded;
                    break;
                case PlayerState.play:
                    //BackColor = Settings.Default.ColourPlayerPlay;
                    BackColor = Color.Transparent;
                    break;
                case PlayerState.paused:
                    BackColor = Settings.Default.ColourPlayerPaused;
                    break;
                default:
                    BackColor = Settings.Default.ColourPlayerIdle;
                    break;
            }
        }

        public int Index {
            get {
                return int.Parse(lbIndex.Text);
            }
            set {
                lbIndex.Text = value.ToString("D3");
            }

        }

        private SFX _SFX;
        public SFX SFX {
            get {
                return _SFX;
            }
            set {
                _SFX = value;
                tbDescription.Text = SFX.Description;
                bnStopAll.Checked = SFX.StopOthers;
            }
        }

        public bool IsPlaying => (PlayerState == PlayerState.play);

        private void tbDescription_TextChanged(object sender, EventArgs e) {
            if (isPlaceholder) return;
            SFX.Description = tbDescription.Text;
        }

        private void cbLoop_CheckedChanged(object sender, EventArgs e) {
            if (isPlaceholder) return;
            SFX.StopOthers = bnStopAll.Checked;
            //if (bnStopAll.Checked) bnStopAll.BackColor = Color.Red;
        }

        private void label1_DoubleClick(object sender, EventArgs e) {
            if (isPlaceholder) return;
            SelectFile();
        }

        private void SelectFile() {
            if (isPlaceholder) return;
            if (PlayerState == PlayerState.play) return;
            if (PlayerState == PlayerState.paused) Stop();
            OFD.Filter = "wav files|*.wav|All files|*.*";
            if (Directory.Exists(Settings.Default.LastAudioFolder)) {
                OFD.InitialDirectory = Settings.Default.LastAudioFolder;
            }
            if (OFD.ShowDialog() == DialogResult.OK) {
                Settings.Default.LastAudioFolder = Path.GetDirectoryName(OFD.FileName); Settings.Default.Save();
                if (tbDescription.Text == SFX.ShortFileName) tbDescription.Text = "";
                SFX.FileName = OFD.FileName;
                if (tbDescription.Text == "") tbDescription.Text = SFX.ShortFileName;
                PlayerState = PlayerState.uninitialised;
                UpdateFileToolTip();
            }
        }

        internal void PreloadFile(object sender, EventArgs e) {
            if (isPlaceholder) return;
            if (PlayerState == PlayerState.uninitialised) {
                LoadFile();
            }
        }

        private void LoadFile() {
            if (isPlaceholder) return;
            if (string.IsNullOrEmpty(SFX.FileName)) return;
            if (!File.Exists(SFX.FileName)) {
                Program.mainForm.ReportStatus("File not found: " + SFX.FileName);
                Debug.WriteLine("File not found: " + Path.GetFullPath(SFX.FileName));
                return;
            }
            PlayerState = PlayerState.loading;
            UpdatePlayerState(PlayerState);
            loadedDevice = (MMDevice)Devices.SelectedItem;
            _musicPlayer.Open(SFX.FileName, loadedDevice);
            _musicPlayer.Volume = SFX.Volume;
            PlayerState = PlayerState.loaded;
        }



        internal void StopOthers(object sender, EventArgs e) {
            if (isPlaceholder) return;
            if (sender != this) {
                //don't stop the paused ones
                if (_musicPlayer.PlaybackState == PlaybackState.Playing) {
                    Stop();
                }
            }
        }

        private void Stop() {
            if (isPlaceholder) return;
            if (PlayerState == PlayerState.paused || PlayerState == PlayerState.play) {
                _musicPlayer.Volume = 0;    //makes the stop less "clicky"
                Thread.Sleep(10);
                _musicPlayer.Stop();
                _musicPlayer.Volume = SFX.Volume;
                PlayerState = PlayerState.loaded;
            }
        }

        private PlayerState _playerstate;

        private void bnPlay_Click(object sender, EventArgs e) {
            if (isPlaceholder) return;
            try {
                if (PlayerState == PlayerState.paused) {
                    UnPause();
                } else if (PlayerState != PlayerState.play) {
                    Play();
                }
            } catch (Exception) {
                //ReportStatus(ex.Message);
            }
        }

        public void Play() {
            if (isPlaceholder) return;
            if (PlayerState == PlayerState.uninitialised) {
                LoadFile();
            }
            if (PlayerState == PlayerState.loaded) {
                PlayFromStart();
            }
            if (bnStopAll.Checked) {
                StopAll?.Invoke(this, new EventArgs());
            }
        }

        private void PlayFromStart() {
            if (isPlaceholder) return;
            _musicPlayer.Position = TimeSpan.Zero;  //this resets the volume!
            _musicPlayer.Volume = SFX.Volume;
            _musicPlayer.Play();
            PlayerState = PlayerState.play;
        }

        private void UnPause() {
            if (isPlaceholder) return;
            _musicPlayer.Resume();
            PlayerState = PlayerState.play;
        }

        private void Pause() {
            if (isPlaceholder) return;
            _musicPlayer.Pause();
            PlayerState = PlayerState.paused;
        }

        private void bnStop_Click(object sender, EventArgs e) {
            if (isPlaceholder) return;
            Stop();
        }

        private void tbDescription_MouseDoubleClick(object sender, MouseEventArgs e) {
            if (isPlaceholder) return;
            SelectFile();
        }

        private void tableLayoutPanel1_MouseDoubleClick(object sender, MouseEventArgs e) {
            if (isPlaceholder) return;
            SelectFile();
        }

        internal void ProgressUpdate(object sender, EventArgs e) {
            if (isPlaceholder) return;
            UpdatePosition(_musicPlayer.Position);
        }

        private int Progress = 0;
        internal static ComboBox PreviewDevices;

        private void UpdatePosition(TimeSpan position) {
            if (isPlaceholder) return;
            double end = _musicPlayer.Length.TotalSeconds;
            double now = _musicPlayer.Position.TotalSeconds;
            int pct = (int)(now / end * Width);
            if (pct == Progress) return;
            Progress = pct;
            //lbIndex.Text = ((int)(now / end * 100)).ToString("D3");
            SuspendLayout();
            DrawGraph(pct);
            ResumeLayout();
            Refresh();
        }

        private void DrawGraph(int pct) {
            if (isPlaceholder) return;
            pct = Math.Max(0, Math.Min(Width, pct));
            Graphics graphGraphics = Graphics.FromImage(graph);
            SolidBrush brush = new SolidBrush(Settings.Default.ColourPlayerPlay);
            if (pct > 0) {
                graphGraphics.FillRectangle(brush, 0, 0, pct, Height);
            }
            brush = new SolidBrush(Settings.Default.ColourPlayerLoaded);
            if (pct < Width) {
                graphGraphics.FillRectangle(brush, pct, 0, Width - pct, Height);
            }
        }

        private void PlayStrip_Load(object sender, EventArgs e) {
            if (isPlaceholder) return;
            if (Program.mainForm.WindowState == FormWindowState.Minimized) return;
            graph = new Bitmap(Width, Height);
            BackgroundImage = graph;
            //BackgroundImageLayout = ImageLayout.Stretch;
            DrawGraph(Progress);
        }

        private void PlayStrip_Resize(object sender, EventArgs e) {
            PlayStrip_Load(sender, e);
        }

        private void bnFile_Click(object sender, EventArgs e) {
            if (isPlaceholder) return;
            if (PlayerState == PlayerState.play) return;
            if (string.IsNullOrEmpty(SFX.FileName)) {
                SelectFile();
            } else {
                SFX.FileName = "";
                PlayerState = PlayerState.uninitialised;
            }
            UpdateFileToolTip();
        }

        private void UpdateFileToolTip() {
            if (string.IsNullOrEmpty(SFX.FileName)) {
                toolTip1.SetToolTip(bnFile, "Select File");
            } else {
                toolTip1.SetToolTip(bnFile, SFX.ShortFileName);
            }

        }

        bool justHidden = false;

        private void bnVolume_Enter(object sender, EventArgs e) {
            if (Form1.lastFocused == volume.Controls[0]) {
                justHidden = true;
            }
        }

        private void bnVolume_Click(object sender, EventArgs e) {
            if (justHidden) {
                justHidden = false;
            } else {
                Parent.Controls.Add(volume);
                Parent.Controls.SetChildIndex(volume, 0);
                Point Loc = Location;
                Loc.X += Width - volume.Width;
                Loc.Y += Height;
                volume.Location = Loc;
                volume.Volume = SFX.Volume;
                volume.Focus();
            }
        }

        private void Volume_Done(object sender, EventArgs e) {
            //focus left the volume fader, so disconnect it
            //Debug.WriteLine("Volume_Done - disconnecting fader control");
            if (Parent == null) return;     //can happen when opening a file with volume control showing
            if (Parent.Controls.Contains(volume)) {
                Parent.Controls.Remove(volume);
            }
        }

    }
}
