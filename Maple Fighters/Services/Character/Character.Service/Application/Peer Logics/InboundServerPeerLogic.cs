using CharacterService.Application.PeerLogics.Operations;
using CommunicationHelper;
using PeerLogic.Common;
using ServerCommunicationInterfaces;
using Character.Server.Common;

namespace Character.Service.Application.PeerLogics
{
    internal class InboundServerPeerLogic : PeerLogicBase<CharacterOperations, EmptyEventCode>
    {
        public override void Initialize(IClientPeerWrapper<IClientPeer> peer)
        {
            base.Initialize(peer);

            AddHandlerForGetCharacterOperation();
        }

        private void AddHandlerForGetCharacterOperation()
        {
            OperationHandlerRegister.SetHandler(CharacterOperations.GetCharacter, new GetCharacterOperationHandler());
        }
    }
}