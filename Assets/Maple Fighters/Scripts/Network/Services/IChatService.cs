using Chat.Common;

namespace Scripts.Services
{
    public interface IChatService : IServiceBase
    {
        UnityEvent<ChatMessageEventParameters> ChatMessageReceived { get; }
    }
}