using System.Windows.Forms;
using CommonTools.Log;
using ComponentModel.Common;

namespace PhotonControl
{
    internal class ExitButtonHandler : Component
    {
        private IPhotonControl photonControl;

        protected override void OnAwake()
        {
            base.OnAwake();

            photonControl = Entity.GetComponent<IPhotonControl>().AssertNotNull();
            photonControl.ExitButtonClicked += OnExitButtonClicked;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            photonControl.ExitButtonClicked -= OnExitButtonClicked;
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
                photonControl.NotifyIcon.ShowBalloonTip(1000, null, message, ToolTipIcon.Warning);
            }
        }
    }
}