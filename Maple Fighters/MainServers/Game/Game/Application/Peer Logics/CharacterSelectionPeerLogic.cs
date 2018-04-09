using CommonTools.Log;
using CommunicationHelper;
using Game.Application.PeerLogic.Operations;
using Game.Common;
using PeerLogic.Common;
using PeerLogic.Common.Components;
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

        protected override void OnInitialized()
        {
            AddCommonComponents();
            AddComponents();

            AddHandlerForCreateCharacterOperation();
            AddHandlerForRemoveCharacterOperation();
            AddHandlerForGetCharactersOperation();
            AddHandlerToValidateCharacterOperation();
        }

        private void AddComponents()
        {
            Components.AddComponent(new InactivityTimeout(seconds: 120, lookForOperations: false));

            var userProfileTracker = Components.AddComponent(new UserProfileTracker(userId, ServerType.Game, isUserProfileChanged: true));
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
                ClientPeerWrapper.SetPeerLogic(new GameScenePeerLogic(userId, character.Value));
            }
            else
            {
                var ip = ClientPeerWrapper.Peer.ConnectionInformation.Ip;
                var peerId = ClientPeerWrapper.PeerId;

                LogUtils.Log(MessageBuilder.Trace($"A peer {ip} with id #{peerId} does not have character but trying to enter with character."));

                ClientPeerWrapper.Peer.Disconnect();
            }
        }
    }
}