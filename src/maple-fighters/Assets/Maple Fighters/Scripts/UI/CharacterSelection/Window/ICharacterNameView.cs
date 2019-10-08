using System;
using UI.Manager;

namespace Scripts.UI.CharacterSelection
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