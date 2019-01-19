using System;
using System.Threading.Tasks;
using Authorization.Client.Common;
using CommonTools.Coroutines;
using CommunicationHelper;
using Login.Common;
using Scripts.Containers;
using Scripts.Services;

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
            // TODO: Show notice: "Could not connect to a login server."
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
            GetServiceBase().SetPeerLogic<LoginPeerLogic, LoginOperations, EmptyEventCode>();
        }

        protected override IServiceBase GetServiceBase()
        {
            return ServiceContainer.LoginService;
        }
    }
}