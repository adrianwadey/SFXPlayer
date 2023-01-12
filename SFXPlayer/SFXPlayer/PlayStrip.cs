using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.ObjectModel;
using AudioPlayerSample;
using SFXPlayer.Properties;
using System.Threading;
using System.IO;
using System.Diagnostics;
using NAudio.Wave;
using NAudio.CoreAudioApi;
using Svg;
using Svg.FilterEffects;
using System.Reflection;
using System.Xml.Linq;
using System.Linq;

namespace SFXPlayer
{
    enum PlayerState
    {
        error = -1,
        uninitialised,// = PlaybackState.Stopped,
        play,// = PlaybackState.Playing,
        paused,// = PlaybackState.Paused,
        loading = 3,
        loaded = 4,

    }

    public partial class PlayStrip : UserControl
    {

        private Bitmap graph;
        private readonly MusicPlayer _musicPlayer = new MusicPlayer();
        private readonly MusicPlayer _PreviewPlayer = new MusicPlayer();
        ucVolume volume = new ucVolume();
        public event EventHandler StopAll;
        public event EventHandler<StatusEventArgs> ReportStatus;
        public static ComboBox Devices;
        int prevPct = -1;

        #region Initialisation

        public PlayStrip()
        {
            InitializeComponent();
            InitialiseButtons();
            InitializeSound();
            SFX = new SFX();
        }

        private void InitialiseButtons()
        {
            bnVolume.Image = FromSvgResource("volume-up-fill.svg");
            bnPlay.Image = FromSvgResource("play-fill.svg");
            bnEdit.Image = FromSvgResource("clock-history.svg");
        }

        public PlayStrip(SFX SFX) : this()
        {
            this.SFX = SFX;
            UpdateFileButton();
        }
        // Sets up the SoundPlayer object.
        private void InitializeSound()
        {
            // Create an instance of the SoundPlayer class.
            //player = new SoundPlayer();

            // Listen for the LoadCompleted event.
            //player.LoadCompleted += new AsyncCompletedEventHandler(player_LoadCompleted);

            // Listen for the SoundLocationChanged event.
            //player.SoundLocationChanged += new EventHandler(player_LocationChanged);

            components = new Container();
            components.Add(_musicPlayer);
            _musicPlayer.PlaybackStopped += (s, args) =>
            {
                //WasapiOut uses SynchronizationContext.Post to raise the event
                //There might be already a new WasapiOut-instance in the background when the async Post method brings the PlaybackStopped-Event to us.
                //if (_musicPlayer.PlaybackState != PlaybackState.Stopped)
                //    btnPlay.Enabled = btnStop.Enabled = btnPause.Enabled = false;
            };
            _musicPlayer.PlaybackStopped += _musicPlayer_PlaybackStopped;
            components.Add(_PreviewPlayer);
            _PreviewPlayer.PlaybackStopped += _PreviewPlayer_PlaybackStopped;
            volume.VolumeChanged += Volume_VolumeChanged;
            volume.Done += Volume_Done;


        }

        private void _PreviewPlayer_PlaybackStopped(object sender, StoppedEventArgs e)
        {
            UpdatePlayerState(PlayerState);
        }

        private void PlayStrip_Load(object sender, EventArgs e)
        {
            AddDnDEventHandlers(this);
        }

        #endregion

        #region SFX_Object

        private SFX _SFX;
        public SFX SFX
        {
            get
            {
                return _SFX;
            }
            set
            {
                _SFX = value;
                tbDescription.Text = SFX.Description;
                bnStopAll.Checked = SFX.StopOthers;
                UpdatePlayerState(PlayerState);
            }
        }

        private void tbDescription_TextChanged(object sender, EventArgs e)
        {
            SFX.Description = tbDescription.Text;
        }

        private void bnStopAll_CheckedChanged(object sender, EventArgs e)
        {
            SFX.StopOthers = bnStopAll.Checked;
        }

        #endregion

        #region UI

        private void PlayStrip_Resize(object sender, EventArgs e)
        {
            ResizeProgressBar();
        }

