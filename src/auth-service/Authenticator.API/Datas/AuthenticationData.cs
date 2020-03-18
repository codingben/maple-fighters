namespace Authenticator.API.Datas
{
    public struct AuthenticationData
    {
        public string Email { get; }

        public string Password { get; }

        public AuthenticationData(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }
}