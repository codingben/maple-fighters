using CommonTools.Log;
using CommunicationHelper;
using Game.Application.PeerLogic.Operations;
using PeerLogic.Common;
using ServerCommunicationInterfaces;
using Shared.Game.Common;

namespace Game.Application.PeerLogics
{
    internal class CharacterSelectionPeerLogic : PeerLogicBase<GameOperations, EmptyEventCode>
    {
        private readonly int dbUserId;
        private CharacterFromDatabase? choosedCharacter;

        public CharacterSelectionPeerLogic(int dbUserId)
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

        private void OnCharacterSelected(CharacterFromDatabase character)
        {
            choosedCharacter = character;

            if (choosedCharacter == null)
            {
                KickOutPeer();
                return;
            }

            PeerWrapper.SetPeerLogic(new GameScenePeerLogic(choosedCharacter.Value));
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