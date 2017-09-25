using Chat.Common;
using CommonCommunicationInterfaces;
using CommonTools.Log;
using Scripts.ScriptableObjects;

namespace Scripts.Services
{
    public sealed class ChatService : ServiceBase<ChatOperations, ChatEvents>, IChatService
    {
        public UnityEvent<ChatMessageEventParameters> ChatMessageReceived { get; } = new UnityEvent<ChatMessageEventParameters>();

        public void Connect()
        {
            var gameConnectionInformation = ServicesConfiguration.GetInstance().GetConnectionInformation(ServersType.Chat);
            Connect(gameConnectionInformation);
        }

        public void Disconnect()
        {
            Dispose();
        }

        protected override void OnConnected()
        {
            AddEventsHandlers();

            SendChatMessage(new ChatMessageRequestParameters("Hello world!"));
        }

        protected override void OnDisconnected()
        {
            RemoveEventsHandlers();
        }

        private void AddEventsHandlers()
        {
            EventHandlerRegister.SetHandler(ChatEvents.ChatMessage, new EventInvoker<ChatMessageEventParameters>(unityEvent =>
            {
                LogUtils.Log($"Message: {unityEvent.Parameters.Message}");
                ChatMessageReceived?.Invoke(unityEvent.Parameters);
                return true;
            }));
        }

        private void RemoveEventsHandlers()
        {
            EventHandlerRegister.RemoveHandler(ChatEvents.ChatMessage);
        }

        public void SendChatMessage(ChatMessageRequestParameters parameters)
        {
            if (!IsConnected())
            {
                return;
            }

            OperationRequestSender.Send(ChatOperations.ChatMessage, parameters, MessageSendOptions.DefaultReliable());
        }
    }
}