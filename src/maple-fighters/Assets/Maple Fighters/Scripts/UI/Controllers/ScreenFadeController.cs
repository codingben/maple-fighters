using UI.Manager;
using UnityEngine;

namespace Scripts.UI.Controllers
{
    public class ScreenFadeController : MonoBehaviour
    {
        private ScreenFadeImage screenFadeImage;

        private void Awake()
        {
            Hide();
        }

        private void OnDestroy()
        {
            DestroyScreenFadeImage();
        }

        private void DestroyScreenFadeImage()
        {
            if (screenFadeImage != null)
            {
                Destroy(screenFadeImage.gameObject);
            }
        }

        public void Show()
        {
            if (screenFadeImage == null)
            {
                screenFadeImage = UIElementsCreator.GetInstance()
                    .Create<ScreenFadeImage>(UILayer.Foreground, UIIndex.End);
            }

            if (screenFadeImage != null)
            {
                screenFadeImage.Show();
            }
        }

        public void Hide()
        {
            if (screenFadeImage == null)
            {
                screenFadeImage = UIElementsCreator.GetInstance()
                    .Create<ScreenFadeImage>(UILayer.Foreground, UIIndex.End);
            }

            SubscribeToUIFadeAnimation();

            if (screenFadeImage != null)
            {
                screenFadeImage.Hide();
            }
        }

        private void SubscribeToUIFadeAnimation()
        {
            if (screenFadeImage != null)
            {
                var uiFadeAnimation =
                    screenFadeImage.GetComponent<UIFadeAnimation>();
                uiFadeAnimation.FadeOutCompleted += OnFadeOutCompleted;
            }
        }

        private void UnsubscribeFromUIFadeAnimation()
        {
            if (screenFadeImage != null)
            {
                var uiFadeAnimation =
                    screenFadeImage.GetComponent<UIFadeAnimation>();
                uiFadeAnimation.FadeOutCompleted -= OnFadeOutCompleted;
            }
        }

        private void OnFadeOutCompleted()
        {
            UnsubscribeFromUIFadeAnimation();
            DestroyScreenFadeImage();
        }
    }
}