using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFXPlayer {
    public class OSC {
        public string path;
        public List<object> parameters = new List<object>();
        public byte[] GetBytes() {
            List<byte> bytes = new List<byte>();
            List<byte> paramValues = new List<byte>();
            bytes.AddRange(Encoding.ASCII.GetBytes(path));
            bytes.Add(0);           //null terminated
            while (bytes.Count % 4 != 0) bytes.Add(0);      //pad path to 32 bit

            bytes.AddRange(Encoding.ASCII.GetBytes(","));   //start of parameters

            //add list of parameter types
            foreach (var param in parameters) {
                switch (param) {
                    case Int32 l:       //int32
                        bytes.AddRange(Encoding.ASCII.GetBytes("i"));
                        if (BitConverter.IsLittleEndian) {
                            paramValues.AddRange(BitConverter.GetBytes(l).Reverse());
                        } else {
                            paramValues.AddRange(BitConverter.GetBytes(l));
                        }
                        break;
                    case Single l:      //float32
                        bytes.AddRange(Encoding.ASCII.GetBytes("f"));
                        if (BitConverter.IsLittleEndian) {
                            paramValues.AddRange(BitConverter.GetBytes(l).Reverse());
                        } else {
                            paramValues.AddRange(BitConverter.GetBytes(l));
                        }
                        break;
                    case string l:      //OSC-string
                        bytes.AddRange(Encoding.ASCII.GetBytes("s"));
                        paramValues.AddRange(Encoding.ASCII.GetBytes(l));
                        break;
                    case byte[] l:       //OSC-blob
                        bytes.AddRange(Encoding.ASCII.GetBytes("b"));
                        if (BitConverter.IsLittleEndian) {
                            paramValues.AddRange(BitConverter.GetBytes(l.Length).Reverse());
                        } else {
                            paramValues.AddRange(BitConverter.GetBytes(l.Length));
                        }
                        paramValues.AddRange(l);
                        break;
                    default:
                        break;
                }
                while (paramValues.Count % 4 != 0) bytes.Add(0);      //pad parameter values to 32 bit
            }
            while (bytes.Count % 4 != 0) bytes.Add(0);      //pad parameter list to 32 bit

            //add the parameter values
            bytes.AddRange(paramValues);        //already padded

            return bytes.ToArray();
        }
    }
}
