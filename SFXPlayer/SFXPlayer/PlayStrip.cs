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

        public PlayStrip(SFX SFX) {
            InitializeComponent();
            InitializeSound();
            this.SFX = SFX;
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
        }

        private void _musicPlayer_PlaybackStopped(object sender, PlaybackStoppedEventArgs e) {
            PlayerState = PlayerState.loaded;
        }

        public static OpenFileDialog OFD;
        public static ComboBox Devices;
        //WAVSounds ws;
        //private SoundPlayer player;
        private bool _Placeholder;
        public bool Placeholder {
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
                bnStopAll.Checked = SFX.Loop;
            }
        }

        public bool IsPlaying => (PlayerState == PlayerState.play);

        private void tbDescription_TextChanged(object sender, EventArgs e) {
            SFX.Description = tbDescription.Text;
        }

        private void cbLoop_CheckedChanged(object sender, EventArgs e) {
            SFX.Loop = bnStopAll.Checked;
        }

        private void label1_DoubleClick(object sender, EventArgs e) {
            SelectFile();
        }

        private void SelectFile() {
            if (PlayerState == PlayerState.play) return;
            if (PlayerState == PlayerState.paused) Stop();
            OFD.Filter = "wav files|*.wav|All files|*.*";
            if (OFD.ShowDialog() == DialogResult.OK) {
                if (tbDescription.Text == SFX.ShortFileName) tbDescription.Text = "";
                SFX.FileName = OFD.FileName;
                if (tbDescription.Text == "") tbDescription.Text = SFX.ShortFileName;
                PlayerState = PlayerState.uninitialised;
            }
        }

        internal void PreloadFile(object sender, EventArgs e) {
            LoadFile();
        }

        private void LoadFile() {
            if (!File.Exists(SFX.FileName)) return;
            PlayerState = PlayerState.loading;
            UpdatePlayerState(PlayerState);
            _musicPlayer.Open(SFX.FileName, (MMDevice)Devices.SelectedItem);
            loadedDevice = (MMDevice)Devices.SelectedItem;
            PlayerState = PlayerState.loaded;
        }



        internal void StopOthers(object sender, EventArgs e) {
            if (sender != this) {
                //don't stop the paused ones
                if (_musicPlayer.PlaybackState == PlaybackState.Playing) {
                    Stop();
                }
            }
        }

        private void Stop() {
            if (PlayerState == PlayerState.paused || PlayerState == PlayerState.play) {
                _musicPlayer.Volume = 0;    //makes the stop less "clicky"
                Thread.Sleep(10);
                _musicPlayer.Stop();
                _musicPlayer.Volume = 100;
                PlayerState = PlayerState.loaded;
            }
        }

        private PlayerState _playerstate;

        private void bnPlay_Click(object sender, EventArgs e) {
            try {
                if (PlayerState == PlayerState.paused) {
                    UnPause();
                } else if (PlayerState != PlayerState.play) {
                    if (PlayerState == PlayerState.uninitialised) {
                        LoadFile();
                    }
                    if (PlayerState == PlayerState.loaded) {
                        Play();
                        if (bnStopAll.Checked) {
                            StopAll(this, new EventArgs());
                        }
                    }
                }
            } catch (Exception) {
                //ReportStatus(ex.Message);
            }
        }

        private void Play() {
            _musicPlayer.Position = TimeSpan.Zero;
            _musicPlayer.Play();
            PlayerState = PlayerState.play;
        }

        private void UnPause() {
            _musicPlayer.Resume();
            PlayerState = PlayerState.play;
        }

        private void Pause() {
            _musicPlayer.Pause();
            PlayerState = PlayerState.paused;
        }

        private void bnStop_Click(object sender, EventArgs e) {
            Stop();
        }

        private void tbDescription_MouseDoubleClick(object sender, MouseEventArgs e) {
            SelectFile();
        }

        private void tableLayoutPanel1_MouseDoubleClick(object sender, MouseEventArgs e) {
            SelectFile();
        }

        internal void ProgressUpdate(object sender, EventArgs e) {
            UpdatePosition(_musicPlayer.Position);
        }

        private int Progress = 0;
        internal static ComboBox PreviewDevices;

        private void UpdatePosition(TimeSpan position) {
            double end = _musicPlayer.Length.TotalSeconds;
            double now = _musicPlayer.Position.TotalSeconds;
            int pct = (int)(now / end * Width);
            if (pct == Progress) return;
            Progress = pct;
            //lbIndex.Text = ((int)(now / end * 100)).ToString("D3");
            DrawGraph(pct);
            Refresh();
        }

        private void DrawGraph(int pct) {
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
            graph = new Bitmap(Width, Height);
            BackgroundImage = graph;
            //BackgroundImageLayout = ImageLayout.Stretch;
            DrawGraph(Progress);
        }
    }
}
