using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SFXPlayer {
    public partial class SplashScreen : Form {
        public SplashScreen() {
            InitializeComponent();
        }

        private void SplashScreen_Close(object sender, EventArgs e) {
            this.Close();
        }

        private void SplashScreen_Close(object sender, KeyPressEventArgs e) {
            this.Close();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            System.Diagnostics.Process.Start("https://adrianwadey.github.io/SFXPlayer/");
        }
    }
}