        private void UpdatePlayerState(PlayerState newstate)
        {
            switch (newstate)
            {
                case PlayerState.uninitialised:
                    if (string.IsNullOrEmpty(SFX.FileName))
                    {
                        BackColor = SystemColors.Control;
                    }
                    else
                    {
                        BackColor = Settings.Default.ColourPlayerIdle;
                    }
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
                case PlayerState.error:
                    BackColor = Color.Red;
                    break;
                default:
                    BackColor = Settings.Default.ColourPlayerIdle;
                    break;
            }
            prevPct = -1;
        }

        public int PlayStripIndex
        {
            get
            {
                return int.Parse(lbIndex.Text) - 1;
            }
            set
            {
                lbIndex.Text = (value + 1).ToString("D3");
            }

        }

        #endregion

        #region AudioFile

        private void bnFile_Click(object sender, EventArgs e)
        {
            if (PlayerState == PlayerState.play) return;
            if (string.IsNullOrEmpty(SFX.FileName))
            {
                ChooseFile();
            }
            else
            {
                if (tbDescription.Text == SFX.ShortFileNameOnly) tbDescription.Text = "";
                SFX.FileName = "";
                PlayerState = PlayerState.uninitialised;
            }
            UpdateFileButton();
        }

        private void tableLayoutPanel1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ChooseFile();
        }

        private void lbIndex_DoubleClick(object sender, EventArgs e)
        {
            ChooseFile();
        }

