using System;
using UnityEngine;

namespace Scripts.UI.GameServerBrowser
{
    public interface IGameServerView
    {
        event Action<int> ButtonClicked;

        GameObject GameObject { get; }

        void SetIndex(int index);

        void SetGameServerButtonData(UIGameServerButtonData data);

        void SelectButton();

        void DeselectButton();

        bool IsButtonSelected();
    }
}