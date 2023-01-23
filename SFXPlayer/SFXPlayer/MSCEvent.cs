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
            if (MIDIBytes == null || MIDIBytes.Length == 0) {
                MIDIBytes = (byte[])SimpleGo.Clone();
            }
            MSCEventEditor.Edit(this);
        }

        public override void Execute()
        {
            if (MIDIBytes != null) Debug.WriteLine("[" + BitConverter.ToString(MIDIBytes) + "]");
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
                return ByteArrayToCSV(MIDIBytes);
            }
            set
            {
                MIDIBytes= CSVToByteArray(value);
            }
        }

        public static string ByteArrayToCSV(byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0) return "";
            return BitConverter.ToString(bytes).Replace("-", ",");
        }

        public static byte[] CSVToByteArray(string CSV)
        {
            byte[] result;
            string[] bytes = CSV.Replace(" ", "").Split(',');       //replace spaces in case manually edited
            if (bytes[0].Length == 0)
            {
                result = null;
            }
            else
            {
                result = new byte[bytes.Length];
                for (int i = 0; i < bytes.Length; i++)
                {
                    result[i] = Convert.ToByte(bytes[i], 16);
                }
            }
            return result;
        }

        private static byte[] SimpleGo = { 0xf0, 0x7f, 0x7f, 0x02, 0x01, 0x01, 0xf7 }; //go command
    }
}