using System;
using System.Windows.Forms;
using CommonTools.Log;
using ComponentModel.Common;

namespace PhotonControl
{
    internal static class Program
    {
        private static readonly IContainer Components = new Container();

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            LogUtils.Logger = new Logger();

            SubscribeToProcessExitEvent();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var form = Components.AddComponent(new PhotonControl());
            Components.AddComponent(new ServersController());
            Components.AddComponent(new ServersCreator());
            Components.AddComponent(new ExitButtonHandler());
            Components.AddComponent(new LogsFolderButtonHandler());

            Application.Run(form);
        }

        private static void SubscribeToProcessExitEvent()
        {
            AppDomain.CurrentDomain.ProcessExit += OnProcessExit;
        }

        private static void UnsubscribeFromProcessExitEvent()
        {
            AppDomain.CurrentDomain.ProcessExit -= OnProcessExit;
        }

        private static void OnProcessExit(object sender, EventArgs e)
        {
            Components.Dispose();
            UnsubscribeFromProcessExitEvent();
        }
    }
}