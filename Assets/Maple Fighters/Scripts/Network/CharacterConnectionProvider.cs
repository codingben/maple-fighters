using System;
using System.Threading.Tasks;
using Authorization.Client.Common;
using CommonTools.Coroutines;
using CommonTools.Log;
using Scripts.Containers;
using Scripts.UI.Core;
using Scripts.UI.Windows;
using Scripts.Utils;

namespace Scripts.Services
{
    public class CharacterConnectionProvider : ServiceConnectionProvider<CharacterConnectionProvider>
    {
        private Action onAuthorized;

        public void Connect(Action onAuthorized)
        {
            this.onAuthorized = onAuthorized;

            var serverConnectionInformation = GetServerConnectionInformation(ServerType.Character);
            CoroutinesExecutor.StartTask((yield) => Connect(yield, ServiceContainer.CharacterService, serverConnectionInformation));
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

            onAuthorized?.Invoke();
        }
    }
}