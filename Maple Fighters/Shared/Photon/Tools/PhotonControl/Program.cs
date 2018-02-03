using System;
using System.Windows.Forms;
using CommonTools.Log;

namespace PhotonControl
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            LogUtils.Logger = new Logger();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new PhotonControl());
        }
    }
}