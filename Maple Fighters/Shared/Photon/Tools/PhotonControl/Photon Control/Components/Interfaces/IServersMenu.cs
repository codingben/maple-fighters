using System.Windows.Forms;

namespace PhotonControl.Components.Interfaces
{
    internal interface IServersMenu
    {
        ToolStripMenuItem AddServerItemToServersMenu(string serverName);
        void RemoveServerItemFromServersMenu(string serverName);
    }
}