using Fizzler;
using NAudio.Midi;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace SFXPlayer
{
    [Serializable]
    public class MSCEvent : AbstractShowEvent
    {
        public MSCEvent()
        {

        }

        public override void Edit()
        {
            MIDIBytes = (byte[])SimpleGo.Clone();
            MSCEventEditor.Edit(this);
        }

        public override void Execute()
        {
            Debug.WriteLine(BitConverter.ToString(SimpleGo));
            if (Program.mainForm.MIDIOut != null)
            {
                if (MIDIBytes!=null && MIDIBytes.Any())
                {
                    Program.mainForm.MIDIOut.SendBuffer(MIDIBytes);
                }
            }
        }

        [XmlIgnore]
        public byte[] MIDIBytes;

        [XmlElement("MIDIBytes"),DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public string use_MIDIBytes_not_this
        {
            get
            {
                if (MIDIBytes == null || MIDIBytes.Length == 0) return "";
                return BitConverter.ToString(MIDIBytes).Replace("-", ",");
            }
            set
            {
                string[] bytes = value.Replace(" ", "").Split(',');       //replace spaces in case manually edited
                if (bytes[0].Length == 0)
                {
                    MIDIBytes = null;
                }
                else
                {
                    MIDIBytes = new byte[bytes.Length];
                    for (int i = 0; i < bytes.Length; i++)
                    {
                        MIDIBytes[i] = Convert.ToByte(bytes[i], 16);
                    }
                }
            }
        }

        private static byte[] SimpleGo = { 0xf0, 0x7f, 0x7f, 0x02, 0x01, 0x01, 0xf7 }; //go command
    }
}