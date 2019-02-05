using System.Threading.Tasks;
using Chat.Common;
using CommonCommunicationInterfaces;

namespace Scripts.Services
{
    public class ChatApi : ApiBase<ChatOperations, ChatEvents>, IChatApi
    {
        public UnityEvent<ChatMessageEventParameters> ChatMessageReceived { get; }

        public ChatApi()
        {
            ChatMessageReceived = new UnityEvent<ChatMessageEventParameters>();
        }

        private void SetEventHandlers()
        {
            ServerPeerHandler
                .SetEventHandler(ChatEvents.ChatMessage, ChatMessageReceived);
        }

        private void RemoveEventHandlers()
        {
            ServerPeerHandler.RemoveEventHandler(ChatEvents.ChatMessage);
        }

        public Task SendChatMessage(ChatMessageRequestParameters parameters)
        {
            return ServerPeerHandler.SendOperation(
                ChatOperations.ChatMessage,
                parameters,
                MessageSendOptions.DefaultReliable());
        }
    }
}