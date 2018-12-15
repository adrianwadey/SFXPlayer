using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace AJW.General {
    class XMLFileHandler<T> {
        const string AllFileExtensions = "All Files (*.*)|*.*";
        public string FileExtensions = "";
        public string CurrentFileName = "";
        public bool Dirty = false;      //needs to point to object dirty flag

        internal T LoadFromFile() {
            OpenFileDialog of = new OpenFileDialog {
                Filter = String.Join("|", new string[] { FileExtensions, AllFileExtensions }),
                FileName = CurrentFileName,
                AddExtension = true
            };
            DialogResult result = of.ShowDialog();
            if (result == DialogResult.OK) {
                CurrentFileName = of.FileName;
                return Load();     //needs to return new object
            }
            return default(T);
        }

        private T Load() {
            T loadedfile = default(T);
            if (!File.Exists(CurrentFileName)) {
                return default(T);
            }
            try {
                XmlSerializer xs = new XmlSerializer(typeof(T));
                TextReader tr = new StreamReader(CurrentFileName);
                loadedfile = (T)xs.Deserialize(tr);
                Dirty = false;
            } catch (Exception e) {
                MessageBox.Show(e.Message);
                loadedfile = default(T);
            }
            return loadedfile;
        }

        internal void Save(T O) {
            if (CurrentFileName == "") {
                SaveAs(O);
            } else {
                //needs access to object
                XmlSerializer xs = new XmlSerializer(typeof(T));
                TextWriter tw = new StreamWriter(CurrentFileName);
                xs.Serialize(tw, O);
                Dirty = false;
            }
        }

        internal DialogResult SaveAs(T O) {
            SaveFileDialog sf = new SaveFileDialog {
                Filter = String.Join("|", new string[] { FileExtensions, AllFileExtensions }),
                FileName = CurrentFileName,
                AddExtension = true
            };
            DialogResult result = sf.ShowDialog();
            if (result == DialogResult.OK) {
                CurrentFileName = sf.FileName;
                Save(O);
                return Dirty ? DialogResult.Cancel : DialogResult.OK;
            }
            return result;
        }

        internal DialogResult CheckSave(T O) {
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
                return SaveAs(O);
            } else {
                Save(O);
                return DialogResult.OK;
            }
        }
    }
}
