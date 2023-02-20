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

namespace SFXPlayer {
    [Serializable]
    public class MSCEvent : AbstractShowEvent {
        public MSCEvent() {

        }

        public override void Edit() {
            if (MIDIBytes == null || MIDIBytes.Length == 0) {
                MIDIBytes = (byte[])SimpleGo.Clone();
            }
            MSCEventEditor.Edit(this);
        }

        public override void Execute() {
            if (MIDIBytes != null) Debug.WriteLine("[" + BitConverter.ToString(MIDIBytes) + "]");
            if (Program.mainForm.MIDIOut != null) {
                if (MIDIBytes != null && MIDIBytes.Any()) {
                    Program.mainForm.MIDIOut.SendBuffer(MIDIBytes);
                }
            }
        }

        [XmlIgnore]
        public byte[] MIDIBytes;

        [XmlElement("MIDIBytes"), DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public string use_MIDIBytes_not_this {
            get {
                return ByteArrayToCSV(MIDIBytes);
            }
            set {
                MIDIBytes = CSVToByteArray(value);
            }
        }

        public static string ByteArrayToCSV(byte[] bytes) {
            if (bytes == null || bytes.Length == 0) return "";
            return BitConverter.ToString(bytes).Replace("-", ",");
        }

        public static byte[] CSVToByteArray(string CSV) {
            byte[] result;
            string[] bytes = CSV.Replace(" ", "").Split(',');       //replace spaces in case manually edited
            if (bytes[0].Length == 0) {
                result = null;
            } else {
                result = new byte[bytes.Length];
                for (int i = 0; i < bytes.Length; i++) {
                    result[i] = Convert.ToByte(bytes[i], 16);
                }
            }
            return result;
        }

        private static byte[] SimpleGo = { 0xf0, 0x7f, 0x7f, 0x02, 0x01, 0x01, 0xf7 }; //go command

        public override event EventHandler ShowEventChanged;

        public override string ToString() {
            if (MIDIBytes == null || MIDIBytes.Length == 0) {
                return "Empty";
            }
            string desc = "";
            if (MIDIBytes[0] == 0xF0 && MIDIBytes[1] == 0x7f && MIDIBytes[3] == 0x02) {
                desc = "MSC:";

                if (MIDIBytes[2] <= 0x6F) {
                    desc += "Device " + MIDIBytes[2].ToString("X2") + ":";
                } else if (MIDIBytes[2] <= 0x7E) {
                    desc += "Group " + MIDIBytes[2].ToString("X2") + ":";
                } else if (MIDIBytes[2] == 0x7F) {
                    desc += "Broadcast " + MIDIBytes[2].ToString("X2") + ":";
                }

                if (MIDIBytes[4] == 0x01) desc += "Lighting:";

                if (MIDIBytes[5] == 0x01) {
                    desc += "Go";
                } else if (MIDIBytes[5] == 0x02) {
                    desc += "Stop";
                } else if (MIDIBytes[5] == 0x03) {
                    desc += "Resume";
                } else if (MIDIBytes[5] == 0x04) {
                    desc += "Timed Go";
                } else if (MIDIBytes[5] == 0x05) {
                    desc += "Load";
                }

                if (MIDIBytes[6] != 0xF7) {
                    desc += ":";
                }
                int n = 6;
                while (n < MIDIBytes.Length) {
                    if (MIDIBytes[n] == 0xF7) {
                        break;
                    }
                    desc += Encoding.ASCII.GetString(MIDIBytes, n, 1);
                    n++;
                }
                return desc;
            }

            return "MIDI " + BitConverter.ToString(MIDIBytes);

        }
    }
}