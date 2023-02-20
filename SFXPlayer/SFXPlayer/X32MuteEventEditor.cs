using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static SFXPlayer.X32MuteEvent;

namespace SFXPlayer {
    public partial class X32MuteEventEditor : Form {
        public X32MuteEventEditor() {
            InitializeComponent();
        }

        internal X32MuteEvent XMute { get; private set; }

        internal static void Edit(X32MuteEvent XMute) {
            Debug.WriteLine(XMute);
            X32MuteEventEditor editor = new X32MuteEventEditor();
            editor.XMute = XMute;
            editor.AddMuteButtons();
            editor.ShowDialog();
        }

        private void AddMuteButtons() {
            flowLayoutPanel1.Controls.Clear();
            for (int Channel = 0; Channel < XMute.Channels.Length; Channel++) {
                var Mute = XMute.Channels[Channel];
                CheckBox cbMute = new CheckBox {
                    Appearance = Appearance.Button,
                    Tag = Channel,
                    ThreeState = true,
                    Text = (Channel + 1).ToString("D2"),
                    AutoSize = true,
                };
                flowLayoutPanel1.Controls.Add(cbMute);
                cbMute.CheckStateChanged += new EventHandler(checkBox_CheckStateChanged);
                switch (Mute) {
                    case MuteState.Neutral:
                        cbMute.CheckState = CheckState.Indeterminate;
                        break;
                    case MuteState.Live:
                        cbMute.CheckState = CheckState.Unchecked;
                        break;
                    case MuteState.Mute:
                        cbMute.CheckState = CheckState.Checked;
                        break;
                }
                checkBox_CheckStateChanged(cbMute, EventArgs.Empty);
            }
        }

        private void checkBox_CheckStateChanged(object sender, EventArgs e) {
            if (sender.GetType() == typeof(CheckBox)) {
                CheckBox cb = (CheckBox)sender;
                switch (cb.CheckState) {
                    case CheckState.Unchecked:          //Mute is off
                        cb.BackColor = Color.Green;
                        if (XMute.Channels[(int)cb.Tag] != MuteState.Live) {
                            XMute.Channels[(int)cb.Tag] = MuteState.Live;
                            XMute.OnShowEventChanged();
                        }
                        break;
                    case CheckState.Checked:
                        cb.BackColor = Color.Red;
                        if (XMute.Channels[(int)cb.Tag] != MuteState.Mute) {
                            XMute.Channels[(int)cb.Tag] = MuteState.Mute;
                            XMute.OnShowEventChanged();
                        }
                        break;
                    case CheckState.Indeterminate:
                        cb.BackColor = SystemColors.Control;
                        if (XMute.Channels[(int)cb.Tag] != MuteState.Neutral) {
                            XMute.Channels[(int)cb.Tag] = MuteState.Neutral;
                            XMute.OnShowEventChanged();
                        }
                        break;
                    default:
                        break;
                }
            }

        }
    }
}
