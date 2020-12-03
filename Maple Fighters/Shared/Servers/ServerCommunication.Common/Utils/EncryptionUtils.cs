using System;
using System.Text;
using System.Security.Cryptography;

namespace ServerCommunication.Common
{
    public static class EncryptionUtils
    {
        public static string CreateSha512(this string content)
        {
            if (content == null)
            {
                return null;
            }

            var hashTool = new SHA512Managed();
            var phraseAsByte = Encoding.UTF8.GetBytes(content);
            var encryptedBytes = hashTool.ComputeHash(phraseAsByte);
            hashTool.Clear();
            return Convert.ToBase64String(encryptedBytes);
        }
    }
}