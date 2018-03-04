using System;
using System.Threading.Tasks;
using Authorization.Client.Common;
using CommonCommunicationInterfaces;
using CommonTools.Coroutines;
using CommonTools.Log;
using Scripts.Containers;
using Scripts.UI.Core;
using Scripts.UI.Windows;
using Scripts.Utils;

namespace Scripts.Services
{
    public class CharacterConnectionProvider : ServiceConnectionProviderBase<CharacterConnectionProvider>
    {
        private Action onAuthorized;
        private AuthorizationStatus authorizationStatus;

        public void Connect(Action onAuthorized)
        {
            this.onAuthorized = onAuthorized;

            var serverConnectionInformation = GetServerConnectionInformation(ServerType.Character);
            CoroutinesExecutor.StartTask((yield) => Connect(yield, serverConnectionInformation));
        }

        protected override void OnPreConnection()
        {
            // Left blank intentionally
        }

        protected override void OnConnectionFailed()
        {
            var isNoticeWindowExists = UserInterfaceContainer.Instance.Get<NoticeWindow>() != null;
            if (isNoticeWindowExists)
            {
                var noticeWindow = UserInterfaceContainer.Instance.Get<NoticeWindow>();
                noticeWindow.Message.text = "Could not connect to a character service.";
                noticeWindow.OkButton.interactable = true;
                noticeWindow.OkButtonClickedAction = LoadedObjects.DestroyAll;
            }
            else
            {
                var noticeWindow = UI.Utils.ShowNotice("Could not connect to a character service.", LoadedObjects.DestroyAll);
                noticeWindow.OkButton.interactable = true;
            }
        }

        protected override void OnConnectionEstablished()
        {
            CoroutinesExecutor.StartTask(Authorize);

            const int TIME_TO_DISCONNECT = 120;
            DisconnectAutomatically(TIME_TO_DISCONNECT);
        }

        protected override void OnDisconnected(DisconnectReason reason, string details)
        {
            base.OnDisconnected(reason, details);

            GoBackToLogin();
        }

        private void GoBackToLogin()
        {
            if (authorizationStatus == AuthorizationStatus.Failed)
            {
                UI.Utils.ShowNotice("Authorization with character service failed.", LoadedObjects.DestroyAll);
            }
        }

        protected override Task<AuthorizeResponseParameters> Authorize(IYield yield, AuthorizeRequestParameters parameters)
        {
            return ServiceContainer.CharacterService.Authorize(yield, parameters);
        }

        protected override void OnPreAuthorization()
        {
            // Left blank intentionally
        }

        protected override void OnAuthorized()
        {
            var noticeWindow = UserInterfaceContainer.Instance.Get<NoticeWindow>().AssertNotNull();
            noticeWindow.Hide();

            authorizationStatus = AuthorizationStatus.Succeed;

            onAuthorized?.Invoke();
        }

        protected override IServiceBase GetServiceBase()
        {
            return ServiceContainer.CharacterService;
        }
    }
}