        private void tbDescription_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ChooseFile();
        }

        private void UpdateFileButton()
        {
            if (string.IsNullOrEmpty(SFX.FileName))
            {
                toolTip1.SetToolTip(bnFile, "Select Sound File");
            }
            else if (!File.Exists(SFX.FileName))
            {
                toolTip1.SetToolTip(bnFile, "File not found: \"" + SFX.ShortFileName + "\"");
            }
            else
            {
                toolTip1.SetToolTip(bnFile, "Remove \"" + SFX.ShortFileName + "\"");
            }
            if (string.IsNullOrEmpty(SFX.FileName))
            {
                bnFile.Image = FromSvgResource("file-music.svg");
            }
            else
            {
                bnFile.Image = FromSvgResource("file-earmark-music.svg");
            }
            UpdatePreviewButton();
        }

        public static OpenFileDialog OFD;

        private void ChooseFile()
        {
            if (PlayerState == PlayerState.play) return;
            if (PlayerState == PlayerState.paused) Stop();
            //OFD.Filter = CSCore.Codecs.CodecFactory.SupportedFilesFilterEn + "|All files|*.*";
            if (Directory.Exists(Settings.Default.LastAudioFolder))
            {
                OFD.InitialDirectory = Settings.Default.LastAudioFolder;
            }
            OFD.Title = "Choose audio file";
            OFD.FileName = "";
            if (OFD.ShowDialog() == DialogResult.OK)
            {
                SelectFile(OFD.FileName);
            }
        }

        public void SelectFile(string FileName)
        {
            Settings.Default.LastAudioFolder = Path.GetDirectoryName(FileName); Settings.Default.Save();
            if (tbDescription.Text == SFX.ShortFileNameOnly) tbDescription.Text = "";
            SFX.FileName = FileName;
            if (tbDescription.Text == "") tbDescription.Text = SFX.ShortFileNameOnly;
            PlayerState = PlayerState.uninitialised;
            UpdateFileButton();
            if (Settings.Default.PreloadAll)
            {
                PreloadFile();
            }
        }

        internal void PreloadFile()
        {
            if (PlayerState == PlayerState.uninitialised)
            {
                LoadFile();
            }
        }

        private void LoadFile()
        {
            if (string.IsNullOrEmpty(SFX.FileName)) return;
            if (!File.Exists(SFX.FileName))
            {
                PlayerState = PlayerState.error;
            }
            if (!File.Exists(SFX.FileName))
            {
                Program.mainForm.ReportStatus("File not found: " + SFX.FileName);
                Debug.WriteLine("File not found: " + Path.GetFullPath(SFX.FileName));
                return;
            }
            PlayerState = PlayerState.loading;
            UpdatePlayerState(PlayerState);
            //var outputDevice = new DirectSoundOut(((DirectSoundDeviceInfo)Devices.SelectedItem).Guid);
            _musicPlayer.Open(SFX.FileName, Devices.SelectedIndex);
            _musicPlayer.Volume = SFX.Volume;
            PlayerState = PlayerState.loaded;
        }

        #endregion

        #region Transport

        private PlayerState _playerstate;

        private PlayerState PlayerState
        {
            get
            {
                return _playerstate;
            }
            set
            {
                _playerstate = value;
                UpdatePlayerState(_playerstate);
            }
        }

        public bool IsPlaying => (PlayerState == PlayerState.play);

        private void bnPreview_Click(object sender, EventArgs e)
        {
            if (_PreviewPlayer.PlaybackState == PlaybackState.Playing)
            {
                _PreviewPlayer.Stop();
            }
            else
            {
                if (!File.Exists(SFX.FileName)) return;
                _PreviewPlayer.Open(SFX.FileName, PreviewDevices.SelectedIndex);
                _PreviewPlayer.Volume = SFX.Volume; _PreviewPlayer.Position = TimeSpan.Zero;  //this resets the volume!
                _PreviewPlayer.Volume = SFX.Volume;
                _PreviewPlayer.Play();
                BackColor = Settings.Default.ColourPreview;
            }
            UpdatePreviewButton();
        }

        private void UpdatePreviewButton()
        {
            if (_PreviewPlayer.PlaybackState == PlaybackState.Playing)
            {
                //bnPreview.Image = Resources.Stop2_18;
                bnPreview.Image = FromSvgResource("stop-fill.svg");
                toolTip1.SetToolTip(bnPreview, "Stop Preview");
            }
            else
            {
                if (!File.Exists(SFX.FileName))
                {
                    bnPreview.Image = FromSvgResource("blank.svg");
                    toolTip1.SetToolTip(bnPreview, "");
                }
                else
                {
                    //bnPreview.Image = Resources.Headphones2_18;
                    bnPreview.Image = FromSvgResource("headphones.svg");
                    toolTip1.SetToolTip(bnPreview, "Preview");
                }
            }
        }

        private Bitmap FromSvgResource(string resourcename)
        {
            // Determine path
            var assembly = Assembly.GetExecutingAssembly();
            string resourcePath;// = resourcename;
                                // Format: "{Namespace}.{Folder}.{filename}.{Extension}"
            resourcePath = assembly.GetManifestResourceNames().First(str => str.EndsWith(resourcename));

            using (Stream stream = assembly.GetManifestResourceStream(resourcePath))
            using (StreamReader reader = new StreamReader(stream))
            {
                return SvgDocument.FromSvg<SvgDocument>(reader.ReadToEnd()).Draw();
            }

        }
        private void bnPlay_Click(object sender, EventArgs e)
        {
            try
            {
                if (PlayerState == PlayerState.paused)
                {
                    UnPause();
                }
                else if (PlayerState == PlayerState.play)
                {
                    Stop();
                }
                else if (PlayerState != PlayerState.play)
                {
                    Play();
                }
            }
            catch (Exception)
            {
                //ReportStatus(ex.Message);
            }
        }

        private void UpdatePlayButton()
        {
            if (PlayerState == PlayerState.play)
            {
                bnPlay.Image = FromSvgResource("stop-fill.svg");
                toolTip1.SetToolTip(bnPlay, "Stop");
            }
            else
            {
                if (!File.Exists(SFX.FileName))
                {
                    bnPlay.Image = FromSvgResource("blank.svg");
                    toolTip1.SetToolTip(bnPlay, "");
                }
                else
                {
                    bnPlay.Image = FromSvgResource("play-fill.svg");
                    toolTip1.SetToolTip(bnPlay, "Play");
                }
            }
        }

        private void bnStop_Click(object sender, EventArgs e)
        {
            Stop();
        }


        public void Play()
        {
            if (PlayerState == PlayerState.uninitialised)
            {
                LoadFile();
            }
            if (PlayerState == PlayerState.loaded)
            {
                PlayFromStart();
                ReportStatus?.Invoke(this, new StatusEventArgs("Playing " + SFX.ShortFileNameOnly));
            }
            if (bnStopAll.Checked)
            {
                StopAll?.Invoke(this, new EventArgs());
            }
        }

        internal void StopOthers(object sender, EventArgs e)
        {
            if (sender != this)
            {
                //don't stop the paused ones
                if (_musicPlayer.PlaybackState == PlaybackState.Playing)
                {
                    Stop();
                }
            }
        }

        internal void StopPreviews(object sender, EventArgs e)
        {
            if (_PreviewPlayer.PlaybackState == PlaybackState.Playing)
            {
                _PreviewPlayer.Stop();
            }
        }

        private void PlayFromStart()
        {
            _musicPlayer.Position = TimeSpan.Zero;  //this resets the volume!
            _musicPlayer.Volume = SFX.Volume;
            _musicPlayer.Play();
            PlayerState = PlayerState.play;
            UpdatePlayButton();
        }

        private void Pause()
        {
            _musicPlayer.Pause();
            PlayerState = PlayerState.paused;
            ReportStatus?.Invoke(this, new StatusEventArgs("Playing " + SFX.ShortFileNameOnly, true));
            UpdatePlayButton();
        }

        private void UnPause()
        {
            _musicPlayer.Resume();
            PlayerState = PlayerState.play;
            ReportStatus?.Invoke(this, new StatusEventArgs("Playing " + SFX.ShortFileNameOnly));
            UpdatePlayButton();
        }

        public void Stop()
        {
            if (PlayerState == PlayerState.paused || PlayerState == PlayerState.play)
            {
                _musicPlayer.Volume = 0;    //makes the stop less "clicky"
                Thread.Sleep(10);
                _musicPlayer.Stop();
                _musicPlayer.Volume = SFX.Volume;
                PlayerState = PlayerState.loaded;
                UpdatePlayButton();
            }
        }

        private void _musicPlayer_PlaybackStopped(object sender, StoppedEventArgs e)
        {
            PlayerState = PlayerState.loaded;
            UpdatePlayButton();
            try
            {
                ReportStatus?.Invoke(this, new StatusEventArgs("Playing " + SFX.ShortFileNameOnly, true));
            }
            catch { }
        }

        #endregion

        #region ProgressBar

        private void ResizeProgressBar()
        {
            if (Program.mainForm?.WindowState == FormWindowState.Minimized) return;
            graph = new Bitmap(Width, Height);
            BackgroundImage = graph;
            //BackgroundImageLayout = ImageLayout.Stretch;
            prevPct = -1;
            DrawGraph(Progress);
        }

        internal void ProgressUpdate(object sender, EventArgs e)
        {
            //UpdatePosition(_musicPlayer.Position);
        }

        private int Progress = 0;
        internal static ComboBox PreviewDevices;

        private void UpdatePosition(TimeSpan position)
        {
            double end = _musicPlayer.Length.TotalSeconds;
            double now = position.TotalSeconds;
            int pct = (int)(now / end * Width);
            if (pct == Progress) return;
            Progress = pct;
            SuspendLayout();
            Rectangle Changed = DrawGraph(pct);
            ResumeLayout();
            Invalidate(Changed);
            Refresh();
        }

        private Rectangle DrawGraph(int pct)
        {
            pct = Math.Max(0, Math.Min(Width, pct));
            Graphics graphGraphics = Graphics.FromImage(graph);
            SolidBrush brush = new SolidBrush(Settings.Default.ColourPlayerPlay);
            if (pct > 0)
            {
                graphGraphics.FillRectangle(brush, 0, 0, pct, Height);
            }
            brush = new SolidBrush(Settings.Default.ColourPlayerLoaded);
            if (pct < Width)
            {
                graphGraphics.FillRectangle(brush, pct, 0, Width - pct, Height);
            }
            if (prevPct == -1)
            {
                prevPct = pct;
                return new Rectangle(0, 0, Width, Height);
            }
            else
            {
                Rectangle result = new Rectangle(prevPct, 0, pct, Height);
                prevPct = pct;
                return result;
            }
        }

        #endregion

        #region Volume

        bool justHidden = false;

        private void bnVolume_Enter(object sender, EventArgs e)
        {
            if (Form1.lastFocused == volume.Controls[0])
            {
                justHidden = true;
            }
        }

        private void bnVolume_Click(object sender, EventArgs e)
        {
            if (justHidden)
            {
                justHidden = false;
            }
            else
            {
                Point Loc = Parent.Location;
                Loc.X += Location.X + Width - volume.Width;
                Loc.Y += Location.Y + Height;
                if (Loc.Y + volume.Height > Parent.Parent.ClientSize.Height)
                {
                    Debug.WriteLine("Won't fit");
                    //return;
                    Loc.Y = Parent.Parent.ClientSize.Height - volume.Height;
                }
                Parent.Parent.Controls.Add(volume);
                Parent.Parent.Controls.SetChildIndex(volume, 0);
                volume.Location = Loc;
                volume.Volume = SFX.Volume;
                volume.Focus();
                Debug.WriteLine("This = " + this.ToString());
                Debug.WriteLine("Parent = " + Parent.ToString());
                Debug.WriteLine("Parent.Parent = " + Parent.Parent.ToString());
                BackColor = SystemColors.Highlight;
            }
        }

        private void Volume_VolumeChanged(object sender, EventArgs e)
        {
            SFX.Volume = volume.Volume;
            _musicPlayer.Volume = SFX.Volume;
            toolTip1.SetToolTip(bnVolume, "Vol=" + SFX.Volume.ToString());
        }

        private void Volume_Done(object sender, EventArgs e)
        {
            //focus left the volume fader, so disconnect it
            //Debug.WriteLine("Volume_Done - disconnecting fader control");
            if (Parent == null) return;
            if (Parent.Parent == null) return;
            if (Parent.Parent.Controls.Contains(volume))
            {
                Parent.Parent.Controls.Remove(volume);
            }
            UpdatePlayerState(PlayerState);
        }
        #endregion

        #region DragNDrop

        int AddDnDEventHandlers(Control ctl)
        {
            int count = 0;
            ctl.MouseDown += MouseDownHandler;
            ctl.MouseMove += MouseMoveHandler;
            ctl.MouseUp += MouseUpHandler;
            count++;
            foreach (Control subCtl in ctl.Controls)
            {
                count += AddDnDEventHandlers(subCtl);
            }
            return count;
        }

        private bool CheckingForDrag = false;
        private Rectangle DragBounds = new Rectangle();

        private void MouseDownHandler(object sender, MouseEventArgs e)
        {
            CheckingForDrag = true;
            DragBounds = new Rectangle(e.X - 5, e.Y - 5, 10, 10);
        }

        private void MouseUpHandler(object sender, MouseEventArgs e)
        {
            CheckingForDrag = false;
        }

        private void MouseMoveHandler(object sender, MouseEventArgs e)
        {
            if (CheckingForDrag)
            {
                if (DragBounds.Contains(e.Location)) return;
                DoDragDrop(this, DragDropEffects.Move | DragDropEffects.Scroll);
                CheckingForDrag = false;
            }
        }
        #endregion

        private void bnEdit_Click(object sender, EventArgs e)
        {
            TimeStamper timeStamper = new TimeStamper();
            timeStamper.Edit(SFX);
        }
    }

    public class StatusEventArgs : EventArgs
    {
        public string Status;
        public bool Clear = false;
        public StatusEventArgs(string status)
        {
            Status = status;
        }
        public StatusEventArgs(string status, bool clear)
        {
            Status = status;
            Clear = clear;
        }
    }
}
