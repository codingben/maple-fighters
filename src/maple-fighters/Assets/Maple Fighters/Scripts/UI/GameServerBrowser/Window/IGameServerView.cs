using System;
using UnityEngine;

namespace Scripts.UI.GameServerBrowser
{
    public interface IGameServerView
    {
        event Action<UIGameServerButtonData> ButtonClicked;

        GameObject GameObject { get; }

        void SetGameServerData(UIGameServerButtonData gameServerData);

        void SetGameServerName(string name);

        void SetGameServerConnections(int min, int max);

        void SelectButton();

        void DeselectButton();

        bool IsButtonSelected();

        UIGameServerButtonData GetGameServerData();
    }
}