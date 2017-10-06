using System;
using Scripts.UI.Controllers;
using Scripts.UI.Core;
using UnityEngine.EventSystems;

namespace Scripts.UI
{
    public class ClickableCharacter : UserInterfaceBaseFadeEffect, IPointerClickHandler
    {
        public event Action<Character, int> CharacterClicked;

        private int index;
        private Character character;

        private void Start()
        {
            Show();
        }

        public void SetCharacter(int index, Character character)
        {
            this.index = index;
            this.character = character;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            CharacterClicked?.Invoke(character, index);
        }

        private void OnDestroy()
        {
            CharacterClicked = null;
        }
    }
}