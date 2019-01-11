using System;
using Game.Common;
using UI.Manager;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Scripts.UI
{
    [RequireComponent(typeof(Animator))]
    public class ClickableCharacter : UIElement, IPointerClickHandler
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
            PlayWalkAnimation();

            CharacterClicked?.Invoke(character.GetValueOrDefault(), index);
        }

        private void PlayIdleAnimation()
        {
            animator.SetBool("Walk", false);
        }

        private void PlayWalkAnimation()
        {
            animator.SetBool("Walk", true);
        }
    }
}