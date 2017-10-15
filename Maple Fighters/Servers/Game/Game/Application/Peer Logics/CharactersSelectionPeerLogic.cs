using CommonTools.Log;
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
        private Character? choosedCharacter;

        public CharactersSelectionPeerLogic(int dbUserId)
        {
            this.dbUserId = dbUserId;
        }

        public override void Initialize(IClientPeerWrapper<IClientPeer> peer)
        {
            base.Initialize(peer);

            AddHandlerForEnteredWorldOperation();
            AddHandlerValidateCharacterOperation();
            AddHandlerForFetchCharactersOperation();
            AddHandlerForCreateCharacterOperation();
            AddHandlerForRemoveCharacterOperation();
        }

        private void AddHandlerForEnteredWorldOperation()
        {
            OperationRequestHandlerRegister.SetHandler(GameOperations.EnterWorld, new EnterWorldOperationHandler(OnCharacterEnterWorld));
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
            choosedCharacter = character;
        }

        private void OnCharacterEnterWorld()
        {
            if (!choosedCharacter.HasValue)
            {
                KickOutPeer();
                return;
            }

            PeerWrapper.SetPeerLogic(new AuthenticatedPeerLogic(choosedCharacter.Value));
        }

        private void KickOutPeer()
        {
            var ip = PeerWrapper.Peer.ConnectionInformation.Ip;
            var peerId = PeerWrapper.PeerId;

            LogUtils.Log(MessageBuilder.Trace($"A peer {ip} with id #{peerId} does not have character but trying to enter with character."));

            PeerWrapper.Peer.Disconnect();
        }
    }
}