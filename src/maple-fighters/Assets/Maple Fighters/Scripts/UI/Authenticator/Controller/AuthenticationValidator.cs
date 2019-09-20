namespace Scripts.UI.Authenticator
{
    public class AuthenticationValidator
    {
        private const int PasswordLength = 6;
        private const int FirstNameLength = 3;
        private const int LastNameLength = 3;

        public bool IsEmptyEmailAddress(string email, out string message)
        {
            message = WindowMessages.EmptyEmailAddress;

            return string.IsNullOrWhiteSpace(email);
        }

        public bool IsInvalidEmailAddress(string email, out string message)
        {
            message = WindowMessages.InvalidEmailAddress;

            return WindowUtils.IsEmailAddressValid(email) == false;
        }

        public bool IsEmptyPassword(string password, out string message)
        {
            message = WindowMessages.EmptyPassword;

            return string.IsNullOrWhiteSpace(password);
        }

        public bool IsPasswordTooShort(string password, out string message)
        {
            message = WindowMessages.ShortPassword;

            return password.Length <= PasswordLength;
        }

        public bool IsEmptyConfirmPassword(string password, out string message)
        {
            message = WindowMessages.EmptyConfirmPassword;

            return string.IsNullOrWhiteSpace(password);
        }

        public bool IsConfirmPasswordTooShort(string password, out string message)
        {
            message = WindowMessages.ShortPassword;

            return password.Length <= PasswordLength;
        }

        public bool ArePasswordsDoNotMatch(string password, string confirmPassword, out string message)
        {
            message = WindowMessages.PasswordsDoNotMatch;

            return password != confirmPassword;
        }

        public bool IsFirstNameEmpty(string firstName, out string message)
        {
            message = WindowMessages.EmptyFirstName;

            return string.IsNullOrWhiteSpace(firstName);
        }

        public bool IsLastNameEmpty(string lastName, out string message)
        {
            message = WindowMessages.EmptyLastName;

            return string.IsNullOrWhiteSpace(lastName);
        }

        public bool IsFirstNameTooShort(string firstName, out string message)
        {
            message = WindowMessages.ShortFirstName;

            return firstName.Length < FirstNameLength;
        }

        public bool IsLastNameTooShort(string lastName, out string message)
        {
            message = WindowMessages.ShortLastName;

            return lastName.Length < LastNameLength;
        }
    }
}