using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFXPlayer
{
    [Serializable]
    public abstract class AbstractShowEvent
    {

        public abstract void Execute();
        public abstract void Edit();

    }
}
