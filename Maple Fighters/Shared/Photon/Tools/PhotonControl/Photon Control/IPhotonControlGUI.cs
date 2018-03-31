using System.Windows.Forms;

namespace PhotonControl
{
    internal interface IPhotonControlGUI
    {
        ToolStripMenuItem ServersMenu { get; }
        NotifyIcon NotifyIcon { get; }
    }
}