using System;
using UI.Manager;
using UnityEngine;

namespace Scripts.UI.CharacterSelection
{
    [RequireComponent(typeof(UIFadeAnimation))]
    public class LoadingText : UIElement, ILoadingView, ILoadingAnimation
    {
        public ILoadingAnimation LoadingAnimation => this;

        public event Action Finished;

        private UIFadeAnimation uiFadeAnimation;

        private void Awake()
        {
            uiFadeAnimation = GetComponent<UIFadeAnimation>();
            uiFadeAnimation.FadeInCompleted += OnFadeInCompleted;
            uiFadeAnimation.FadeOutCompleted += OnFadeOutCompleted;
        }

        private void OnDestroy()
        {
            uiFadeAnimation.FadeInCompleted -= OnFadeInCompleted;
            uiFadeAnimation.FadeOutCompleted -= OnFadeOutCompleted;
        }

        private void OnFadeInCompleted()
        {
            Finished?.Invoke();
        }

        private void OnFadeOutCompleted()
        {
            Finished?.Invoke();
        }
    }
}