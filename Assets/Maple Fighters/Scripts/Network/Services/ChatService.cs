using System.Threading.Tasks;
using Chat.Common;
using CommonCommunicationInterfaces;
using CommonTools.Coroutines;

namespace Scripts.Services
{
    public sealed class ChatService : ServiceBase<ChatOperations, ChatEvents>, IChatService
    {
        public UnityEvent<ChatMessageEventParameters> ChatMessageReceived { get; } = new UnityEvent<ChatMessageEventParameters>();

        protected override void OnConnected()
        {
            AddEventsHandlers();
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

        public async Task<AuthenticationStatus> Authenticate(IYield yield)
        {
            if (!IsConnected())
            {
                return AuthenticationStatus.Failed;
            }

            var parameters = new AuthenticateRequestParameters(AccessTokenProvider.AccessToken);
            var requestId = OperationRequestSender.Send(ChatOperations.Authenticate, parameters, MessageSendOptions.DefaultReliable());
            var authenticationStatus = await SubscriptionProvider.ProvideSubscription<AuthenticateResponseParameters>(yield, requestId);
            return authenticationStatus.Status;
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