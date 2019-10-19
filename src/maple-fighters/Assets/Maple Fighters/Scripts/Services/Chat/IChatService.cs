using Scripts.Services.Authorizer;

namespace Scripts.Services.Chat
{
    public interface IChatService
    {
        IAuthorizerApi AuthorizerApi { get; }

        IChatApi ChatApi { get; }
    }
}