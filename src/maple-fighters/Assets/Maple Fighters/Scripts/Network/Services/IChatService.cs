using Scripts.Network.APIs;
using Scripts.Network.Core;

namespace Scripts.Network.Services
{
    public interface IChatService : IServiceBase
    {
        IAuthorizerApi GetAuthorizerApi();

        IChatApi GetChatApi();
    }
}