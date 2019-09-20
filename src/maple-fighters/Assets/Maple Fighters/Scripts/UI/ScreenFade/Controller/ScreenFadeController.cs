using UI.Manager;
using UnityEngine;

namespace Scripts.UI.ScreenFade
{
    public class ScreenFadeController : MonoBehaviour
    {
        private IScreenFadeView screenFadeView;

        private void Awake()
        {
            CreateScreenFadeImage();
        }

        private void Start()
        {
            Hide();
        }

        private void OnDestroy()
        {
            UnsubscribeFromUIFadeAnimation();
        }

        private void CreateScreenFadeImage()
        {
            screenFadeView = UIElementsCreator.GetInstance()
                .Create<ScreenFadeImage>(UILayer.Foreground, UIIndex.End);
        }

        private void Hide()
        {
            SubscribeToUIFadeAnimation();

            screenFadeView?.Hide();
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