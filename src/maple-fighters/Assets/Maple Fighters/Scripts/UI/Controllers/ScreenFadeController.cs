using UI.Manager;
using UnityEngine;

namespace Scripts.UI.Controllers
{
    public class ScreenFadeController : MonoBehaviour
    {
        private IScreenFadeView screenFadeView;

        private void Awake()
        {
            Hide();
        }

        public void Show()
        {
            if (screenFadeView == null)
            {
                screenFadeView = UIElementsCreator.GetInstance()
                    .Create<ScreenFadeImage>(UILayer.Foreground, UIIndex.End);
            }

            if (screenFadeView != null)
            {
                screenFadeView.Show();
            }
        }

        public void Hide()
        {
            if (screenFadeView == null)
            {
                screenFadeView = UIElementsCreator.GetInstance()
                    .Create<ScreenFadeImage>(UILayer.Foreground, UIIndex.End);
            }

            SubscribeToUIFadeAnimation();

            if (screenFadeView != null)
            {
                screenFadeView.Hide();
            }
        }

        private void SubscribeToUIFadeAnimation()
        {
            if (screenFadeView != null)
            {
                screenFadeView.FadeOutCompleted += OnFadeOutCompleted;
            }
        }

        private void UnsubscribeFromUIFadeAnimation()
        {
            if (screenFadeView != null)
            {
                screenFadeView.FadeOutCompleted -= OnFadeOutCompleted;
            }
        }

        private void OnFadeOutCompleted()
        {
            UnsubscribeFromUIFadeAnimation();
        }
    }
}