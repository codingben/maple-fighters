using System.Windows.Forms;

namespace PhotonControl
{
    internal interface IServersMenu
    {
        ToolStripMenuItem AddServerItemToServersMenu(string serverName);
        void RemoveServerItemFromServersMenu(string serverName);
    }
}