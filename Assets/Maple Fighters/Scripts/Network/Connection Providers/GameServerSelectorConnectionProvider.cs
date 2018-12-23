using System.Threading.Tasks;
using Authorization.Client.Common;
using CommonCommunicationInterfaces;
using CommonTools.Coroutines;
using CommonTools.Log;
using CommunicationHelper;
using GameServerProvider.Client.Common;
using Scripts.Containers;
using Scripts.UI.Controllers;
using Scripts.UI.Core;
using Scripts.UI.Windows;
using Scripts.Utils;

namespace Scripts.Services
{
    public class GameServerSelectorConnectionProvider : ServiceConnectionProviderBase<GameServerSelectorConnectionProvider>
    {
        private AuthorizationStatus authorizationStatus = AuthorizationStatus.Failed;

        public void Connect()
        {
            var serverConnectionInformation = GetServerConnectionInformation(ServerType.GameServerProvider);
            CoroutinesExecutor.StartTask((yield) => Connect(yield, serverConnectionInformation));
        }

        protected override void OnPreConnection()
        {
            var noticeWindow = UserInterfaceContainer.GetInstance().Get<NoticeWindow>().AssertNotNull();
            noticeWindow.Message.text = "Connecting to master server...";
        }

        protected override void OnConnectionFailed()
        {
            var noticeWindow = UserInterfaceContainer.GetInstance().Get<NoticeWindow>().AssertNotNull();
            noticeWindow.Message.text = "Could not connect to master server.";
            noticeWindow.OkButton.interactable = true;
            noticeWindow.OkButtonClickedAction = SavedGameObjectsUtils.GetInstance().GoBackToLogin;
        }

        protected override void OnConnectionEstablished()
        {
            CoroutinesExecutor.StartTask(Authorize);
        }

        protected override void OnDisconnected(DisconnectReason reason, string details)
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
            UI.Utils.ShowNotice("The connection has timed out.", SavedGameObjectsUtils.GetInstance().GoBackToLogin, true, Index.Last);
        }

        protected override Task<AuthorizeResponseParameters> Authorize(IYield yield, AuthorizeRequestParameters parameters)
        {
            var authorizationPeerLogic = GetServiceBase().GetPeerLogic<IAuthorizationPeerLogicAPI>().AssertNotNull();
            return authorizationPeerLogic.Authorize(yield, parameters);
        }

        protected override void OnPreAuthorization()
        {
            // Left blank intentionally
        }

        protected override void OnNonAuthorized()
        {
            var noticeWindow = UserInterfaceContainer.GetInstance().Get<NoticeWindow>().AssertNotNull();
            noticeWindow.Message.text = "Authorization with master server failed.";
            noticeWindow.OkButton.interactable = true;
            noticeWindow.OkButtonClickedAction = SavedGameObjectsUtils.GetInstance().GoBackToLogin;
        }

        protected override void OnAuthorized()
        {
            var noticeWindow = UserInterfaceContainer.GetInstance().Get<NoticeWindow>().AssertNotNull();
            noticeWindow.Hide();

            authorizationStatus = AuthorizationStatus.Succeed;

            GameServerSelectorController.GetInstance().ShowGameServerSelectorUI();
        }

        protected override void SetPeerLogicAfterAuthorization()
        {
            GetServiceBase().SetPeerLogic<GameServerProviderPeerLogic, GameServerProviderOperations, EmptyEventCode>();
        }

        protected override IServiceBase GetServiceBase()
        {
            return ServiceContainer.GameServerProviderService;
        }
    }
}