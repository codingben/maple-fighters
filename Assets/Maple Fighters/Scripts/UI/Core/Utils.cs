using System;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using Scripts.UI.Core;
using Scripts.UI.Windows;

namespace Scripts.UI
{
    public static class Utils
    {
        public static string CreateSha512(this string content)
        {
            if (content == null)
            {
                return null;
            }

            var hashTool = new SHA512Managed();
            var phraseAsByte = System.Text.Encoding.UTF8.GetBytes(string.Concat(content));
            var encryptedBytes = hashTool.ComputeHash(phraseAsByte);
            hashTool.Clear();
            return Convert.ToBase64String(encryptedBytes);
        }

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

        public static NoticeWindow ShowNotice(string message, Action okButtonClicked, bool background = false, Index index = Index.First)
        {
            var noticeWindow = UserInterfaceContainer.Instance.Add<NoticeWindow>(ViewType.Foreground, index);
            noticeWindow.Initialize(message, okButtonClicked, background);
            noticeWindow.Show();
            return noticeWindow;
        }

        /// <summary>
        /// Removing a "(Clone)" from a name of the created game object.
        /// </summary>
        public static string RemoveCloneFromName(this string value)
        {
            var result = value.Replace("(Clone)", string.Empty);
            return result;
        }
    }
}