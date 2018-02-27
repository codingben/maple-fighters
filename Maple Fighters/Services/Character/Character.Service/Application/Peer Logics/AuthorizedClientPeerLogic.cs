using Character.Client.Common;
using CharacterService.Application.PeerLogics.Operations;
using CommunicationHelper;
using PeerLogic.Common;
using ServerCommunicationInterfaces;

namespace Character.Service.Application.PeerLogics
{
    internal class AuthorizedClientPeerLogic : PeerLogicBase<CharacterOperations, EmptyEventCode>
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
            OperationHandlerRegister.SetHandler(CharacterOperations.CreateCharacter, new CreateCharacterOperationHandler(userId));
        }

        private void AddHandlerForRemoveCharacterOperation()
        {
            OperationHandlerRegister.SetHandler(CharacterOperations.RemoveCharacter, new RemoveCharacterOperationHandler(userId));
        }

        private void AddHandlerForGetCharactersOperation()
        {
            OperationHandlerRegister.SetHandler(CharacterOperations.GetCharacters, new GetCharactersOperationHandler(userId));
        }
    }
}