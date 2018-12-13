using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SFXPlayer {
    [Serializable]
    class Show {
        public Panel Panel;
        public ObservableCollection<SFX> Cues = new ObservableCollection<SFX>();
        [NonSerialized]
        string FileName;
        public event NotifyShowChanged CueListChanged;
        public Show() {
            Cues.CollectionChanged += Cues_CollectionChanged;
        }
        //public event CueCollectionChanged 

        private void Cues_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e) {
            switch (e.Action) {
                case NotifyCollectionChangedAction.Add:
                    OnCueListChanged(this, );
                    break;
                case NotifyCollectionChangedAction.Move:
                    break;
                case NotifyCollectionChangedAction.Remove:
                    break;
                case NotifyCollectionChangedAction.Replace:
                    break;
                case NotifyCollectionChangedAction.Reset:
                    break;
            }
            Debug.WriteLine(e.Action);
            Debug.WriteLine("OldItems (" + e.OldItems?.Count + ") " + e.OldItems);
            Debug.WriteLine("NewItems (" + e.NewItems?.Count + ") " + e.NewItems);
            Debug.WriteLine("OldStartingIndex " + e.OldStartingIndex);
            Debug.WriteLine("NewStartingIndex " + e.NewStartingIndex);
        }

        private void OnCueListChanged(object sender, NotifyCollectionChangedEventArgs e) {
            CueListChanged?.Invoke(sender, e);
        }

        internal void AddCue(SFX SFX, int Index) {
            Cues.Insert(Index, SFX);
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
