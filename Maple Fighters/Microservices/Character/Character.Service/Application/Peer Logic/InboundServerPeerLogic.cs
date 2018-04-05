using CharacterService.Application.PeerLogics.Operations;
using CommunicationHelper;
using PeerLogic.Common;
using Character.Server.Common;

namespace Character.Service.Application.PeerLogic
{
    internal class InboundServerPeerLogic : PeerLogicBase<CharacterOperations, EmptyEventCode>
    {
        protected override void OnInitialized()
        {
            AddHandlerForCreateCharacterOperation();
            AddHandlerForRemoveCharacterOperation();
            AddHandlerForGetCharactersOperation();
            AddHandlerForGetCharacterOperation();
        }

        private void AddHandlerForCreateCharacterOperation()
        {
            OperationHandlerRegister.SetHandler(CharacterOperations.CreateCharacter, new CreateCharacterOperationHandler());
        }

        private void AddHandlerForRemoveCharacterOperation()
        {
            OperationHandlerRegister.SetHandler(CharacterOperations.RemoveCharacter, new RemoveCharacterOperationHandler());
        }

        private void AddHandlerForGetCharactersOperation()
        {
            OperationHandlerRegister.SetHandler(CharacterOperations.GetCharacters, new GetCharactersOperationHandler());
        }

        private void AddHandlerForGetCharacterOperation()
        {
            OperationHandlerRegister.SetHandler(CharacterOperations.GetCharacter, new GetCharacterOperationHandler());
        }
    }
}