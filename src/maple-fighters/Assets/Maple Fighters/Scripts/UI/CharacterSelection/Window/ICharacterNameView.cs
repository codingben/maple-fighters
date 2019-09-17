using System;

namespace Scripts.UI.Windows
{
    public interface ICharacterNameView : IView
    {
        event Action<string> ConfirmButtonClicked;

        event Action BackButtonClicked;

        event Action<string> NameInputFieldChanged;

        void EnableConfirmButton();

        void DisableConfirmButton();
    }
}