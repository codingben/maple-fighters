namespace Authenticator.API.Datas
{
    public struct LoginData
    {
        public string Email { get; }

        public string Password { get; }

        public LoginData(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }
}