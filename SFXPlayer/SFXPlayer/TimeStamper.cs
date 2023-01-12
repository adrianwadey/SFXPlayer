using NAudio.Wave;
using NAudio.WaveFormRenderer;
using SFXPlayer.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace SFXPlayer
{
    public partial class TimeStamper : Form
    {
        public TimeStamper()
        {
            InitializeComponent();
        }
        private SFX SFX;

        public void Edit(SFX SFX)
        {
            this.SFX = SFX;
            this.ShowDialog();
        }

        private void TimeStamper_Load(object sender, EventArgs e)
        {
            pictureBox1_SizeChanged(this, EventArgs.Empty);
            listBox1.DataSource = SFX.Triggers;
            SFX.Triggers.ListChanged += Triggers_ListChanged;
            //listBox1.DisplayMember = nameof(Trigger.Description);
            //listBox1.ValueMember = nameof(Trigger.Time);
        }

        private void Triggers_ListChanged(object sender, ListChangedEventArgs e)
        {
            if (e.ListChangedType == ListChangedType.ItemChanged)
            {
                listBox1.SelectedItem = ActiveTrigger;
            }
        }

        Bitmap bm;
        Point ClickedAt;
        Trigger ActiveTrigger;

        public TimeSpan WaveLength { get; private set; }

        private void pictureBox1_DoubleClick(object sender, EventArgs e)
        {
            var trig = new Trigger() { Description = "MIDIEvent" };
            trig.Time = PositionToTime(ClickedAt.X);
            SFX.Triggers.Add(trig);
            ActiveTrigger = trig;
            RedrawTriggers();
        }
        private void RedrawTriggers(){
            bm = new Bitmap(pictureBox1.BackgroundImage.Width, pictureBox1.BackgroundImage.Height);
            Graphics g = Graphics.FromImage(bm);
            foreach (var trig in SFX.Triggers)
            {
                var X = TimeToPosition(trig.Time);
                if (listBox1.SelectedItem == trig)
                {
                    g.DrawLine(Pens.Red, X, 0, X, bm.Height - 1);
                    trackBar1.Value = X;
                }
                else
                {
                    g.DrawLine(Pens.Blue, X, 0, X, bm.Height - 1);
                }
            }
            pictureBox1.Image = bm;
            if (listBox1.SelectedItems.Count == 0)
            {
                trackBar1.Enabled = false;
            }
            else
            {
                trackBar1.Enabled = true;
            }
        }

        private int TimeToPosition(DateTime time)
        {
            return (int)(pictureBox1.BackgroundImage.Width * time.Ticks / WaveLength.Ticks);
        }

        private DateTime PositionToTime(int x)
        {
            return new DateTime(WaveLength.Ticks * x / pictureBox1.BackgroundImage.Width);
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            ClickedAt = e.Location;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ActiveTrigger = (Trigger)listBox1.SelectedItem;
            RedrawTriggers();
        }

        private void pictureBox1_SizeChanged(object sender, EventArgs e)
        {
            var maxPeakProvider = new MaxPeakProvider();
            var rmsPeakProvider = new RmsPeakProvider(200); // e.g. 200
            var samplingPeakProvider = new SamplingPeakProvider(200); // e.g. 200
            var averagePeakProvider = new AveragePeakProvider(4); // e.g. 4
            var myRendererSettings = new StandardWaveFormRendererSettings();
            myRendererSettings.Width = pictureBox1.Width - pictureBox1.Padding.Horizontal;
            myRendererSettings.BottomHeight = myRendererSettings.TopHeight = (pictureBox1.Height - pictureBox1.Padding.Vertical) / 2 - 2;
            var renderer = new WaveFormRenderer();
            using (var waveStream = new AudioFileReader(SFX.FileName))
            {
                WaveLength = waveStream.TotalTime;
                Length.Text = WaveLength.ToString(@"mm\:ss\.ff");
                pictureBox1.BackgroundImage = renderer.Render(waveStream, rmsPeakProvider, myRendererSettings);
                bm = new Bitmap(pictureBox1.BackgroundImage.Width, pictureBox1.BackgroundImage.Height);
                pictureBox1.Image = bm;
            }
            trackBar1.Maximum = myRendererSettings.Width;
            RedrawTriggers();
        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            if (listBox1.SelectedItems.Count == 0) return;
            ((Trigger)listBox1.SelectedItem).Edit();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            ActiveTrigger.Time = PositionToTime(trackBar1.Value);
            SFX.Triggers.ResetBindings();
            RedrawTriggers();
        }

    }
}
