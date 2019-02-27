using System;

namespace Scripts.UI
{
    public interface IClickableCharacterView
    {
        event Action<UICharacterIndex, bool> CharacterClicked;

        UICharacterIndex CharacterIndex { set; }

        string CharacterName { set; }

        void PlayAnimation(UICharacterAnimation characterAnimation);
    }
}