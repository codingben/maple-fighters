namespace Scripts.UI.Windows
{
    public struct UIRegistrationDetails
    {
        public string Email { get; }

        public string Password { get; }

        public string ConfirmPassword { get; }

        public string Firstname { get; }

        public string Lastname { get; }

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
            Firstname = firstname;
            Lastname = lastname;
        }
    }
}
