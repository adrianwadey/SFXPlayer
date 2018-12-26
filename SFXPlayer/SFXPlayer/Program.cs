using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SFXPlayer {
    static class Program {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {
            //FocusTracker focusTracker = new FocusTracker();
            Properties.Settings.Default.Upgrade();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.AddMessageFilter(focusTracker);
            mainForm = new Form1();
            Application.Run(mainForm);
            //Application.RemoveMessageFilter(focusTracker);
        }

        public static Form1 mainForm;
    }
}
