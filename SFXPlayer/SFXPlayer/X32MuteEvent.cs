using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;

namespace SFXPlayer {
    public class X32MuteEvent : AbstractShowEvent {
        public enum MuteState {
            Neutral, 
            Mute, 
            Live
        }
        public override void Edit() {
            X32MuteEventEditor.Edit(this);
        }

        static IPEndPoint X32 = new IPEndPoint(IPAddress.Parse("192.168.1.147"), 10023);    // 10023);

        public override void Execute() {
            UdpClient client = new UdpClient();
            for (int Channel = 0; Channel < Channels.Length; Channel++) {
                if (Channels[Channel] != MuteState.Neutral) {
                    OSC osc = new OSC();
                    osc.path = $"/ch/{Channel + 1:D2}/mix/on";
                    osc.parameters.Add((Int32)((Channels[Channel] == MuteState.Mute)?0:1));
                    byte[] dgram = osc.GetBytes();
                    client.Send(dgram, dgram.Length, X32);
                }
            }
        }

        public void OnShowEventChanged() {
            ShowEventChanged?.Invoke(this, EventArgs.Empty);
        }

        public MuteState[] Channels = new MuteState[16] { MuteState.Neutral, MuteState.Neutral, MuteState.Neutral, MuteState.Neutral, MuteState.Neutral, MuteState.Neutral, MuteState.Neutral, MuteState.Neutral, MuteState.Neutral, MuteState.Neutral, MuteState.Neutral, MuteState.Neutral, MuteState.Neutral, MuteState.Neutral, MuteState.Neutral, MuteState.Neutral };

        public override event EventHandler ShowEventChanged;
    }
}
