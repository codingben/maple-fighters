using System;
using Character.Client.Common;
using CommonTools.Log;
using Scripts.UI.Core;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Scripts.UI
{
    public class ClickableCharacter : UserInterfaceBaseFadeEffect, IPointerClickHandler
    {
        public Action PlayIdleAnimationAction;
        public Action PlayWalkAnimationAction;

        public event Action<CharacterFromDatabaseParameters, int> CharacterClicked;

        private int index;
        private CharacterFromDatabaseParameters? character;

        private Animator animator;

        private void Start()
        {
            animator = GetComponent<Animator>();

            PlayIdleAnimationAction = PlayIdleAnimation;
            PlayWalkAnimationAction = PlayWalkAnimation;

            Show();

            SubscribeToMouseDetectionBackgroundEvent();
        }

        private void OnDestroy()
        {
            CharacterClicked = null;

            UnsubscribeFromMouseDetectionBackgroundEvent();
        }

        public override void Hide()
        {
            Hide(onFinished: () => Destroy(gameObject));
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

        public void SetCharacter(int index, CharacterFromDatabaseParameters character)
        {
            this.index = index;
            this.character = character;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!CanvasGroup.interactable)
            {
                return;
            }

            PlayWalkAnimation();

            CharacterClicked?.Invoke(character.GetValueOrDefault(), index);
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