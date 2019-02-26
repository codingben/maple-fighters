using System;

namespace Scripts.UI
{
    public interface IClickableCharacterView
    {
        event Action<UICharacterClass> CharacterClicked;

        UICharacterClass CharacterClass { set; }

        string CharacterName { set; }

        void PlayAnimation(UICharacterAnimation characterAnimation);
    }
}