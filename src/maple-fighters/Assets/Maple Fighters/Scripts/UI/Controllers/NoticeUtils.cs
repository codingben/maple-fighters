using System;

namespace Scripts.UI.Controllers
{
    using Object = UnityEngine.Object;

    public static class NoticeUtils
    {
        public static void ShowNotice(string message, Action onClicked = null, bool background = true)
        {
            // TODO: Use event bus system
            var noticeController = Object.FindObjectOfType<NoticeController>();
            if (noticeController != null)
            {
                noticeController.Show(message, onClicked, background);
            }
        }
    }
}