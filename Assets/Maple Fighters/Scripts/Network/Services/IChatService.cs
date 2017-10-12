using Chat.Common;

namespace Scripts.Services
{
    public interface IChatService
    {
        void Connect();
        void Disconnect();

        void SendChatMessage(ChatMessageRequestParameters parameters);

        UnityEvent<ChatMessageEventParameters> ChatMessageReceived { get; }
    }
}