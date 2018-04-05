using CommonTools.Log;
using Game.Application.Components.Interfaces;
using Game.Application.PeerLogic.Components;
using Game.Application.PeerLogic.Operations;
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
        private readonly ISceneObject sceneObject;

        public GameScenePeerLogic(int userId, CharacterParameters character)
        {
            this.userId = userId;
            sceneObject = CreateSceneObject(character);
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
            sceneObject.Components.AddComponent(new PeerIdGetter(ClientPeerWrapper.PeerId));

            Components.AddComponent(new InactivityTimeout());
            Components.AddComponent(new UserProfileTracker(userId, ServerType.Game, isUserProfileChanged: true));
            Components.AddComponent(new SceneObjectGetter(sceneObject));
            Components.AddComponent(new InterestManagementNotifier());
            Components.AddComponent(new CharacterSender());
            Components.AddComponent(new PositionChangesListener());
        }

        private void AddHandlerForEnterSceneOperation()
        {
            OperationHandlerRegister.SetHandler(GameOperations.EnterScene, new EnterSceneOperationHandler(sceneObject));
        }

        private void AddHandlerForUpdatePositionOperation()
        {
            var positionTransform = sceneObject.Components.GetComponent<IPositionTransform>().AssertNotNull();
            var directionTransform = sceneObject.Components.GetComponent<IDirectionTransform>().AssertNotNull();
            OperationHandlerRegister.SetHandler(GameOperations.PositionChanged, new UpdatePositionOperationHandler(positionTransform, directionTransform));
        }

        private void AddHandlerForUpdatePlayerStateOperation()
        {
            OperationHandlerRegister.SetHandler(GameOperations.PlayerStateChanged, new UpdatePlayerStateOperationHandler(sceneObject));
        }

        private void AddHandlerForChangeSceneOperation()
        {
            OperationHandlerRegister.SetHandler(GameOperations.ChangeScene, new ChangeSceneOperationHandler(sceneObject));
        }

        protected override void OnDispose()
        {
            sceneObject.Dispose();
        }

        private ISceneObject CreateSceneObject(CharacterParameters character)
        {
            var characterSceneObjectCreator = Server.Components.GetComponent<ICharacterCreator>().AssertNotNull();
            var characterSceneObject = characterSceneObjectCreator.Create(character);
            return characterSceneObject;
        }
    }
}