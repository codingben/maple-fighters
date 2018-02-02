using System;
using System.Windows.Forms;
using ComponentModel.Common;

namespace PhotonControl
{
    internal interface IPhotonControl : IExposableComponent
    {
        ToolStripMenuItem ServersMenu { get; }
        NotifyIcon NotifyIcon { get; }

        event Action LogsFolderButtonClicked;
        event Action ExitButtonClicked;
    }
}