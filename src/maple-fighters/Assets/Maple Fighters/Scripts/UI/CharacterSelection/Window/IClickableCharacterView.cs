using System;
using UI;
using UnityEngine;

namespace Scripts.UI.CharacterSelection
{
    public interface IClickableCharacterView : IView
    {
        event Action<UICharacterIndex, bool> CharacterClicked;

        int Id { get; set; }

        string CharacterName { get; set; }

        int CharacterLevel { get; set; }

        float CharacterExperience { get; set; }

        UICharacterIndex CharacterIndex { get; set; }

        UICharacterClass CharacterClass { get; set; }

        GameObject GameObject { get; }

        void PlayAnimation(UICharacterAnimation characterAnimation);
    }
}