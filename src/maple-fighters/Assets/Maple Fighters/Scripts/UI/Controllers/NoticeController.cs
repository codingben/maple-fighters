using System;
using Scripts.UI.Windows;
using Scripts.Utils;
using UI.Manager;

namespace Scripts.UI.Controllers
{
    public class NoticeController : MonoSingleton<NoticeController>
    {
        private NoticeWindow noticeWindow;

        protected override void OnDestroying()
        {
            base.OnDestroying();

            if (noticeWindow != null)
            {
                Destroy(noticeWindow.gameObject);
            }
        }

        public void Show(string message, Action onOkClicked = null, bool background = true)
        {
            if (noticeWindow == null)
            {
                noticeWindow = UIElementsCreator.GetInstance()
                    .Create<NoticeWindow>(UILayer.Foreground, UIIndex.End);

                noticeWindow.Message = message;
                noticeWindow.OkButtonClicked += Hide;

                if (onOkClicked != null)
                {
                    noticeWindow.OkButtonClicked += onOkClicked;
                }

                if (background == false)
                {
                    noticeWindow.HideBackground();
                }

                noticeWindow.Show();
            }
        }

        public void Hide()
        {
            if (noticeWindow != null)
            {
                var uiFadeAnimation =
                    noticeWindow.GetComponent<UIFadeAnimation>();
                uiFadeAnimation.FadeOutCompleted += OnFadeOutCompleted;

                noticeWindow.OkButtonClicked -= Hide;
                noticeWindow.Hide();
            }
        }

        private void OnFadeOutCompleted()
        {
            var uiFadeAnimation =
                noticeWindow.GetComponent<UIFadeAnimation>();
            uiFadeAnimation.FadeOutCompleted -= OnFadeOutCompleted;

            Destroy(noticeWindow.gameObject);
        }
    }
}