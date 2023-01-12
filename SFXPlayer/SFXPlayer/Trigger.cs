using ExCSS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFXPlayer
{
    [Serializable]
    public class Trigger
    {
        public override string ToString()
        {
            return  Time.ToString(@"mm\:ss\.ff") + "\t" + Description;
        }

        internal void Edit()
        {
            TriggerEditor.Edit(this);
        }
        
        public string Description;
        public DateTime Time;
        public byte[] bytes = {0xf0, 0x7f, 0x7f, 0x02, 0x01, 0x01, 0xf7}; //go command
    }
}
