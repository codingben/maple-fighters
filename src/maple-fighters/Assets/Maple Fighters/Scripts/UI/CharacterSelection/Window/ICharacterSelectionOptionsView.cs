using System;
using UI;

namespace Scripts.UI.CharacterSelection
{
    public interface ICharacterSelectionOptionsView : IView
    {
        event Action JoinGameButtonClicked;

        event Action CreateCharacterButtonClicked;

        event Action DeleteCharacterButtonClicked;

        void EnableOrDisableJoinGameButton(bool interactable);

        void EnableOrDisableCreateCharacterButton(bool interactable);

        void EnableOrDisableDeleteCharacterButton(bool interactable);
    }
}