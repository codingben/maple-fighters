using System.Windows.Forms;
using ComponentModel.Common;

namespace PhotonControl
{
    internal interface IServersMenu : IExposableComponent
    {
        ToolStripMenuItem AddServerItemToServersMenu(string serverName);
        void RemoveServerItemFromServersMenu(string serverName);
    }
}