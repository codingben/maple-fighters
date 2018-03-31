using CommunicationHelper;
using PeerLogic.Common;
using PeerLogic.Common.Components;
using Registration.Application.PeerLogic.Operations;
using Registration.Common;
using ServerCommunicationInterfaces;

namespace Registration.Application.PeerLogic
{
    internal class RegistrationPeerLogic : PeerLogicBase<RegistrationOperations, EmptyEventCode>
    {
        public override void Initialize(IClientPeerWrapper<IClientPeer> peer)
        {
            base.Initialize(peer);

            AddCommonComponents();
            AddComponents();

            AddHandlerForRegisterOperation();
        }

        private void AddComponents()
        {
            Components.AddComponent(new InactivityTimeout());
        }

        private void AddHandlerForRegisterOperation()
        {
            OperationHandlerRegister.SetHandler(RegistrationOperations.Register, new RegisterOperationHandler());
        }
    }
}