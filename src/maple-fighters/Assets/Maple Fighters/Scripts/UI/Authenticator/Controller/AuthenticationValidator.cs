using System;
using System.Text.RegularExpressions;
using Scripts.Constants;
using Scripts.UI.Utils;

namespace Scripts.UI.Authenticator
{
    public class AuthenticationValidator
    {
        private const int PasswordLength = 6;
        private const int FirstNameLength = 3;
        private const int LastNameLength = 3;

        public bool IsEmptyEmailAddress(string email, out string message)
        {
            message = NoticeMessages.AuthView.EmptyEmailAddress;

            return string.IsNullOrWhiteSpace(email);
        }

        public bool IsInvalidEmailAddress(string email, out string message)
        {
            message = NoticeMessages.AuthView.InvalidEmailAddress;

            return IsEmailAddressValid(email) == false;
        }

        public bool IsEmptyPassword(string password, out string message)
        {
            message = NoticeMessages.AuthView.EmptyPassword;

            return string.IsNullOrWhiteSpace(password);
        }

        public bool IsPasswordTooShort(string password, out string message)
        {
            message = NoticeMessages.AuthView.ShortPassword;

            return password.Length <= PasswordLength;
        }

        public bool IsEmptyConfirmPassword(string password, out string message)
        {
            message = NoticeMessages.AuthView.EmptyConfirmPassword;

            return string.IsNullOrWhiteSpace(password);
        }

        public bool IsConfirmPasswordTooShort(string password, out string message)
        {
            message = NoticeMessages.AuthView.ShortPassword;

            return password.Length <= PasswordLength;
        }

        public bool ArePasswordsDoNotMatch(string password, string confirmPassword, out string message)
        {
            message = NoticeMessages.AuthView.PasswordsDoNotMatch;

            return password != confirmPassword;
        }

        public bool IsFirstNameEmpty(string firstName, out string message)
        {
            message = NoticeMessages.AuthView.EmptyFirstName;

            return string.IsNullOrWhiteSpace(firstName);
        }

        public bool IsLastNameEmpty(string lastName, out string message)
        {
            message = NoticeMessages.AuthView.EmptyLastName;

            return string.IsNullOrWhiteSpace(lastName);
        }

        public bool IsFirstNameTooShort(string firstName, out string message)
        {
            message = NoticeMessages.AuthView.ShortFirstName;

            return firstName.Length < FirstNameLength;
        }

        public bool IsLastNameTooShort(string lastName, out string message)
        {
            message = NoticeMessages.AuthView.ShortLastName;

            return lastName.Length < LastNameLength;
        }

        public bool IsEmailAddressValid(string emailAddress)
        {
            try
            {
                var regex = new Regex(@"^[-!#$%&'*+/0-9=?A-Z^_a-z{|}~](\.?[-!#$%&'*+/0-9=?A-Z^_a-z{|}~])*@[a-zA-Z](-?[a-zA-Z0-9])*(\.[a-zA-Z](-?[a-zA-Z0-9])*)+$");
                return regex.IsMatch(emailAddress);
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}