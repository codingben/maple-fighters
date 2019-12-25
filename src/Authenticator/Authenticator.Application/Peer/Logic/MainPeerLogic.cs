using Authenticator.Application.Peer.Logic.Operations;
using Authenticator.Common.Enums;
using Authenticator.Domain.Aggregates.User.Services;
using CommonTools.Log;
using CommunicationHelper;
using ServerCommon.PeerLogic.Common;

namespace Authenticator.Application.Peer.Logic
{
    public class MainPeerLogic : InboundPeerLogicBase<AuthenticatorOperations, EmptyEventCode>
    {
        protected override void OnSetup()
        {
            base.OnSetup();

            LogUtils.Log("OnCleanup()");

            AddComponents();

            AddHandlerForLoginOperation();
            AddHandlerForRegisterOperation();
        }

        protected override void OnCleanup()
        {
            base.OnCleanup();

            LogUtils.Log("OnCleanup()");
        }

        private void AddComponents()
        {
            Components.Add(new LoginService());
            Components.Add(new RegistrationService());
        }

        private void AddHandlerForLoginOperation()
        {
            var loginService = Components.Get<ILoginService>().AssertNotNull();

            OperationHandlerRegister.SetHandler(
                AuthenticatorOperations.Login,
                new LoginOperationHandler(loginService));
        }

        private void AddHandlerForRegisterOperation()
        {
            var registrationService = Components.Get<IRegistrationService>()
                .AssertNotNull();

            OperationHandlerRegister.SetHandler(
                AuthenticatorOperations.Register,
                new RegisterOperationHandler(registrationService));
        }
    }
}