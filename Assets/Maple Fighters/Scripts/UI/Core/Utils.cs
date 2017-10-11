using System;
using System.Text.RegularExpressions;
using Scripts.UI.Core;
using Scripts.UI.Windows;

namespace Scripts.UI
{
    public static class Utils
    {
        public static bool IsValidEmailAddress(this string emailAddress)
        {
            try
            {
                var regex = new Regex(
                    @"^[-!#$%&'*+/0-9=?A-Z^_a-z{|}~](\.?[-!#$%&'*+/0-9=?A-Z^_a-z{|}~])*@[a-zA-Z](-?[a-zA-Z0-9])*(\.[a-zA-Z](-?[a-zA-Z0-9])*)+$");
                return regex.IsMatch(emailAddress);
            }
            catch (FormatException)
            {
                return false;
            }
        }

        public static NoticeWindow ShowNotice(string message, Action okButtonClicked, bool background = false)
        {
            var noticeWindow = UserInterfaceContainer.Instance.Add<NoticeWindow>();
            noticeWindow.Initialize(message, okButtonClicked, background);
            noticeWindow.Show();
            return noticeWindow;
        }
    }
}