using System;
using UI.Manager;
using UnityEngine;

namespace Scripts.UI.CharacterSelection
{
    public interface IClickableCharacterView : IView
    {
        event Action<UICharacterIndex, bool> CharacterClicked;

        UICharacterIndex CharacterIndex { set; }

        string CharacterName { set; }

        bool HasCharacter { set; }

        GameObject GameObject { get; }

        void PlayAnimation(UICharacterAnimation characterAnimation);
    }
}