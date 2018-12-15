using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
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
        [XmlIgnore] public Panel Panel;
        public ObservableCollection<SFX> Cues = new ObservableCollection<SFX>();
        public event Action UpdateShow;

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

                    break;
                case NotifyCollectionChangedAction.Move:
                    break;
                case NotifyCollectionChangedAction.Remove:
                    break;
                case NotifyCollectionChangedAction.Replace:
                    break;
                case NotifyCollectionChangedAction.Reset:
                    OnUpdateShow();
                    break;
            }
            //Dirty = true; need to set it in filehandler
            Debug.WriteLine(e.Action);
            Debug.WriteLine("OldItems (" + e.OldItems?.Count + ") " + e.OldItems);
            Debug.WriteLine("NewItems (" + e.NewItems?.Count + ") " + e.NewItems);
            Debug.WriteLine("OldStartingIndex " + e.OldStartingIndex);
            Debug.WriteLine("NewStartingIndex " + e.NewStartingIndex);
        }

        internal void AddCue(SFX SFX, int Index) {
            Cues.Insert(Index, SFX);
        }

        internal void MoveCue(int fromIndex, int toIndex) {
            Cues.Move(fromIndex,toIndex);
        }

        internal DialogResult DeleteCue(int Index) {
            DialogResult Response = MessageBox.Show("Delete Cue?" + Environment.NewLine + Cues[Index].Description, "Cue List", MessageBoxButtons.YesNo);
            if (Response == DialogResult.Yes) {
                Cues.RemoveAt(Index);
            }
            return Response;
        }
    }

    public class ShowChangeArgs : EventArgs {
        enum ShowChangeType {
            Add, Move, Remove, Replace

        }

        private string message;

        public ShowChangeArgs(string message) {
            this.message = message;
        }

        // This is a straightforward implementation for 
        // declaring a public field
        public string Message {
            get {
                return message;
            }
        }
    }
    public class NotifyShowChanged {
    }
}
