using System;
using System.Collections;
using UI;
using UnityEngine;

namespace Scripts.UI.ScreenFade
{
    public class ScreenFadeController : MonoBehaviour
    {
        public Action FadeInCompleted;
        public Action FadeOutCompleted;

        [SerializeField]
        private ScreenFadeImage screenFadeView;

        private void Start()
        {
            SubscribeToScreenFadeImage();
        }

        private void OnDestroy()
        {
            if (screenFadeView != null)
            {
                screenFadeView.FadeInCompleted -= OnFadeInCompleted;
                screenFadeView.FadeOutCompleted -= OnFadeOutCompleted;
            }
        }

        private void SubscribeToScreenFadeImage()
        {
            if (screenFadeView != null)
            {
                screenFadeView.FadeInCompleted += OnFadeInCompleted;
                screenFadeView.FadeOutCompleted += OnFadeOutCompleted;
            }
        }

        private void UnsubscribeFromScreenFadeImage()
        {
            if (screenFadeView != null)
            {
                screenFadeView.FadeInCompleted -= OnFadeInCompleted;
                screenFadeView.FadeOutCompleted -= OnFadeOutCompleted;
            }
        }

        public void Show()
        {
            screenFadeView?.Show();
        }

        public void Hide()
        {
            screenFadeView?.Hide();
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