namespace Scripts.Constants
{
    public static class NoticeMessages
    {
        public static class AuthView
        {
            public const string EmptyEmailAddress = "Email address can not be empty.";
            public const string InvalidEmailAddress = "Email address is not valid.";
            public const string EmptyPassword = "Password can not be empty.";
            public const string ShortPassword = "Please enter a longer password.";
            public const string EmptyConfirmPassword = "Confirm password can not be empty.";
            public const string PasswordsDoNotMatch = "Passwords are not match.";
            public const string EmptyFirstName = "The first name is empty.";
            public const string EmptyLastName = "The last name is empty.";
            public const string ShortFirstName = "The first name is too short.";
            public const string ShortLastName = "The last name is too short.";
            public const string WrongPassword = "The password is incorrect.";
            public const string WrongEmailAddress = "The email address does not exist.";
            public const string RegistrationSucceed = "Registration completed successfully!";
            public const string UnknownError = "An unknown error has occurred. Please try again.";
        }

        public static class GameServer
        {
            public const string ConnectionClosed = "Connection to the game server has been lost. Please try again.";
        }

        public static class GameServerBrowserView
        {
            public const string UnknownError = "An unknown error has occurred. Please try again.";
        }

        public static class CharacterView
        {
            public const string CreationFailed = "Character creation failed. Please try again.";
            public const string DeletionFailed = "Character deletion failed. Please try again.";
            public const string NotFound = "Character not found. Please create new character.";
            public const string NameAlreadyInUse = "Character name is already taken.";
            public const string UnknownError = "An unknown error has occurred. Please try again.";
        }
    }
}