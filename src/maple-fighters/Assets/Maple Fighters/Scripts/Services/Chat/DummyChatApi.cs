using Chat.Common;
using CommonCommunicationInterfaces;
using Network.Scripts;

namespace Scripts.Services.Chat
{
    internal class DummyChatApi : NetworkApi<ChatOperations, ChatEvents>, IChatApi
    {
        public UnityEvent<ChatMessageEventParameters> ChatMessageReceived { get; }

        internal DummyChatApi(IServerPeer serverPeer, bool log = false)
            : base(serverPeer, log)
        {
            ChatMessageReceived = new UnityEvent<ChatMessageEventParameters>();

            EventHandlerRegister.SetHandler(ChatEvents.ChatMessage, ChatMessageReceived.ToEventHandler());
        }

        public new void Dispose()
        {
            base.Dispose();

            EventHandlerRegister.RemoveHandler(ChatEvents.ChatMessage);
        }

        public void SendChatMessage(ChatMessageRequestParameters parameters)
        {
            OperationRequestSender.Send(
                ChatOperations.ChatMessage,
                parameters,
                MessageSendOptions.DefaultReliable());
        }
    }
}