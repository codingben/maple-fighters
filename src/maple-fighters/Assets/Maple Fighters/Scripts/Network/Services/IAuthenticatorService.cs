using Scripts.Network.APIs;

namespace Scripts.Network
{
    public interface IAuthenticatorService : IServiceBase
    {
        IAuthenticatorApi GetAuthenticatorApi();
    }
}