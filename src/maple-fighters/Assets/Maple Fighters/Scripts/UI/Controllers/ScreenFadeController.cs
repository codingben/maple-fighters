using Scripts.Utils;
using UI.Manager;

namespace Scripts.UI.Controllers
{
    public class ScreenFadeController : MonoSingleton<ScreenFadeController>
    {
        private ScreenFadeImage screenFadeImage;

        protected override void OnAwake()
        {
            base.OnAwake();

            Hide();
        }

        protected override void OnDestroying()
        {
            base.OnDestroying();

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
            var uiFadeAnimation =
                screenFadeImage.GetComponent<UIFadeAnimation>();
            uiFadeAnimation.FadeOutCompleted -= OnFadeOutCompleted;

            Destroy(screenFadeImage.gameObject);
        }
    }
}