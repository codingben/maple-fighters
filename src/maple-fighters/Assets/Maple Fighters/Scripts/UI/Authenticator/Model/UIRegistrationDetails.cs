namespace Scripts.UI.Authenticator
{
    public struct UIRegistrationDetails
    {
        public string Email { get; }

        public string Password { get; }

        public string ConfirmPassword { get; }

        public string FirstName { get; }

        public string LastName { get; }

        public UIRegistrationDetails(
            string email,
            string password,
            string confirmPassword,
            string firstname,
            string lastname)
        {
            Email = email;
            Password = password;
            ConfirmPassword = confirmPassword;
            FirstName = firstname;
            LastName = lastname;
        }
    }
}
