namespace Scripts.Services.Authenticator
{
    public interface IAuthenticatorService
    {
        IAuthenticatorApi AuthenticatorApi { get; }
    }
}