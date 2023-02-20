using NAudio.Wave;
using NAudio.WaveFormRenderer;
using SFXPlayer.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace SFXPlayer {
    public partial class EventEditor : Form {
        public EventEditor() {
            InitializeComponent();
        }
        private SFX SFX;

        public void Edit(SFX SFX) {
            this.SFX = SFX;
            this.ShowDialog();
        }

        private void EventEditor_Load(object sender, EventArgs e) {
            listBox1.DataSource = SFX.Triggers;
            SFX.Triggers.ListChanged += Triggers_ListChanged;
            //listBox1.DisplayMember = nameof(Trigger.Description);
            //listBox1.ValueMember = nameof(Trigger.Time);
        }

        private void Triggers_ListChanged(object sender, ListChangedEventArgs e) {
            //changed to avoid re-ordering of trigger points.
            //Debug.WriteLine("changed");
            //SFX.Triggers = new BindingList<Trigger>(SFX.Triggers.OrderBy(t => t.TimeTicks).ToList());
            //if (e.ListChangedType == ListChangedType.ItemChanged)
            //{
            //    listBox1.SelectedItem = ActiveTrigger;
            //}
        }

        private Trigger AddMIDIEvent() {
            var trig = new Trigger() { Description = "MIDIEvent", showEvent = new MSCEvent() };
            SFX.Triggers.Add(trig);
            return trig;
        }

        private Trigger AddX32MuteEvent() {
            var trig = new Trigger() { Description = "X32 Mute Change", showEvent = new X32MuteEvent() };
            SFX.Triggers.Add(trig);
            return trig;
        }

        private void listBox1_DoubleClick(object sender, EventArgs e) {
            if (listBox1.SelectedItem == null) return;
            ((Trigger)listBox1.SelectedItem).Edit();
            listBox1.DataSource = null;
            listBox1.DataSource = SFX.Triggers;
        }

        private void bnAdd_Click(object sender, EventArgs e) {
            listBox1.SelectedItem = AddMIDIEvent();
            ((Trigger)listBox1.SelectedItem).Edit();
            listBox1.DataSource = null;
            listBox1.DataSource = SFX.Triggers;
        }

        private void button1_Click(object sender, EventArgs e) {
            listBox1.SelectedItem = AddX32MuteEvent();
            ((Trigger)listBox1.SelectedItem).Edit();
            listBox1.DataSource = null;
            listBox1.DataSource = SFX.Triggers;
        }

        private void bnRemove_Click(object sender, EventArgs e) {
            if (listBox1.SelectedItem == null) return;
            ((BindingList<Trigger>)listBox1.DataSource).Remove((Trigger)listBox1.SelectedItem);
        }
    }
}
