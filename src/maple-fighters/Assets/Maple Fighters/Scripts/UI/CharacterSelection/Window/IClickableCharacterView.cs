using System;
using UI.Manager;
using UnityEngine;

namespace Scripts.UI.CharacterSelection
{
    public interface IClickableCharacterView : IView
    {
        event Action<UICharacterIndex, bool> CharacterClicked;

        string CharacterName { set; }

        UICharacterIndex CharacterIndex { set; }

        UICharacterClass CharacterClass { set; }

        GameObject GameObject { get; }

        void PlayAnimation(UICharacterAnimation characterAnimation);
    }
}