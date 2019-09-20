namespace Scripts.UI.Authenticator
{
    public struct UIAuthenticationDetails
    {
        public string Email { get; }

        public string Password { get; }

        public UIAuthenticationDetails(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }
}