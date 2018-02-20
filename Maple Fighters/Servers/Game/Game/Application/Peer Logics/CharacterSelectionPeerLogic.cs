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

        public CharacterSelectionPeerLogic(int dbUserId)
        {
            this.dbUserId = dbUserId;
        }

        public override void Initialize(IClientPeerWrapper<IClientPeer> peer)
        {
            base.Initialize(peer);

            AddHandlerToValidateCharacterOperation();
        }

        private void AddHandlerToValidateCharacterOperation()
        {
            OperationRequestHandlerRegister.SetAsyncHandler(GameOperations.ValidateCharacter, new ValidateCharacterOperationHandler(dbUserId, OnCharacterSelected));
        }

        private void OnCharacterSelected(CharacterFromDatabaseParameters? character)
        {
            if (!character.HasValue)
            {
                KickOutPeer();
                return;
            }

            PeerWrapper.SetPeerLogic(new GameScenePeerLogic(character.Value));
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