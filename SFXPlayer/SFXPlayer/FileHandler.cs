using SFXPlayer.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace AJW.General {
    class XMLFileHandler<ObjectType> {
        const string AllFileExtensions = "All Files (*.*)|*.*";
        public string FileExtensions = "";
        public bool Dirty { get; private set; } = false;      //needs to point to object dirty flag

        internal event EventHandler FileTitleUpdate;

        public void SetDirty() {
            Dirty = true;
            OnFileTitleUpdate();
        }

        private void OnFileTitleUpdate() {
            FileTitleUpdate?.Invoke(this, new EventArgs());
        }

        public string CurrentFileName { get; private set; } = "";
        public string DisplayFileName {
            get {
                if (string.IsNullOrEmpty(CurrentFileName)) {
                    return "Untitled";
                }
                return Path.GetFileName(CurrentFileName);
            }
        }

        internal void NewFile() {
            CurrentFileName = "";
            Dirty = false;
            OnFileTitleUpdate();
        }

        internal ObjectType LoadFromFile() {
            OpenFileDialog of = new OpenFileDialog {
                Filter = String.Join("|", new string[] { FileExtensions, AllFileExtensions }),
                FileName = CurrentFileName,
                AddExtension = true
            };
            //to be useful as a module this needs to be separated from the Application
            if (Directory.Exists(Settings.Default.LastProjectFolder)) {
                of.InitialDirectory = Settings.Default.LastProjectFolder;
            }
            DialogResult result = of.ShowDialog();
            if (result == DialogResult.OK) {
                return LoadFromFile(of.FileName);
            }
            return default(ObjectType);     //null where allowed
        }

        internal ObjectType LoadFromFile(string FileName) {
            Settings.Default.LastAudioFolder = Path.GetDirectoryName(FileName); Settings.Default.Save();
            CurrentFileName = FileName;
            ObjectType temp = Load(CurrentFileName);     //needs to return new object
            Environment.CurrentDirectory = Path.GetDirectoryName(CurrentFileName);
            Dirty = false;
            OnFileTitleUpdate();
            return temp;
        }

        internal static ObjectType Load(string FileName) {
            ObjectType loadedfile = default(ObjectType);
            if (!File.Exists(FileName)) {
                return default(ObjectType);
            }
            try {
                XmlSerializer xs = new XmlSerializer(typeof(ObjectType));
                using (TextReader tr = new StreamReader(FileName)) {
                    loadedfile = (ObjectType)xs.Deserialize(tr);
                }
            } catch (Exception e) {
                MessageBox.Show(e.Message);
                loadedfile = default(ObjectType);
            }
            return loadedfile;
        }

        internal void Save(ObjectType theObject) {
            if (CurrentFileName == "") {
                SaveAs(theObject);
            } else {
                UntrackedSave(theObject, CurrentFileName);
                Dirty = false;
                OnFileTitleUpdate();
            }
        }

        internal static void UntrackedSave(ObjectType theObject, string FileName) {
            XmlSerializer xs = new XmlSerializer(typeof(ObjectType));
            using (TextWriter tw = new StreamWriter(FileName)) {
                xs.Serialize(tw, theObject);
            }
        }

        internal DialogResult SaveAs(ObjectType theObject) {
            SaveFileDialog sf = new SaveFileDialog {
                Filter = String.Join("|", new string[] { FileExtensions, AllFileExtensions }),
                FileName = CurrentFileName,
                AddExtension = true
            };
            if (Directory.Exists(Settings.Default.LastProjectFolder)) {
                sf.InitialDirectory = Settings.Default.LastProjectFolder;
            }
            DialogResult result = sf.ShowDialog();
            if (result == DialogResult.OK) {
                Settings.Default.LastAudioFolder = Path.GetDirectoryName(sf.FileName); Settings.Default.Save();
                CurrentFileName = sf.FileName;
                Save(theObject);
                return Dirty ? DialogResult.Cancel : DialogResult.OK;
            }
            return result;
        }

        internal DialogResult CheckSave(ObjectType theObject) {
            if (!Dirty) return DialogResult.OK;
            switch (MessageBox.Show("File has changed. Do you wish to save it?",
                Application.ProductName, MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.None, MessageBoxDefaultButton.Button3)) {
                case DialogResult.Yes:
                    break;
                case DialogResult.No:
                    return DialogResult.OK;
                case DialogResult.Cancel:
                    return DialogResult.Cancel;
            }
            if (CurrentFileName == "") {
                return SaveAs(theObject);
            } else {
                Save(theObject);
                return DialogResult.OK;
            }
        }

    }
}
