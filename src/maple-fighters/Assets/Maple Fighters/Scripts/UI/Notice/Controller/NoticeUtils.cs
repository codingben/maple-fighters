using System;

namespace Scripts.UI.Controllers
{
    using Object = UnityEngine.Object;

    public static class NoticeUtils
    {
        public static void ShowNotice(string message, Action onClicked = null, bool background = true)
        {
            var noticeController = Object.FindObjectOfType<NoticeController>();
            if (noticeController != null)
            {
                noticeController.Show(message, onClicked, background);
            }
        }
    }
}