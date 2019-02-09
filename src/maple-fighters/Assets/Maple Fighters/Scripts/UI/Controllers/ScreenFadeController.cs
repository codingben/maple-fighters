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
                screenFadeImage.Show();
            }
        }

        public void Hide()
        {
            if (screenFadeImage == null)
            {
                screenFadeImage = UIElementsCreator.GetInstance()
                    .Create<ScreenFadeImage>(UILayer.Foreground, UIIndex.End);
                screenFadeImage.Hide();
            }
            else
            {
                screenFadeImage.Hide();
            }

            var uiFadeAnimation =
                screenFadeImage.GetComponent<UIFadeAnimation>();
            uiFadeAnimation.FadeOutCompleted += OnFadeOutCompleted;
        }

        private void OnFadeOutCompleted()
        {
            if (screenFadeImage != null)
            {
                var uiFadeAnimation =
                    screenFadeImage.GetComponent<UIFadeAnimation>();
                uiFadeAnimation.FadeOutCompleted -= OnFadeOutCompleted;

                Destroy(screenFadeImage.gameObject);
            }
        }
    }
}