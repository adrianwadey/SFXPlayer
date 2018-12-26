using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace SFXPlayer {
    public partial class ucVolume : UserControl {
        public event EventHandler VolumeChanged;
        public event EventHandler Done;

        public ucVolume() {
            InitializeComponent();
        }

        public int Volume {
            get {
                return tbVolume.Value;
            }
            internal set {
                tbVolume.Value = value;
            }
        }

        private void tbVolume_Scroll(object sender, EventArgs e) {
            VolumeChanged?.Invoke(this, e);
        }

        private void tbVolume_Leave(object sender, EventArgs e) {
            Done?.Invoke(this, e);
            //Debug.WriteLine(sender.ToString() + " lost focus");
        }

        private void tbVolume_Enter(object sender, EventArgs e) {
            ((Form1)TopLevelControl).ScrollTimer.Enabled = true;
        }
    }
}
