namespace Scripts.Services
{
    public interface IAuthenticatorService : IServiceBase
    {
        IAuthenticatorApi GetAuthenticatorApi();
    }
}