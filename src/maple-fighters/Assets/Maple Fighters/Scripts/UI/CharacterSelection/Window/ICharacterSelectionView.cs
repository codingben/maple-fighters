using System;
using UI;

namespace Scripts.UI.CharacterSelection
{
    public interface ICharacterSelectionView : IView
    {
        event Action<UICharacterClass> CharacterSelected;
    }
}