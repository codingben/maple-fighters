using System;

namespace Scripts.UI.Windows
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