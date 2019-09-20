using System;
using UnityEngine;

namespace Scripts.UI.GameServerBrowser
{
    public interface IGameServerBrowserView : IView
    {
        event Action JoinButtonClicked;

        event Action RefreshButtonClicked;

        IGameServerBrowserRefreshView RefreshImage { get; }

        Transform GameServerList { get; }

        void EnableJoinButton();

        void EnableRefreshButton();

        void DisableAllButtons();
    }
}