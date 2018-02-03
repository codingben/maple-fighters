using System.Windows.Forms;
using ComponentModel.Common;

namespace PhotonControl
{
    internal class ExitButtonHandler : Component<IPhotonControl>
    {
        protected override void OnAwake()
        {
            base.OnAwake();

            Entity.ExitButtonClicked += OnExitButtonClicked;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            Entity.ExitButtonClicked -= OnExitButtonClicked;
        }

        private void OnExitButtonClicked()
        {
            const string PHOTON_SERVER_EXECUTABLE = @"{0}\PhotonSocketServer.exe";

            var path = string.Format(PHOTON_SERVER_EXECUTABLE, Application.StartupPath);
            var stopped = Utils.RunProcess(path, "/stop");
            if (stopped)
            {
                Application.Exit();
            }
            else
            {
                var message = "Could not stop servers.";
                Entity.NotifyIcon.ShowBalloonTip(1000, null, message, ToolTipIcon.Warning);
            }
        }
    }
}