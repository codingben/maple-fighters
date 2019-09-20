using System;

namespace Scripts.UI.CharacterSelection
{
    public interface ICharacterSelectionOptionsView : IView
    {
        event Action StartButtonClicked;

        event Action CreateCharacterButtonClicked;

        event Action DeleteCharacterButtonClicked;

        void EnableOrDisableStartButton(bool interactable);

        void EnableOrDisableCreateCharacterButton(bool interactable);

        void EnableOrDisableDeleteCharacterButton(bool interactable);
    }
}