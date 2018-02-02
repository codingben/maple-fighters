using System.Windows.Forms;

namespace PhotonControl
{
    internal class ServerDetails
    {
        public bool IsRunning { get; set; }
        public ToolStripMenuItem ServerMenu { get; }

        public ServerDetails(ToolStripMenuItem menu, bool isRunning = false)
        {
            ServerMenu = menu;
            IsRunning = isRunning;
        }
    }
}