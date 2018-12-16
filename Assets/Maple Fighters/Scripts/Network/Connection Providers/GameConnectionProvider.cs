using System;
using System.Threading.Tasks;
using Authorization.Client.Common;
using CommonCommunicationInterfaces;
using CommonTools.Coroutines;
using CommonTools.Log;
using CommunicationHelper;
using Game.Common;
using Scripts.Containers;
using Scripts.UI.Controllers;
using Scripts.UI.Core;
using Scripts.UI.Windows;
using Scripts.Utils;

namespace Scripts.Services
{
    using Utils = UI.Utils;

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
            var noticeWindow = Utils.ShowNotice($"Connecting to the {serverName} server...", null);
            noticeWindow.OkButton.interactable = false;
        }

        protected override void OnConnectionFailed()
        {
            Action onButtonClicked = delegate 
            {
                GameServerSelectorController.GetInstance().ShowGameServerSelectorUI();
            };

            var noticeWindow = UserInterfaceContainer.GetInstance().Get<NoticeWindow>().AssertNotNull();
            noticeWindow.Message.text = $"Could not connect to the {serverName} server.";
            noticeWindow.OkButton.interactable = true;
            noticeWindow.OkButtonClickedAction = onButtonClicked;
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
            FocusController.GetInstance()?.SetState(Focusable.UI);

            Utils.ShowNotice("The connection has timed out.", SavedObjectsUtils.GoBackToLogin, true, Index.Last);
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
            var noticeWindow = UserInterfaceContainer.GetInstance()?.Get<NoticeWindow>();
            if (noticeWindow != null)
            {
                noticeWindow.Message.text = $"Authorization with {serverName} server failed.";
                noticeWindow.OkButton.interactable = true;
                noticeWindow.OkButtonClickedAction = SavedObjectsUtils.GoBackToLogin;
            }
        }

        protected override void OnAuthorized()
        {
            var noticeWindow = UserInterfaceContainer.GetInstance().Get<NoticeWindow>().AssertNotNull();
            noticeWindow.Hide();

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