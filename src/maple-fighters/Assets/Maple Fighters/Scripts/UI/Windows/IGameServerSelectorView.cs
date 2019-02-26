using System;
using UnityEngine;

namespace Scripts.UI.Windows
{
    public interface IGameServerSelectorView : IView
    {
        event Action JoinButtonClicked;

        event Action RefreshButtonClicked;

        IGameServerSelectorRefreshView RefreshImage { get; }

        Transform GameServerList { get; }

        void EnableJoinButton();

        void EnableRefreshButton();

        void DisableAllButtons();
    }
}