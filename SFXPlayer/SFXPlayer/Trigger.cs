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
            return  new TimeSpan(TimeTicks).ToString(@"mm\:ss\.ff") + "\t" + Description;
        }

        internal void Edit()
        {
            showEvent.Edit();
        }
        
        public string Description;
        public long TimeTicks;
        public AbstractShowEvent showEvent;
    }
}
