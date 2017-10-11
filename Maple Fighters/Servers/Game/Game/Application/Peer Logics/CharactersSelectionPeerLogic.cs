using CommunicationHelper;
using Game.Application.PeerLogic.Operations;
using Game.InterestManagement;
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

            AddHandlerForEnterWorldOperation();
            AddHandlerForFetchCharactersOperation();
            AddHandlerForCreateCharacterOperation();
            AddHandlerForRemoveCharacterOperation();
        }

        private void AddHandlerForEnterWorldOperation()
        {
            OperationRequestHandlerRegister.SetHandler(GameOperations.EnterWorld, new EnterWorldOperationHandler(dbUserId, OnCharacterSelected));
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

        private void OnCharacterSelected(IGameObject gameObject)
        {
            PeerWrapper.SetPeerLogic(new AuthenticatedPeerLogic(gameObject));
        }
    }
}