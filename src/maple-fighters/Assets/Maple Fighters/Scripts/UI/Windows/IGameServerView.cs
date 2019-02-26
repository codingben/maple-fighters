using System;

namespace Scripts.UI
{
    public interface IGameServerView
    {
        event Action<string> ButtonClicked;

        void SetGameServerButtonData(UIGameServerButtonData data);

        void SelectButton();

        void DeselectButton();

        bool IsButtonSelected();
    }
}