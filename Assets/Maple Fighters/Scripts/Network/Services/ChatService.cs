using System.Threading.Tasks;
using Chat.Common;
using CommonCommunicationInterfaces;
using CommonTools.Coroutines;
using CommunicationHelper;

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
    }
}