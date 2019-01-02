using AJW.General;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace SFXPlayer {
    class SFXShowEvent {

    }
    [Serializable]
    public class Show {
        public ObservableCollection<SFX> Cues = new ObservableCollection<SFX>();
        public event Action UpdateShow;
        [DefaultValue(0)]
        public int NextPlayCueIndex;
        internal Action ShowFileBecameDirty;

        public Show() {
            Cues.CollectionChanged += Cues_CollectionChanged;
        }

        private void OnUpdateShow() {
            UpdateShow?.Invoke();
        }

        //public event CueCollectionChanged 

        private void Cues_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e) {
            switch (e.Action) {
                case NotifyCollectionChangedAction.Add:
                    foreach (SFX item in e.NewItems) {
                        item.SFXBecameDirty += ShowFileBecameDirty;
                    }
                    break;
                case NotifyCollectionChangedAction.Move:
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (SFX item in e.OldItems) {
                        item.SFXBecameDirty -= ShowFileBecameDirty;
                    }
                    break;
                case NotifyCollectionChangedAction.Replace:
                    foreach (SFX item in e.OldItems) {
                        item.SFXBecameDirty -= ShowFileBecameDirty;
                    }
                    foreach (SFX item in e.NewItems) {
                        item.SFXBecameDirty += ShowFileBecameDirty;
                    }
                    break;
                case NotifyCollectionChangedAction.Reset:
                    foreach (SFX item in e.OldItems) {
                        item.SFXBecameDirty -= ShowFileBecameDirty;
                    }
                    break;
            }
            OnUpdateShow();
            OnShowFileBecameDirty();            //Dirty = true; need to set it in filehandler
            //Debug.WriteLine(e.Action);
            //Debug.WriteLine("OldItems (" + e.OldItems?.Count + ") " + e.OldItems);
            //Debug.WriteLine("NewItems (" + e.NewItems?.Count + ") " + e.NewItems);
            //Debug.WriteLine("OldStartingIndex " + e.OldStartingIndex);
            //Debug.WriteLine("NewStartingIndex " + e.NewStartingIndex);
        }

        private void OnShowFileBecameDirty() {
            ShowFileBecameDirty?.Invoke();
        }

        internal void AddCue(SFX SFX, int Index) {
            Cues.Insert(Index, SFX);
        }

        internal void MoveCue(int fromIndex, int toIndex) {
            Cues.Move(fromIndex, toIndex);
        }

        internal string CreateArchive(string CurrentFileName) {
            string tempDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            string tempArchiveFileName = Path.Combine(Path.GetTempPath(), Path.GetFileNameWithoutExtension(CurrentFileName) + ".show");
            Directory.CreateDirectory(tempDirectory);
            Program.mainForm.ReportStatus("Creating Archive: Copying files");
            //get a list of the sound files
            List<string> archCues = new List<string>();
            foreach (SFX cue in Cues) {
                archCues.Add(cue.FileName);
            }
            archCues = archCues.Distinct().ToList();    //only need one copy of each file
            foreach (string audioFileName in archCues) {
                if (!string.IsNullOrEmpty(audioFileName)) {
                    Program.mainForm.ReportStatus("Creating Archive: Copying " + Path.GetFileName(audioFileName));
                    File.Copy(audioFileName, Path.Combine(tempDirectory, Path.GetFileName(audioFileName)));
                }
            }

            Program.mainForm.ReportStatus("Creating Archive: Copying Cue List");
            //save a copy of the xml show file with audio file paths removed
            XMLFileHandler<Show>.UntrackedSave(this, Path.Combine(tempDirectory, Path.GetFileName(CurrentFileName)));
            Show tempShow = XMLFileHandler<Show>.Load(Path.Combine(tempDirectory, Path.GetFileName(CurrentFileName)));
            foreach (SFX cue in tempShow.Cues) {
                cue.FileName = Path.GetFileName(cue.FileName);      //remove the path
            }
            XMLFileHandler<Show>.UntrackedSave(tempShow, Path.Combine(tempDirectory, Path.GetFileName(CurrentFileName)));

            //all files now in temp folder
            Program.mainForm.ReportStatus("Creating Archive: Combining Files");
            ZipFile.CreateFromDirectory(tempDirectory, tempArchiveFileName);

            Directory.Delete(tempDirectory, true);
            Program.mainForm.ReportStatus("Creating Archive: Archive Complete");
            return tempArchiveFileName;
        }

        internal static string ExtractArchive(string fnArchive, string ShowFolder) {
            ZipFile.ExtractToDirectory(fnArchive, ShowFolder);
            DirectoryInfo directory = new DirectoryInfo(ShowFolder);
            FileInfo[] fi = directory.GetFiles("*.sfx");
            if (fi.Count() > 0) return fi[0].FullName;

            FileInfo fileToDecompress = new FileInfo(fnArchive);
            using (FileStream originalFileStream = fileToDecompress.OpenRead()) {
                string currentFileName = fileToDecompress.FullName;
                string newFileName = currentFileName.Remove(currentFileName.Length - fileToDecompress.Extension.Length);

                using (FileStream decompressedFileStream = File.Create(newFileName)) {
                    using (GZipStream decompressionStream = new GZipStream(originalFileStream, CompressionMode.Decompress)) {
                        decompressionStream.CopyTo(decompressedFileStream);
                        Console.WriteLine("Decompressed: {0}", fileToDecompress.Name);
                    }
                }
            }
            return "";
        }

        internal void RemoveCue(SFX sfx) {
            Cues.Remove(sfx);
        }
    }
}
