using System.Threading.Tasks;
using Chat.Common;
using CommonCommunicationInterfaces;
using Scripts.Network.Core;

namespace Scripts.Network.APIs
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
            EventHandlerRegister
                .SetHandler(ChatEvents.ChatMessage, ChatMessageReceived.ToHandler());
        }

        private void RemoveEventHandlers()
        {
            EventHandlerRegister.RemoveHandler(ChatEvents.ChatMessage);
        }

        public Task SendChatMessage(ChatMessageRequestParameters parameters)
        {
            OperationRequestSender.Send(
                ChatOperations.ChatMessage,
                parameters,
                MessageSendOptions.DefaultReliable());

            return Task.CompletedTask;
        }
    }
}