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
    [XmlInclude(typeof(MSCEvent))]
    [XmlInclude(typeof(X32MuteEvent))]

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
        [DefaultValue(""), XmlElement("FileName")] public string FilePath { get { return _FileName; } set { _FileName = value; SFXBecameDirty?.Invoke(); } }
        [DefaultValue(false)] public bool StopOthers { get { return _StopOthers; } set { _StopOthers = value; SFXBecameDirty?.Invoke(); } }
        [DefaultValue("")] public string MainText { get { return _MainText; } set { _MainText = value; SFXBecameDirty?.Invoke(); } }
        [DefaultValue(50)] public int Volume { get { return _Volume; } set { _Volume = value; SFXBecameDirty?.Invoke(); } }
        public BindingList<Trigger> Triggers { get; set; } = new BindingList<Trigger>();
        public string FileNameOnly {
            get {
                return Path.GetFileNameWithoutExtension(FilePath);
            }
        }
        public string FileName {
            get {
                return Path.GetFileName(FilePath);
            }
        }

    }
}
