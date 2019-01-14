using System;
using Game.Common;
using UI.Manager;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Scripts.UI
{
    [RequireComponent(typeof(Animator), typeof(UIFadeAnimation))]
    public class ClickableCharacterImage : UIElement, IPointerClickHandler
    {
        public event Action<CharacterParameters, int> CharacterClicked;

        private int index;
        private CharacterParameters? character;

        private Animator animator;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        private void Start()
        {
            Hidden += OnHidden;
        }

        private void OnDestroy()
        {
            Hidden -= OnHidden;
        }

        private void OnHidden()
        {
            Destroy(gameObject);
        }

        public void SetCharacter(int index, CharacterParameters character)
        {
            this.index = index;
            this.character = character;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            CharacterClicked?.Invoke(character.GetValueOrDefault(), index);
        }

        public void PlayAnimation(UiCharacterAnimation characterAnimation)
        {
            switch (characterAnimation)
            {
                case UiCharacterAnimation.Idle:
                {
                    animator.SetBool("Walk", false);
                    break;
                }

                case UiCharacterAnimation.Walk:
                {
                    animator.SetBool("Walk", true);
                    break;
                }
            }
        }
    }
}