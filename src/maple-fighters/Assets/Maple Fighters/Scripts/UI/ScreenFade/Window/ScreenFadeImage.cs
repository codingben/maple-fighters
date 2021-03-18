using System;
using UI;
using UnityEngine;

namespace Scripts.UI.ScreenFade
{
    [RequireComponent(typeof(UIFadeAnimation))]
    public class ScreenFadeImage : UIElement, IScreenFadeView
    {
        public event Action FadeInCompleted;
        public event Action FadeOutCompleted;

        private void Start()
        {
            SubscribeToUIFadeAnimation();
        }

        private void OnDestroy()
        {
            UnsubscribeFromUIFadeAnimation();
        }

        private void SubscribeToUIFadeAnimation()
        {
            var uiFadeAnimation = GetComponent<UIFadeAnimation>();
            uiFadeAnimation.FadeInCompleted += OnFadeInCompleted;
            uiFadeAnimation.FadeOutCompleted += OnFadeOutCompleted;
        }

        private void UnsubscribeFromUIFadeAnimation()
        {
            var uiFadeAnimation = GetComponent<UIFadeAnimation>();
            uiFadeAnimation.FadeInCompleted -= OnFadeInCompleted;
            uiFadeAnimation.FadeOutCompleted -= OnFadeOutCompleted;
        }

        private void OnFadeInCompleted()
        {
            FadeInCompleted?.Invoke();
        }

        private void OnFadeOutCompleted()
        {
            FadeOutCompleted?.Invoke();
        }
    }
}