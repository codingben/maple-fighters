using Characters.Client.Common;
using CharactersService.Application.PeerLogics.Operations;
using CommunicationHelper;
using PeerLogic.Common;
using ServerCommunicationInterfaces;

namespace Characters.Service.Application.PeerLogics
{
    internal class AuthorizedClientPeerLogic : PeerLogicBase<ClientOperations, EmptyEventCode>
    {
        private readonly int userId;

        public AuthorizedClientPeerLogic(int userId)
        {
            this.userId = userId;
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
            OperationRequestHandlerRegister.SetHandler(ClientOperations.CreateCharacter, new CreateCharacterOperationHandler(userId));
        }

        private void AddHandlerForRemoveCharacterOperation()
        {
            OperationRequestHandlerRegister.SetHandler(ClientOperations.RemoveCharacter, new RemoveCharacterOperationHandler(userId));
        }

        private void AddHandlerForGetCharactersOperation()
        {
            OperationRequestHandlerRegister.SetHandler(ClientOperations.GetCharacters, new GetCharactersOperationHandler(userId));
        }
    }
}