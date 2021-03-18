using System;
using UI;
using UnityEngine;

namespace Scripts.UI.ScreenFade
{
    public class ScreenFadeController : MonoBehaviour
    {
        public Action FadeInCompleted;
        public Action FadeOutCompleted;

        [SerializeField]
        private bool hideOnStart;

        private IScreenFadeView screenFadeView;

        private void Start()
        {
            if (hideOnStart)
            {
                Hide();
            }
        }

        private void OnDestroy()
        {
            if (screenFadeView != null)
            {
                screenFadeView.FadeInCompleted -= OnFadeInCompleted;
                screenFadeView.FadeOutCompleted -= OnFadeOutCompleted;
            }
        }

        private void CreateAndSubscribeToScreenFadeImage()
        {
            screenFadeView = UIElementsCreator
                .GetInstance()
                .Create<ScreenFadeImage>(UILayer.Foreground, UIIndex.End);
            screenFadeView.FadeInCompleted += OnFadeInCompleted;
            screenFadeView.FadeOutCompleted += OnFadeOutCompleted;
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
            if (screenFadeView == null)
            {
                CreateAndSubscribeToScreenFadeImage();
            }

            screenFadeView.Show();
        }

        public void Hide()
        {
            if (screenFadeView == null)
            {
                CreateAndSubscribeToScreenFadeImage();
            }

            screenFadeView.Hide();
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