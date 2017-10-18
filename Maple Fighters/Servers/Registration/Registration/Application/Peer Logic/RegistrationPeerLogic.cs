using CommunicationHelper;
using PeerLogic.Common;
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

            AddHandlerForRegisterOperation();
        }

        private void AddHandlerForRegisterOperation()
        {
            OperationRequestHandlerRegister.SetHandler(RegistrationOperations.Register, new RegisterOperationHandler());
        }
    }
}