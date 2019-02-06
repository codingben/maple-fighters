using Scripts.Network.APIs;
using Scripts.Network.Core;

namespace Scripts.Network
{
    public interface IAuthenticatorService : IServiceBase
    {
        IAuthenticatorApi GetAuthenticatorApi();
    }
}