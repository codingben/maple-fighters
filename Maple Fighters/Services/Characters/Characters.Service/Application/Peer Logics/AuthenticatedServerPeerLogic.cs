using Characters.Common;
using CharactersService.Application.PeerLogics.Operations;
using CommunicationHelper;
using PeerLogic.Common;
using ServerCommunicationInterfaces;

namespace Characters.Service.Application.PeerLogics
{
    internal class AuthenticatedServerPeerLogic : PeerLogicBase<CharactersServiceOperations, EmptyEventCode>
    {
        public override void Initialize(IClientPeerWrapper<IClientPeer> peer)
        {
            base.Initialize(peer);

            AddHandlerForFetchCharacterOperation();
        }

        private void AddHandlerForFetchCharacterOperation()
        {
            OperationRequestHandlerRegister.SetHandler(CharactersServiceOperations.FetchCharacter, new FetchCharacterOperationHandler());
        }
    }
}