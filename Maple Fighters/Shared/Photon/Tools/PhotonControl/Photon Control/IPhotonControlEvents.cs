using System;

namespace PhotonControl
{
    internal interface IPhotonControlEvents
    {
        event Action ClearLogsButtonClicked;
        event Action LogsFolderButtonClicked;
        event Action ExitButtonClicked;
    }
}