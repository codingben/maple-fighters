using System;
using UI;

namespace Scripts.UI.CharacterSelection
{
    public interface ICharacterSelectionView : IView
    {
        event Action<UICharacterClass> CharacterSelected;

        event Action ChooseButtonClicked;

        event Action CancelButtonClicked;

        void EnableChooseButton();

        void DisableChooseButton();
    }
}