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
            try
            {
                var maxPeakProvider = new MaxPeakProvider();
                //var rmsPeakProvider = new RmsPeakProvider(200); // e.g. 200
                var samplingPeakProvider = new SamplingPeakProvider(200); // e.g. 200
                var averagePeakProvider = new AveragePeakProvider(4); // e.g. 4
                var myRendererSettings = new StandardWaveFormRendererSettings();
                myRendererSettings.Width = Screen.AllScreens.Select(s => s.Bounds.Width).Max();
                myRendererSettings.BottomHeight = myRendererSettings.TopHeight = (pictureBox1.Height - pictureBox1.Padding.Vertical) / 2 - 2;
                var renderer = new WaveFormRenderer();
                using (var waveStream = new AudioFileReader(SFX.FileName))
                {
                    WaveLength = waveStream.TotalTime;
                    Length.Text = WaveLength.ToString(@"mm\:ss\.ff");
                    pictureBox1.BackgroundImageLayout = ImageLayout.Stretch;
                    pictureBox1.BackgroundImage = renderer.Render(waveStream, maxPeakProvider, myRendererSettings);
                    bm = new Bitmap(pictureBox1.BackgroundImage.Width, pictureBox1.BackgroundImage.Height);
                    pictureBox1.Image = bm;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Something went wrong");
                this.Close();
            }


            listBox1.DataSource = SFX.Triggers;
            SFX.Triggers.ListChanged += Triggers_ListChanged;
            //listBox1.DisplayMember = nameof(Trigger.Description);
            //listBox1.ValueMember = nameof(Trigger.Time);
        }

        private void Triggers_ListChanged(object sender, ListChangedEventArgs e)
        {
            //changed to avoid re-ordering of trigger points.
            //Debug.WriteLine("changed");
            //SFX.Triggers = new BindingList<Trigger>(SFX.Triggers.OrderBy(t => t.TimeTicks).ToList());
            //if (e.ListChangedType == ListChangedType.ItemChanged)
            //{
            //    listBox1.SelectedItem = ActiveTrigger;
            //}
        }

        Bitmap bm;
        Point ClickedAt;
        private Trigger _ActiveTrigger;
        int trigmin, trigmax;
        Trigger ActiveTrigger
        {
            get
            {
                return _ActiveTrigger;
            }
            set
            {
                _ActiveTrigger = value;
                trigmin = trackBar1.Minimum;    //always 0
                trigmax = trackBar1.Maximum;    //limit range of trigger (prevent overlapping)
                if (listBox1.SelectedIndex > 0)
                {
                    trigmin = TicksToPosition(((Trigger)listBox1.Items[listBox1.SelectedIndex - 1]).TimeTicks) + 2;
                }
                if (listBox1.SelectedIndex < listBox1.Items.Count - 1)
                {
                    trigmax = TicksToPosition(((Trigger)listBox1.Items[listBox1.SelectedIndex + 1]).TimeTicks);
                }
            }
        }

        public TimeSpan WaveLength { get; private set; }

        private void pictureBox1_DoubleClick(object sender, EventArgs e)
        {
            var trig = new Trigger() { Description = "MIDIEvent", showEvent = new MSCEvent() };
            trig.TimeTicks = PositionToTicks(ClickedAt.X);
            SFX.Triggers.Add(trig);
            ActiveTrigger = trig;
            RedrawTriggers();
        }
        private void RedrawTriggers()
        {
            trackBar1.Maximum = pictureBox1.Width - pictureBox1.Padding.Horizontal;
            bm = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Graphics g = Graphics.FromImage(bm);
            foreach (var trig in SFX.Triggers)
            {
                var X = TicksToPosition(trig.TimeTicks);
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

        private int TicksToPosition(long ticks)
        {
            return (int)(pictureBox1.Width * ticks / WaveLength.Ticks);
        }

        private long PositionToTicks(int x)
        {
            return WaveLength.Ticks * x / pictureBox1.Width;
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
            ActiveTrigger = ActiveTrigger;      //force update of movement limits
            RedrawTriggers();
        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            if (listBox1.SelectedItems.Count == 0) return;
            ((Trigger)listBox1.SelectedItem).Edit();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            int newval = trackBar1.Value;
            newval = Math.Max(trigmin, newval);
            newval = Math.Min(trigmax, newval);
            ActiveTrigger.TimeTicks = PositionToTicks(newval);
            SFX.Triggers.ResetBindings();
            RedrawTriggers();
        }

    }
}
