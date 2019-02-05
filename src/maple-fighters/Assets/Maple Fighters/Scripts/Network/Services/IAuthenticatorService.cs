namespace Scripts.Services
{
    public interface IAuthenticatorService
    {
        IAuthenticatorApi AuthenticatorApi { get; }
    }
}