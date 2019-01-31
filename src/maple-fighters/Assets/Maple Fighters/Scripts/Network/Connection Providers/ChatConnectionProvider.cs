using System.Threading.Tasks;
using Authorization.Client.Common;
using Chat.Common;
using CommonCommunicationInterfaces;
using CommonTools.Coroutines;
using Scripts.Containers;

namespace Scripts.Services
{
    public class ChatConnectionProvider : ServiceConnectionProviderBase<ChatConnectionProvider>
    {
        private AuthorizationStatus authorizationStatus =
            AuthorizationStatus.Failed;

        public void Connect()
        {
            var serverConnectionInformation =
                GetServerConnectionInformation(ServerType.Chat);
            CoroutinesExecutor.StartTask(
                (yield) => Connect(yield, serverConnectionInformation));
        }

        protected override void OnPreConnection()
        {
            // TODO: Add message: "Connecting to a chat server..."
        }

        protected override void OnConnectionFailed()
        {
            // TODO: Add message: "Could not connect to a chat server."
        }

        protected override void OnConnectionEstablished()
        {
            CoroutinesExecutor.StartTask(Authorize);
        }

        protected override void OnDisconnected(
            DisconnectReason reason,
            string details)
        {
            base.OnDisconnected(reason, details);

            if (authorizationStatus == AuthorizationStatus.Succeed)
            {
                // TODO: Add message: "Could not connect to a chat server."
            }
        }

        protected override Task<AuthorizeResponseParameters> Authorize(
            IYield yield,
            AuthorizeRequestParameters parameters)
        {
            var authorizationPeerLogic = GetServiceBase()
                .GetPeerLogic<IAuthorizationPeerLogicAPI>();
            return authorizationPeerLogic.Authorize(yield, parameters);
        }

        protected override void OnPreAuthorization()
        {
            // Left blank intentionally
        }

        protected override void OnNonAuthorized()
        {
            // TODO: Add message: "Authorization with chat server failed."
        }

        protected override void OnAuthorized()
        {
            authorizationStatus = AuthorizationStatus.Succeed;

            // TODO: Add message: "Connected to a chat server successfully."
        }

        protected override void SetPeerLogicAfterAuthorization()
        {
            GetServiceBase()
                .SetPeerLogic<ChatPeerLogic, ChatOperations, ChatEvents>();
        }

        protected override IServiceBase GetServiceBase()
        {
            return ServiceContainer.ChatService;
        }
    }
}