using System;
using System.Threading;
using System.Windows.Forms;
using CommonTools.Log;
using ComponentModel.Common;

namespace PhotonControl.Components
{
    internal class ServersController : Component<IPhotonControl>, IServersController
    {
        private const string PHOTON_SERVER_EXECUTABLE = @"{0}\PhotonSocketServer.exe";

        private IServersContainer serversContainer;

        protected override void OnAwake()
        {
            base.OnAwake();

            serversContainer = Entity.Components.AddComponent(new ServersContainer());
        }

        public void StartServer(string serverName, bool notify = true)
        {
            var path = string.Format(PHOTON_SERVER_EXECUTABLE, Application.StartupPath);
            var command = $"/debug {serverName}";

            var isRunning = Utils.RunProcess(path, command);
            if (isRunning)
            {
                var serverDetails = serversContainer.GetServerDetails(serverName);
                serverDetails.IsRunning = true;

                UpdateStateOfServerItemButtons(serverName, true);
            }

            if (notify)
            {
                var message = isRunning ? $"{serverName} server started." : $"{serverName} server is not started.";
                Entity.NotifyIcon.ShowBalloonTip(100, $"{DateTime.Now:HH:mm:ss tt}", message, ToolTipIcon.Info);
            }
        }

        public void StopServer(string serverName, bool notify = true)
        {
            var path = string.Format(PHOTON_SERVER_EXECUTABLE, Application.StartupPath);
            var command = $"/stop1 {serverName}";

            var isStopped = Utils.RunProcess(path, command);
            if (isStopped)
            {
                var serverDetails = serversContainer.GetServerDetails(serverName);
                serverDetails.IsRunning = false;

                UpdateStateOfServerItemButtons(serverName, false);
            }

            if (notify)
            {
                var message = isStopped ? $"{serverName} server stopped." : $"{serverName} server is not stopped.";
                Entity.NotifyIcon.ShowBalloonTip(100, $"{DateTime.Now:HH:mm:ss tt}", message, ToolTipIcon.Info);
            }
        }

        public void StartAllServers()
        {
            var count = 0;

            foreach (var serverDetails in serversContainer.GetAllServersDetails())
            {
                if (serverDetails.IsRunning)
                {
                    continue;
                }

                var serverName = serverDetails.ServerMenu.Name;
                StartServer(serverName, false);

                count++;

                Thread.Sleep(1000);
            }

            string message;

            switch (count)
            {
                case 0:
                {
                    message = "All servers are already running.";
                    break;
                }
                case 1:
                {
                    message = "1 server was started.";
                    break;
                }
                default:
                {
                    message = $"{count} servers were started.";
                    break;
                }
            }

            Entity.NotifyIcon.ShowBalloonTip(100, $"{DateTime.Now:HH:mm:ss tt}", message, ToolTipIcon.Info);
        }

        public void StopAllServers()
        {
            var count = 0;

            foreach (var serverDetails in serversContainer.GetAllServersDetails())
            {
                if (!serverDetails.IsRunning)
                {
                    continue;
                }

                var serverName = serverDetails.ServerMenu.Name;
                StopServer(serverName, false);

                count++;

                Thread.Sleep(1000);
            }

            string message;

            switch (count)
            {
                case 0:
                {
                    message = "All servers are already stopped.";
                    break;
                }
                case 1:
                {
                    message = "1 server was stopped.";
                    break;
                }
                default:
                {
                    message = $"{count} servers were stopped.";
                    break;
                }
            }

            Entity.NotifyIcon.ShowBalloonTip(100, $"{DateTime.Now:HH:mm:ss tt}", message, ToolTipIcon.Info);
        }

        private void UpdateStateOfServerItemButtons(string serverName, bool isRunning)
        {
            var serverDetails = serversContainer.GetServerDetails(serverName).AssertNotNull();
            if (serverDetails == null)
            {
                return;
            }

            const int START_BUTTON_INDEX = 0;
            const int STOP_BUTTON_INDEX = 1;

            serverDetails.ServerMenu.DropDownItems[START_BUTTON_INDEX].Enabled = !isRunning;
            serverDetails.ServerMenu.DropDownItems[STOP_BUTTON_INDEX].Enabled = isRunning;
        }
    }
}