using Chat.Common;
using Network.Scripts;

namespace Scripts.Services.Chat
{
    public interface IChatApi
    {
        UnityEvent<ChatMessageEventParameters> ChatMessageReceived { get; }

        void SendChatMessage(ChatMessageRequestParameters parameters);
    }
}