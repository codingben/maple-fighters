namespace Scripts.Network
{
    public interface IAuthenticatorService : IServiceBase
    {
        IAuthenticatorApi GetAuthenticatorApi();
    }
}