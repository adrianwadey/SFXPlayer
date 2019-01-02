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
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            SplashScreen ss = new SplashScreen();
            ss.Show();

            Properties.Settings.Default.Upgrade();

            mainForm = new Form1();
            Application.Run(mainForm);
        }

        public static Form1 mainForm;
    }
}
