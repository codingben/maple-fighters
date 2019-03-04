using UnityEngine;

namespace Scripts.UI.Controllers
{
    public static class NoticeUtils
    {
        public static void ShowNotice(string message)
        {
            // TODO: Use event bus system
            var noticeController = Object.FindObjectOfType<NoticeController>();
            if (noticeController != null)
            {
                noticeController.Show(message);
            }
        }
    }
}