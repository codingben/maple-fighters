using CommonTools.Log;
using Game.Application.Components;
using Game.Application.PeerLogic.Components;
using Game.Application.PeerLogic.Operations;
using Game.InterestManagement;
using PeerLogic.Common;
using ServerApplication.Common.ApplicationBase;
using ServerCommunicationInterfaces;
using Game.Common;
using PeerLogic.Common.Components;
using UserProfile.Server.Common;

namespace Game.Application.PeerLogics
{
    internal class GameScenePeerLogic : PeerLogicBase<GameOperations, GameEvents>
    {
        private readonly int userId;
        private readonly ISceneObject sceneObject;

        private const int INACTIVITY_TIMEOUT = 120; // In seconds

        public GameScenePeerLogic(int userId, CharacterParameters character)
        {
            this.userId = userId;
            sceneObject = CreateSceneObject(character);
        }

        public override void Initialize(IClientPeerWrapper<IClientPeer> peer)
        {
            base.Initialize(peer);

            AddCommonComponents();
            AddComponents();

            AddHandlerForEnterSceneOperation();
            AddHandlerForUpdatePositionOperation();
            AddHandlerForUpdatePlayerStateOperation();
            AddHandlerForChangeSceneOperation();
        }

        private void AddComponents()
        {
            sceneObject.Components.AddComponent(new PeerIdGetter(PeerWrapper.PeerId));

            Components.AddComponent(new InactivityTimeout(INACTIVITY_TIMEOUT, lookForOperationsRequest: true));
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
            var transform = sceneObject.Components.GetComponent<ITransform>().AssertNotNull();
            OperationHandlerRegister.SetHandler(GameOperations.PositionChanged, new UpdatePositionOperationHandler(transform));
        }

        private void AddHandlerForUpdatePlayerStateOperation()
        {
            OperationHandlerRegister.SetHandler(GameOperations.PlayerStateChanged, new UpdatePlayerStateOperationHandler(sceneObject));
        }

        private void AddHandlerForChangeSceneOperation()
        {
            OperationHandlerRegister.SetHandler(GameOperations.ChangeScene, new ChangeSceneOperationHandler(sceneObject));
        }

        public override void Dispose()
        {
            base.Dispose();

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