using System;
using System.Threading.Tasks;
using Authorization.Client.Common;
using CommonTools.Coroutines;
using CommonTools.Log;
using CommunicationHelper;
using Login.Common;
using Scripts.Containers;
using Scripts.Services;
using Scripts.UI.Core;
using Scripts.UI.Windows;

namespace Scripts.UI.Controllers
{
    public class LoginConnectionProvider : ServiceConnectionProviderBase<LoginConnectionProvider>
    {
        private Action onConnected;

        public void Connect(Action onConnected)
        {
            this.onConnected = onConnected;

            var serverConnectionInformation = GetServerConnectionInformation(ServerType.Login);
            CoroutinesExecutor.StartTask((yield) => Connect(yield, serverConnectionInformation, authorize: false));
        }

        protected override void OnPreConnection()
        {
            // Left blank intentionally
        }

        protected override void OnConnectionFailed()
        {
            var noticeWindow = UserInterfaceContainer.Instance.Get<NoticeWindow>().AssertNotNull();
            noticeWindow.Message.text = "Could not connect to a login server.";
            noticeWindow.OkButton.interactable = true;
        }

        protected override void OnConnectionEstablished()
        {
            onConnected.Invoke();
        }

        protected override Task<AuthorizeResponseParameters> Authorize(IYield yield, AuthorizeRequestParameters parameters)
        {
            throw new NotImplementedException();
        }

        protected override void OnPreAuthorization()
        {
            // Left blank intentionally
        }

        protected override void OnNonAuthorized()
        {
            // Left blank intentionally
        }

        protected override void OnAuthorized()
        {
            // Left blank intentionally
        }

        protected override void SetPeerLogicAfterAuthorization()
        {
            GetServiceBase().SetPeerLogic<LoginPeerLogic, LoginOperations, EmptyEventCode>(new LoginPeerLogic());
        }

        protected override IServiceBase GetServiceBase()
        {
            return ServiceContainer.LoginService;
        }
    }
}