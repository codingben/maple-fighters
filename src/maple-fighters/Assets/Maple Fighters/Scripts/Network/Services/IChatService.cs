using Scripts.Network.APIs;

namespace Scripts.Network.Services
{
    public interface IChatService
    {
        IAuthorizerApi GetAuthorizerApi();

        IChatApi GetChatApi();
    }
}