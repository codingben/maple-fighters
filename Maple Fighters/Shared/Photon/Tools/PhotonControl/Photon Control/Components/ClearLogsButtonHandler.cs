using System.IO;
using System.Windows.Forms;
using ComponentModel.Common;

namespace PhotonControl.Components
{
    internal class ClearLogsButtonHandler : Component<IPhotonControl>
    {
        private const string PHOTON_SERVER_LOG_FOLDER = @"{0}\log";

        protected override void OnAwake()
        {
            base.OnAwake();

            Entity.ClearLogsButtonClicked += OnClearLogsButtonClicked;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            Entity.ClearLogsButtonClicked -= OnClearLogsButtonClicked;
        }

        private void OnClearLogsButtonClicked()
        {
            var path = string.Format(PHOTON_SERVER_LOG_FOLDER, Application.StartupPath);
            var directoryInfo = new DirectoryInfo(path);
            foreach (var log in directoryInfo.EnumerateFiles())
            {
                try
                {
                    log.Delete();
                }
                catch (IOException) // The log file may be in use, so skip this log file
                {
                    // Left blank intentionally
                }
            }
        }
    }
}