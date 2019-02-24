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

        public void Show(string message, Action onClicked = null, bool background = true)
        {
            var noticeWindow = CreateAndShowNoticeWindow();
            if (noticeWindow != null)
            {
                noticeWindow.Message = message;

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
            }
        }

        private NoticeWindow CreateAndShowNoticeWindow()
        {
            if (noticeWindow == null)
            {
                noticeWindow = UIElementsCreator.GetInstance()
                    .Create<NoticeWindow>(UILayer.Foreground, UIIndex.End);
                noticeWindow.OkButtonClicked += Hide;
                noticeWindow.Show();
            }

            return null;
        }

        public void Hide()
        {
            SubscribeToUIFadeAnimation();
            HideNoticeWindow();
        }

        private void HideNoticeWindow()
        {
            if (noticeWindow != null)
            {
                noticeWindow.OkButtonClicked -= Hide;

                if (onOkButtonClicked != null)
                {
                    noticeWindow.OkButtonClicked -= onOkButtonClicked;
                }

                noticeWindow.Hide();
            }
        }

        private void SubscribeToUIFadeAnimation()
        {
            if (noticeWindow != null)
            {
                var uiFadeAnimation =
                    noticeWindow.GetComponent<UIFadeAnimation>();
                uiFadeAnimation.FadeOutCompleted += OnFadeOutCompleted;
            }
        }

        private void UnsubscribeFromUIFadeAnimation()
        {
            if (noticeWindow != null)
            {
                var uiFadeAnimation =
                    noticeWindow.GetComponent<UIFadeAnimation>();
                uiFadeAnimation.FadeOutCompleted -= OnFadeOutCompleted;
            }
        }

        private void OnDestroy()
        {
            DestroyNoticeWindow();
        }

        private void DestroyNoticeWindow()
        {
            if (noticeWindow != null)
            {
                Destroy(noticeWindow.gameObject);
            }
        }

        private void OnFadeOutCompleted()
        {
            UnsubscribeFromUIFadeAnimation();
            DestroyNoticeWindow();
        }
    }
}