using CommunicationHelper;
using Registration.Application.PeerLogic.Operations;
using Registration.Common;
using ServerCommunicationInterfaces;
using Shared.ServerApplication.Common.PeerLogic;

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