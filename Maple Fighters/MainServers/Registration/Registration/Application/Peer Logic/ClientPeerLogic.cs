using CommunicationHelper;
using PeerLogic.Common;
using PeerLogic.Common.Components;
using Registration.Application.PeerLogic.Operations;
using Registration.Common;

namespace Registration.Application.PeerLogic
{
    internal class ClientPeerLogic : PeerLogicBase<RegistrationOperations, EmptyEventCode>
    {
        protected override void OnInitialized()
        {
            AddCommonComponents();
            AddComponents();

            AddHandlerForRegisterOperation();
        }

        private void AddComponents()
        {
            Components.AddComponent(new InactivityTimeout(seconds: 60, lookForOperations: false));
        }

        private void AddHandlerForRegisterOperation()
        {
            OperationHandlerRegister.SetHandler(RegistrationOperations.Register, new RegisterOperationHandler());
        }
    }
}