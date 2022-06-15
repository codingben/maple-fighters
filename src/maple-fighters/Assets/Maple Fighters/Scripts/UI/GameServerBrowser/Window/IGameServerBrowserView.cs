using System;
using UI;
using UnityEngine;

namespace Scripts.UI.GameServerBrowser
{
    public interface IGameServerBrowserView : IView
    {
        event Action JoinGameButtonClicked;

        event Action RefreshButtonClicked;

        IGameServerBrowserRefreshView RefreshImage { get; }

        Transform GameServerList { get; }

        void EnableJoinGameButton();

        void DisableJoinGameButton();

        void EnableRefreshButton();

        void DisableRefreshButton();
    }
}