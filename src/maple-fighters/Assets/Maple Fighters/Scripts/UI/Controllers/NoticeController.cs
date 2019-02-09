using System;
using Scripts.UI.Windows;
using UI.Manager;
using UnityEngine;

namespace Scripts.UI.Controllers
{
    public class NoticeController : MonoBehaviour
    {
        private NoticeWindow noticeWindow;
        private Action onOkButtonClicked;

        private void OnDestroy()
        {
            if (noticeWindow != null)
            {
                Destroy(noticeWindow.gameObject);
            }
        }

        public void Show(string message, Action onClicked = null, bool background = true)
        {
            if (noticeWindow == null)
            {
                noticeWindow = UIElementsCreator.GetInstance()
                    .Create<NoticeWindow>(UILayer.Foreground, UIIndex.End);

                noticeWindow.Message = message;
                noticeWindow.OkButtonClicked += Hide;

                if (onClicked != null)
                {
                    onOkButtonClicked = onClicked;
                    noticeWindow.OkButtonClicked += onOkButtonClicked;
                }
                else
                {
                    onOkButtonClicked = null;
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

                if (onOkButtonClicked != null)
                {
                    noticeWindow.OkButtonClicked -= onOkButtonClicked;
                }

                noticeWindow.Hide();
            }
        }

        private void OnFadeOutCompleted()
        {
            if (noticeWindow != null)
            {
                var uiFadeAnimation =
                    noticeWindow.GetComponent<UIFadeAnimation>();
                uiFadeAnimation.FadeOutCompleted -= OnFadeOutCompleted;

                Destroy(noticeWindow.gameObject);
            }
        }
    }
}