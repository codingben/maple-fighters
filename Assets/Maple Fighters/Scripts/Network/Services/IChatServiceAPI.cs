using Chat.Common;

namespace Scripts.Services
{
    public interface IChatServiceAPI : IPeerLogicBase
    {
        void SendChatMessage(ChatMessageRequestParameters parameters);

        UnityEvent<ChatMessageEventParameters> ChatMessageReceived { get; }
    }
}