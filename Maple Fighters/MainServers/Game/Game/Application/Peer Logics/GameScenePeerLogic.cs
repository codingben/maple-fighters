using CommonTools.Log;
using Game.Application.Components.Interfaces;
using Game.Application.PeerLogic.Components;
using Game.Application.PeerLogic.Operations;
using Game.Application.GameObjects;
using PeerLogic.Common;
using ServerApplication.Common.ApplicationBase;
using Game.Common;
using InterestManagement.Components.Interfaces;
using PeerLogic.Common.Components;
using UserProfile.Server.Common;

namespace Game.Application.PeerLogics
{
    internal class GameScenePeerLogic : PeerLogicBase<GameOperations, GameEvents>
    {
        private readonly int userId;
        private readonly PlayerGameObject playerGameObject;

        public GameScenePeerLogic(int userId, CharacterParameters character)
        {
            this.userId = userId;
            playerGameObject = CreatePlayerGameObject(character);
        }

        protected override void OnInitialized()
        {
            AddCommonComponents();
            AddComponents();

            AddHandlerForEnterSceneOperation();
            AddHandlerForUpdatePositionOperation();
            AddHandlerForUpdatePlayerStateOperation();
            AddHandlerForChangeSceneOperation();
        }

        private void AddComponents()
        {
            playerGameObject.Components.AddComponent(new PeerIdGetter(ClientPeerWrapper.PeerId));

            Components.AddComponent(new InactivityTimeout());
            Components.AddComponent(new UserProfileTracker(userId, ServerType.Game, isUserProfileChanged: true));
            Components.AddComponent(new PlayerGameObjectGetter(playerGameObject));
            Components.AddComponent(new InterestManagementNotifier());
            Components.AddComponent(new CharacterSender());
            Components.AddComponent(new PositionChangesListener());
        }

        private void AddHandlerForEnterSceneOperation()
        {
            OperationHandlerRegister.SetHandler(GameOperations.EnterScene, new EnterSceneOperationHandler(playerGameObject));
        }

        private void AddHandlerForUpdatePositionOperation()
        {
            var positionTransform = playerGameObject.Components.GetComponent<IPositionTransform>().AssertNotNull();
            var directionTransform = playerGameObject.Components.GetComponent<IDirectionTransform>().AssertNotNull();
            OperationHandlerRegister.SetHandler(GameOperations.PositionChanged, new UpdatePositionOperationHandler(positionTransform, directionTransform));
        }

        private void AddHandlerForUpdatePlayerStateOperation()
        {
            OperationHandlerRegister.SetHandler(GameOperations.PlayerStateChanged, new UpdatePlayerStateOperationHandler(playerGameObject));
        }

        private void AddHandlerForChangeSceneOperation()
        {
            OperationHandlerRegister.SetHandler(GameOperations.ChangeScene, new ChangeSceneOperationHandler(playerGameObject));
        }

        protected override void OnDispose()
        {
            playerGameObject.Dispose();
        }

        private PlayerGameObject CreatePlayerGameObject(CharacterParameters character)
        {
            var characterSceneObjectCreator = ServerComponents.GetComponent<IPlayerGameObjectCreator>().AssertNotNull();
            var characterSceneObject = characterSceneObjectCreator.Create(character);
            return characterSceneObject;
        }
    }
}