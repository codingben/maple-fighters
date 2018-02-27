using Chat.Common;

namespace Scripts.Services
{
    public sealed class ChatService : ServiceBase<ChatOperations, ChatEvents>, IChatService
    {
        public UnityEvent<ChatMessageEventParameters> ChatMessageReceived { get; } = new UnityEvent<ChatMessageEventParameters>();

        protected override void OnConnected()
        {
            SetEventHandler(ChatEvents.ChatMessage, ChatMessageReceived);
        }

        protected override void OnDisconnected()
        {
            RemoveEventHandler(ChatEvents.ChatMessage);
        }
    }
}