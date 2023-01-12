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
        internal Action SFXBecameDirty;
        private string _Description = "";
        private string _FileName = "";
        private bool _StopOthers = false;
        private string _MainText = "";
        private int _Volume = 50;
        public SFX()
        {
            Triggers.ListChanged += Triggers_ListChanged;
        }

        private void Triggers_ListChanged(object sender, ListChangedEventArgs e)
        {
            SFXBecameDirty?.Invoke();
        }

        [DefaultValue("")] public string Description { get { return _Description; } set { _Description = value; SFXBecameDirty?.Invoke(); } }
        [DefaultValue("")] public string FileName { get { return _FileName; } set { _FileName = value; SFXBecameDirty?.Invoke(); } }
        [DefaultValue(false)] public bool StopOthers { get { return _StopOthers; } set { _StopOthers = value; SFXBecameDirty?.Invoke(); } }
        [DefaultValue("")] public string MainText { get { return _MainText; } set { _MainText = value; SFXBecameDirty?.Invoke(); } }
        [DefaultValue(50)] public int Volume { get { return _Volume; } set { _Volume = value; SFXBecameDirty?.Invoke(); } }
        public BindingList<Trigger> Triggers { get; set; } = new BindingList<Trigger>();
        public string ShortFileNameOnly {
            get {
                return Path.GetFileNameWithoutExtension(FileName);
            }
        }
        public string ShortFileName {
            get {
                return Path.GetFileName(FileName);
            }
        }

    }
}
