using System;

namespace Scripts.UI
{
    public interface IClickableCharacterView : IView
    {
        event Action<UICharacterIndex, bool> CharacterClicked;

        UICharacterIndex CharacterIndex { set; }

        string CharacterName { set; }

        bool HasCharacter { set; }

        void PlayAnimation(UICharacterAnimation characterAnimation);
    }
}