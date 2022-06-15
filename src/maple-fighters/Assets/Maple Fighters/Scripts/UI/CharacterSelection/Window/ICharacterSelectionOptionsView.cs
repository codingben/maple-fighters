using System;
using UI;

namespace Scripts.UI.CharacterSelection
{
    public interface ICharacterSelectionOptionsView : IView
    {
        event Action ChooseCharacterButtonClicked;

        event Action CreateCharacterButtonClicked;

        event Action DeleteCharacterButtonClicked;

        void EnableOrDisableChooseCharacterButton(bool interactable);

        void EnableOrDisableCreateCharacterButton(bool interactable);

        void EnableOrDisableDeleteCharacterButton(bool interactable);
    }
}