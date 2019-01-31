using System.Threading.Tasks;
using Authorization.Client.Common;
using CommonCommunicationInterfaces;
using CommonTools.Coroutines;
using CommunicationHelper;
using GameServerProvider.Client.Common;
using Scripts.Containers;

namespace Scripts.Services
{
    public class GameServerSelectorConnectionProvider : ServiceConnectionProviderBase<GameServerSelectorConnectionProvider>
    {
        private AuthorizationStatus authorizationStatus =
            AuthorizationStatus.Failed;

        public void Connect()
        {
            var serverConnectionInformation =
                GetServerConnectionInformation(ServerType.GameServerProvider);
            CoroutinesExecutor.StartTask(
                (yield) => Connect(yield, serverConnectionInformation));
        }

        protected override void OnPreConnection()
        {
            // TODO: Show notice: "Connecting to master server..."
        }

        protected override void OnConnectionFailed()
        {
            // TODO: Show notice: "Could not connect to master server."
            // TODO: Ok: GoBackToLogin()
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

            GoBackToLogin(reason);
        }

        private void GoBackToLogin(DisconnectReason reason)
        {
            if (authorizationStatus == AuthorizationStatus.Failed)
            {
                OnNonAuthorized();
            }
            else if (reason != DisconnectReason.ServerDisconnect)
            {
                ShowConnectionTimeout();
            }
        }

        private void ShowConnectionTimeout()
        {
            // TODO: Show notice: "The connection has timed out."
            // TODO: Ok: GoBackToLogin()
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
            // TODO: Show notice: "Authorization with master server failed."
            // TODO: Ok: GoBackToLogin()
        }

        protected override void OnAuthorized()
        {
            authorizationStatus = AuthorizationStatus.Succeed;

            // GameServerSelectorController.GetInstance().ShowGameServerSelectorUI();
        }

        protected override void SetPeerLogicAfterAuthorization()
        {
            GetServiceBase()
                .SetPeerLogic<GameServerProviderPeerLogic, GameServerProviderOperations, EmptyEventCode>();
        }

        protected override IServiceBase GetServiceBase()
        {
            return ServiceContainer.GameServerProviderService;
        }
    }
}