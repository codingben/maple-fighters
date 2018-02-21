using Characters.Client.Common;
using CharactersService.Application.PeerLogics.Operations;
using CommunicationHelper;
using PeerLogic.Common;
using ServerCommunicationInterfaces;

namespace Characters.Service.Application.PeerLogics
{
    internal class AuthenticatedClientPeerLogic : PeerLogicBase<ClientOperations, EmptyEventCode>
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
            AddHandlerForGetCharactersOperation();
        }

        private void AddHandlerForCreateCharacterOperation()
        {
            OperationRequestHandlerRegister.SetHandler(ClientOperations.CreateCharacter, new CreateCharacterOperationHandler(dbUserId));
        }

        private void AddHandlerForRemoveCharacterOperation()
        {
            OperationRequestHandlerRegister.SetHandler(ClientOperations.RemoveCharacter, new RemoveCharacterOperationHandler(dbUserId));
        }

        private void AddHandlerForGetCharactersOperation()
        {
            OperationRequestHandlerRegister.SetHandler(ClientOperations.GetCharacters, new GetCharactersOperationHandler(dbUserId));
        }
    }
}