using System;
using System.Windows.Forms;

namespace RIA.GUI
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var compositionRoot = new CompositionRoot();
            compositionRoot.Initialize();
            Application.Run(compositionRoot.MainForm);
        }
    }
}
