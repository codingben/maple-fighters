using System.Threading.Tasks;
using Chat.Common;
using CommonCommunicationInterfaces;

namespace Scripts.Services
{
    public class ChatApi : IChatApi
    {
        public ServerPeerHandler<ChatOperations, ChatEvents> ServerPeer
        {
            get;
        }

        public UnityEvent<ChatMessageEventParameters> ChatMessageReceived { get; }

        public ChatApi()
        {
            ServerPeer =
                new ServerPeerHandler<ChatOperations, ChatEvents>();

            ChatMessageReceived = new UnityEvent<ChatMessageEventParameters>();
        }

        private void SetEventHandlers()
        {
            ServerPeer
                .SetEventHandler(ChatEvents.ChatMessage, ChatMessageReceived);
        }

        private void RemoveEventHandlers()
        {
            ServerPeer.RemoveEventHandler(ChatEvents.ChatMessage);
        }

        public Task SendChatMessage(ChatMessageRequestParameters parameters)
        {
            ServerPeer
                .SendOperation(
                    ChatOperations.ChatMessage,
                    parameters,
                    MessageSendOptions.DefaultReliable());

            return Task.CompletedTask;
        }
    }
}