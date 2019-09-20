using System;
using UI.Manager;
using UnityEngine;

namespace Scripts.UI.ScreenFade
{
    [RequireComponent(typeof(UIFadeAnimation))]
    public class ScreenFadeImage : UIElement, IScreenFadeView
    {
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
            uiFadeAnimation.FadeOutCompleted += OnFadeOutCompleted;
        }

        private void UnsubscribeFromUIFadeAnimation()
        {
            var uiFadeAnimation = GetComponent<UIFadeAnimation>();
            uiFadeAnimation.FadeOutCompleted -= OnFadeOutCompleted;
        }

        private void OnFadeOutCompleted()
        {
            FadeOutCompleted?.Invoke();
        }
    }
}