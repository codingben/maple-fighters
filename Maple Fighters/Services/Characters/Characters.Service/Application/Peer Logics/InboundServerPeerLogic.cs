using Characters.Server.Common;
using CharactersService.Application.PeerLogics.Operations;
using CommunicationHelper;
using PeerLogic.Common;
using ServerCommunicationInterfaces;

namespace Characters.Service.Application.PeerLogics
{
    internal class InboundServerPeerLogic : PeerLogicBase<ServerOperations, EmptyEventCode>
    {
        public override void Initialize(IClientPeerWrapper<IClientPeer> peer)
        {
            base.Initialize(peer);

            AddHandlerForGetCharacterOperation();
        }

        private void AddHandlerForGetCharacterOperation()
        {
            OperationRequestHandlerRegister.SetHandler(ServerOperations.GetCharacter, new GetCharacterOperationHandler());
        }
    }
}