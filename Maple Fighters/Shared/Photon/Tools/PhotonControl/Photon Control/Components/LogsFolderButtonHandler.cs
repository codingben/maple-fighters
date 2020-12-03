using System.Windows.Forms;
using ComponentModel.Common;

namespace PhotonControl.Components
{
    internal class LogsFolderButtonHandler : Component<IPhotonControl>
    {
        private const string PHOTON_SERVER_LOG_FOLDER = @"{0}\log";

        protected override void OnAwake()
        {
            base.OnAwake();

            Entity.LogsFolderButtonClicked += OnLogsFolderButtonClicked;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            Entity.LogsFolderButtonClicked -= OnLogsFolderButtonClicked;
        }

        private void OnLogsFolderButtonClicked()
        {
            var path = string.Format(PHOTON_SERVER_LOG_FOLDER, Application.StartupPath);
            Utils.RunProcess(path);
        }
    }
}