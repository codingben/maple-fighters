using Chat.Common;

namespace Scripts.Services
{
    public interface IChatPeerLogicAPI : IPeerLogicBase
    {
        void SendChatMessage(ChatMessageRequestParameters parameters);

        UnityEvent<ChatMessageEventParameters> ChatMessageReceived { get; }
    }
}