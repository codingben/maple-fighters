namespace Authenticator.API.Constants
{
    public static class ErrorMessages
    {
        public const string InvalidEmail = "Invalid email address.";
        public const string EmailAlreadyExists = "This email address is already being used.";
        public const string PasswordRequired = "Password is required.";
        public const string FirstNameRequired = "First name is required.";
        public const string LastNameRequired = "Last name is required.";
        public const string AccountNotFound = "Account does not exist.";
        public const string WrongPassword = "Password is incorrect.";
        public const string EmailLength = "Email must be between 3 and 50 characters.";
        public const string PasswordLength = "Password must be between 8 and 16 characters.";
        public const string FirstNameLength = "First name must be between 2 and 26 characters.";
        public const string LastNameLength = "Last name must be between 2 and 26 characters.";
    }
}