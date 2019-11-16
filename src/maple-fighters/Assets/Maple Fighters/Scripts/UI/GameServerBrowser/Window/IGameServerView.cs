using System;
using UnityEngine;

namespace Scripts.UI.GameServerBrowser
{
    public interface IGameServerView
    {
        event Action<int> ButtonClicked;

        GameObject GameObject { get; }

        void SetGameServerData(UIGameServerButtonData data);

        void SetGameServerName(string name);

        void SetGameServerConnections(int min, int max);

        void SelectButton();

        void DeselectButton();

        bool IsButtonSelected();

        UIGameServerButtonData GetGameServerData();
    }
}