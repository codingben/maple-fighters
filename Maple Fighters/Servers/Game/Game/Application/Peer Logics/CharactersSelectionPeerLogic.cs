using CommunicationHelper;
using Game.Application.PeerLogic.Operations;
using ServerCommunicationInterfaces;
using Shared.Game.Common;
using Shared.ServerApplication.Common.PeerLogic;

namespace Game.Application.PeerLogics
{
    internal class CharactersSelectionPeerLogic : PeerLogicBase<GameOperations, EmptyEventCode>
    {
        private readonly int dbUserId;

        public CharactersSelectionPeerLogic(int dbUserId)
        {
            this.dbUserId = dbUserId;
        }

        public override void Initialize(IClientPeerWrapper<IClientPeer> peer)
        {
            base.Initialize(peer);

            AddHandlerValidateCharacterOperation();
            AddHandlerForFetchCharactersOperation();
            AddHandlerForCreateCharacterOperation();
            AddHandlerForRemoveCharacterOperation();
        }

        private void AddHandlerValidateCharacterOperation()
        {
            OperationRequestHandlerRegister.SetHandler(GameOperations.ValidateCharacter, new ValidateCharacterOperationHandler(dbUserId, OnCharacterSelected));
        }

        private void AddHandlerForFetchCharactersOperation()
        {
            OperationRequestHandlerRegister.SetHandler(GameOperations.FetchCharacters, new FetchCharactersOperationHandler(dbUserId));
        }

        private void AddHandlerForCreateCharacterOperation()
        {
            OperationRequestHandlerRegister.SetHandler(GameOperations.CreateCharacter, new CreateCharacterOperationHandler(dbUserId));
        }

        private void AddHandlerForRemoveCharacterOperation()
        {
            OperationRequestHandlerRegister.SetHandler(GameOperations.RemoveCharacter, new RemoveCharacterOperationHandler(dbUserId));
        }

        private void OnCharacterSelected(Character character)
        {
            PeerWrapper.SetPeerLogic(new AuthenticatedPeerLogic(character));
        }
    }
}