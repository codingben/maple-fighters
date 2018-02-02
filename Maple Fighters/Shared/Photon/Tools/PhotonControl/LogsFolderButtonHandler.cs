using System.Windows.Forms;
using CommonTools.Log;
using ComponentModel.Common;

namespace PhotonControl
{
    internal class LogsFolderButtonHandler : Component
    {
        private const string PHOTON_SERVER_LOG_FOLDER = @"{0}\log";

        protected override void OnAwake()
        {
            base.OnAwake();

            var photonControl = Entity.GetComponent<IPhotonControl>().AssertNotNull();
            photonControl.LogsFolderButtonClicked += OnLogsFolderButtonClicked;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            var photonControl = Entity.GetComponent<IPhotonControl>().AssertNotNull();
            photonControl.LogsFolderButtonClicked -= OnLogsFolderButtonClicked;
        }

        private void OnLogsFolderButtonClicked()
        {
            var path = string.Format(PHOTON_SERVER_LOG_FOLDER, Application.StartupPath);
            Utils.RunProcess(path);
        }
    }
}