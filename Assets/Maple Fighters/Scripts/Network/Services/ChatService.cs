using System;
using System.Threading.Tasks;
using Chat.Common;
using CommonCommunicationInterfaces;
using CommonTools.Coroutines;
using Scripts.ScriptableObjects;

namespace Scripts.Services
{
    public sealed class ChatService : ServiceBase<ChatOperations, ChatEvents>, IChatService
    {
        public event Action Authenticated;

        public UnityEvent<ChatMessageEventParameters> ChatMessageReceived { get; } = new UnityEvent<ChatMessageEventParameters>();

        public void Connect()
        {
            var connectionInformation = ServicesConfiguration.GetInstance().GetConnectionInformation(ServersType.Chat);
            Connect(connectionInformation);
        }

        public void Disconnect()
        {
            Dispose();
        }

        protected override void OnConnected()
        {
            AddEventsHandlers();

            CoroutinesExecutor.StartTask(Authenticate);
        }

        protected override void OnDisconnected()
        {
            RemoveEventsHandlers();
        }

        private void AddEventsHandlers()
        {
            EventHandlerRegister.SetHandler(ChatEvents.ChatMessage, new EventInvoker<ChatMessageEventParameters>(unityEvent =>
            {
                ChatMessageReceived?.Invoke(unityEvent.Parameters);
                return true;
            }));
        }

        private void RemoveEventsHandlers()
        {
            EventHandlerRegister.RemoveHandler(ChatEvents.ChatMessage);
        }

        public async Task Authenticate(IYield yield)
        {
            if (!IsServerConnected())
            {
                return;
            }

            var parameters = new AuthenticateRequestParameters(AccessTokenProvider.AccessToken);
            var requestId = OperationRequestSender.Send(ChatOperations.Authenticate, parameters, MessageSendOptions.DefaultReliable());
            await SubscriptionProvider.ProvideSubscription<EmptyParameters>(yield, requestId);

            Authenticated?.Invoke();
        }

        public void SendChatMessage(ChatMessageRequestParameters parameters)
        {
            if (!IsServerConnected())
            {
                return;
            }

            OperationRequestSender.Send(ChatOperations.ChatMessage, parameters, MessageSendOptions.DefaultReliable());
        }
    }
}