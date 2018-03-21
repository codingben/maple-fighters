using CommonTools.Log;
using CommunicationHelper;
using Game.Application.PeerLogic.Operations;
using Game.Common;
using PeerLogic.Common;
using ServerCommunicationInterfaces;
using UserProfile.Server.Common;

namespace Game.Application.PeerLogics
{
    internal class CharacterSelectionPeerLogic : PeerLogicBase<CharacterOperations, EmptyEventCode>
    {
        private readonly int userId;

        public CharacterSelectionPeerLogic(int userId)
        {
            this.userId = userId;
        }

        public override void Initialize(IClientPeerWrapper<IClientPeer> peer)
        {
            base.Initialize(peer);

            AddCommonComponents();
            AddComponents();

            AddHandlerForCreateCharacterOperation();
            AddHandlerForRemoveCharacterOperation();
            AddHandlerForGetCharactersOperation();
            AddHandlerToValidateCharacterOperation();
        }

        private void AddComponents()
        {
            var userProfileTracker = Components.AddComponent(new UserProfileTracker(userId, ServerType.Game));
            userProfileTracker.ChangeUserProfileProperties();
        }

        private void AddHandlerForCreateCharacterOperation()
        {
            OperationHandlerRegister.SetAsyncHandler(CharacterOperations.CreateCharacter, new CreateCharacterOperationHandler(userId));
        }

        private void AddHandlerForRemoveCharacterOperation()
        {
            OperationHandlerRegister.SetAsyncHandler(CharacterOperations.RemoveCharacter, new RemoveCharacterOperationHandler(userId));
        }

        private void AddHandlerForGetCharactersOperation()
        {
            OperationHandlerRegister.SetAsyncHandler(CharacterOperations.GetCharacters, new GetCharactersOperationHandler(userId));
        }

        private void AddHandlerToValidateCharacterOperation()
        {
            OperationHandlerRegister.SetAsyncHandler(CharacterOperations.ValidateCharacter, new CharacterValidationOperationHandler(userId, OnCharacterSelected));
        }

        private void OnCharacterSelected(CharacterParameters? character)
        {
            if (character.HasValue)
            {
                PeerWrapper.SetPeerLogic(new GameScenePeerLogic(userId, character.Value));
            }
            else
            {
                var ip = PeerWrapper.Peer.ConnectionInformation.Ip;
                var peerId = PeerWrapper.PeerId;

                LogUtils.Log(MessageBuilder.Trace($"A peer {ip} with id #{peerId} does not have character but trying to enter with character."));

                PeerWrapper.Peer.Disconnect();
            }
        }
    }
}