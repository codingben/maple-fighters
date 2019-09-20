using System;
using UnityEngine;

namespace Scripts.UI.GameServerBrowser
{
    public interface IGameServerView
    {
        event Action<string> ButtonClicked;

        GameObject GameObject { get; }

        void SetGameServerButtonData(UIGameServerButtonData data);

        void SelectButton();

        void DeselectButton();

        bool IsButtonSelected();
    }
}