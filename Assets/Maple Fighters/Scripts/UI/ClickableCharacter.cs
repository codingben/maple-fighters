using System;
using CommonTools.Log;
using Scripts.UI.Controllers;
using Scripts.UI.Core;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Scripts.UI
{
    public class ClickableCharacter : UserInterfaceBaseFadeEffect, IPointerClickHandler
    {
        public event Action<Character, int> CharacterClicked;

        private int index;
        private Character character;

        private Animator animator;

        private void Start()
        {
            animator = GetComponent<Animator>();

            Show();

            SubscribeToMouseDetectionBackgroundEvent();
        }

        private void OnDestroy()
        {
            CharacterClicked = null;

            UnsubscribeFromMouseDetectionBackgroundEvent();
        }

        private void SubscribeToMouseDetectionBackgroundEvent()
        {
            var screenMouseDetection = UserInterfaceContainer.Instance.Get<MouseDetectionBackground>().AssertNotNull();
            screenMouseDetection.MouseClicked += PlayIdleAnimation;
        }

        private void UnsubscribeFromMouseDetectionBackgroundEvent()
        {
            var screenMouseDetection = UserInterfaceContainer.Instance.Get<MouseDetectionBackground>().AssertNotNull();
            screenMouseDetection.MouseClicked -= PlayIdleAnimation;
        }

        public void SetCharacter(int index, Character character)
        {
            this.index = index;
            this.character = character;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            PlayWalkAnimation();

            CharacterClicked?.Invoke(character, index);
        }

        private void PlayIdleAnimation()
        {
            const string ANIMATION_NAME = "Walk";
            animator.SetBool(ANIMATION_NAME, false);
        }

        private void PlayWalkAnimation()
        {
            const string ANIMATION_NAME = "Walk";
            animator.SetBool(ANIMATION_NAME, true);
        }
    }
}