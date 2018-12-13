using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFXPlayer {
    [Serializable]
    public class SFX {
        [DefaultValue("")]
        public string Description;
        public string FileName;
        [DefaultValue(false)]
        public bool Loop;

        public string ShortFileName {
            get {
                return Path.GetFileNameWithoutExtension(FileName);
            }
        }
    }
}
