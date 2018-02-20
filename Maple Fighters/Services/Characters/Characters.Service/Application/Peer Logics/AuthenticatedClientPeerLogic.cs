using Characters.Common;
using CharactersService.Application.PeerLogics.Operations;
using CommunicationHelper;
using PeerLogic.Common;
using ServerCommunicationInterfaces;

namespace Characters.Service.Application.PeerLogics
{
    internal class AuthenticatedClientPeerLogic : PeerLogicBase<CharactersServiceOperations, EmptyEventCode>
    {
        private readonly int dbUserId;

        public AuthenticatedClientPeerLogic(int dbUserId)
        {
            this.dbUserId = dbUserId;
        }

        public override void Initialize(IClientPeerWrapper<IClientPeer> peer)
        {
            base.Initialize(peer);

            AddHandlerForCreateCharacterOperation();
            AddHandlerForRemoveCharacterOperation();
            AddHandlerForFetchCharactersOperation();
        }

        private void AddHandlerForCreateCharacterOperation()
        {
            OperationRequestHandlerRegister.SetHandler(CharactersServiceOperations.CreateCharacter, new CreateCharacterOperationHandler(dbUserId));
        }

        private void AddHandlerForRemoveCharacterOperation()
        {
            OperationRequestHandlerRegister.SetHandler(CharactersServiceOperations.RemoveCharacter, new RemoveCharacterOperationHandler(dbUserId));
        }

        private void AddHandlerForFetchCharactersOperation()
        {
            OperationRequestHandlerRegister.SetHandler(CharactersServiceOperations.FetchCharacters, new FetchCharactersOperationHandler(dbUserId));
        }
    }
}