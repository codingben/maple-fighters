namespace Scripts.Services.AuthenticatorApi
{
    public interface IAuthenticatorApi
    {
        void Authenticate(string email, string password);

        void Register();
    }
}