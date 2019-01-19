using System;
using System.Threading.Tasks;
using Authorization.Client.Common;
using CommonCommunicationInterfaces;
using CommonTools.Coroutines;
using CommunicationHelper;
using Game.Common;
using Scripts.Containers;

namespace Scripts.Services
{
    public class GameConnectionProvider : ServiceConnectionProviderBase<GameConnectionProvider>
    {
        private Action onAuthorized;
        private AuthorizationStatus authorizationStatus = AuthorizationStatus.Failed;
        private string serverName;

        public void Connect(string serverName, Action onAuthorized, PeerConnectionInformation peerConnectionInformation)
        {
            this.serverName = serverName;
            this.onAuthorized = onAuthorized;

            var serverConnectionInformation = GetServerConnectionInformation(ServerType.Game, peerConnectionInformation);
            CoroutinesExecutor.StartTask((yield) => Connect(yield, serverConnectionInformation));
        }

        protected override void OnPreConnection()
        {
            // TODO: Show notice: "Connecting to the {serverName} server..."
        }

        protected override void OnConnectionFailed()
        {
            // TODO: Show notice: "Could not connect to the {serverName} server."
            // TODO: Ok: ShowGameServerSelectorUI()
        }

        protected override void OnConnectionEstablished()
        {
            CoroutinesExecutor.StartTask(Authorize);
        }

        protected override void OnDisconnected(DisconnectReason reason, string details)
        {
            base.OnDisconnected(reason, details);

            GoBackToLogin();
        }

        private void GoBackToLogin()
        {
            if (authorizationStatus == AuthorizationStatus.Succeed && !IsDestroying)
            {
                ShowConnectionTimeout();
            }
            else if (!IsDestroying)
            {
                OnNonAuthorized();
            }
        }

        private void ShowConnectionTimeout()
        {
            // TODO: FocusStateController.GetInstance().SetState(FocusState.UI);
            // TODO: Show notice: "The connection has timed out."
            // TODO: Ok: GoBackToLogin()
        }

        protected override Task<AuthorizeResponseParameters> Authorize(IYield yield, AuthorizeRequestParameters parameters)
        {
            var authorizationPeerLogic = GetServiceBase().GetPeerLogic<IAuthorizationPeerLogicAPI>();
            return authorizationPeerLogic.Authorize(yield, parameters);
        }

        protected override void OnPreAuthorization()
        {
            // Left blank intentionally
        }

        protected override void OnNonAuthorized()
        {
            // TODO: Show notice: $"Authorization with {serverName} server failed."
            // TODO: Ok: GoBackToLogin()
        }

        protected override void OnAuthorized()
        {
            authorizationStatus = AuthorizationStatus.Succeed;

            onAuthorized.Invoke();
        }

        protected override void SetPeerLogicAfterAuthorization()
        {
            GetServiceBase().SetPeerLogic<CharacterPeerLogic, CharacterOperations, EmptyEventCode>();
        }

        protected override IServiceBase GetServiceBase()
        {
            return ServiceContainer.GameService;
        }
    }
}