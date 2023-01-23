using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            SplashScreen ss = new SplashScreen();
#if !DEBUG
            ss.Show();
#endif  
            Properties.Settings.Default.Upgrade();
            mainForm = new SFXPlayer();
            Application.Run(mainForm);
        }

        public static SFXPlayer mainForm;
    }
}
