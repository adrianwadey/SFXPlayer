using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SFXPlayer {
    [Serializable]
    public class SFX {
        [DefaultValue("")]
        public string Description;
        public string FileName;
        [DefaultValue(false)]
        public bool StopOthers;
        [DefaultValue("")]
        public string MainText;
        [DefaultValue(50)]
        public int Volume = 50;

        public string ShortFileName {
            get {
                return Path.GetFileNameWithoutExtension(FileName);
            }
        }

    }
}
