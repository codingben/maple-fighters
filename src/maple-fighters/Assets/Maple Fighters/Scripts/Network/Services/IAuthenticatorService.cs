using Scripts.Network.APIs;
using Scripts.Network.Core;

namespace Scripts.Network.Services
{
    public interface IAuthenticatorService : IServiceBase
    {
        IAuthenticatorApi GetAuthenticatorApi();
    }
}