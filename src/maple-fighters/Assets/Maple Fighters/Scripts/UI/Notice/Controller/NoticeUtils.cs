using System;

namespace Scripts.UI.Notice
{
    public static class NoticeUtils
    {
        public static void ShowNotice(string message, Action onClicked = null, bool background = true)
        {
            var noticeController =
                UnityEngine.Object.FindObjectOfType<NoticeController>();
            if (noticeController != null)
            {
                noticeController.Show(message, onClicked, background);
            }
        }
    }
}