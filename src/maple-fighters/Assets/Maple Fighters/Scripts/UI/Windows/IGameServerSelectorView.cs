using System;
using UnityEngine;

namespace Scripts.UI.Windows
{
    public interface IGameServerSelectorView : IView
    {
        event Action JoinButtonClicked;

        event Action RefreshButtonClicked;

        GameServerSelectorRefreshImage RefreshImage { get; }

        Transform GameServerList { get; }

        void EnableJoinButton();

        void EnableRefreshButton();

        void DisableAllButtons();
    }
}