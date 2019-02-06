using Scripts.Network.APIs;
using Scripts.Network.Core;

namespace Scripts.Network
{
    public interface IChatService : IServiceBase
    {
        IAuthorizerApi GetAuthorizerApi();

        IChatApi GetChatApi();
    }
}