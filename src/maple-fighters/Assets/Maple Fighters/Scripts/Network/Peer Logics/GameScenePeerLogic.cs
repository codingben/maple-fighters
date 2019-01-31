using System.Threading.Tasks;
using CommonCommunicationInterfaces;
using CommonTools.Coroutines;
using Game.Common;

namespace Scripts.Services
{
    public sealed class GameScenePeerLogic : PeerLogicBase, IGameScenePeerLogicAPI
    {
        public UnityEvent<EnterSceneResponseParameters> SceneEntered { get; }

        public UnityEvent<SceneObjectAddedEventParameters> SceneObjectAdded { get; }

        public UnityEvent<SceneObjectRemovedEventParameters> SceneObjectRemoved { get; }

        public UnityEvent<SceneObjectsAddedEventParameters> SceneObjectsAdded { get; }

        public UnityEvent<SceneObjectsRemovedEventParameters> SceneObjectsRemoved { get; }

        public UnityEvent<SceneObjectPositionChangedEventParameters> PositionChanged { get; }

        public UnityEvent<PlayerStateChangedEventParameters> PlayerStateChanged { get; }

        public UnityEvent<PlayerAttackedEventParameters> PlayerAttacked { get; }

        public UnityEvent<CharacterAddedEventParameters> CharacterAdded { get; }

        public UnityEvent<CharactersAddedEventParameters> CharactersAdded { get; }

        public UnityEvent<BubbleMessageEventParameters> BubbleMessageReceived { get; }

        public GameScenePeerLogic()
        {
            BubbleMessageReceived = new UnityEvent<BubbleMessageEventParameters>();
            CharactersAdded = new UnityEvent<CharactersAddedEventParameters>();
            CharacterAdded = new UnityEvent<CharacterAddedEventParameters>();
            PlayerAttacked = new UnityEvent<PlayerAttackedEventParameters>();
            PlayerStateChanged = new UnityEvent<PlayerStateChangedEventParameters>();
            PositionChanged = new UnityEvent<SceneObjectPositionChangedEventParameters>();
            SceneObjectsRemoved = new UnityEvent<SceneObjectsRemovedEventParameters>();
            SceneObjectsAdded = new UnityEvent<SceneObjectsAddedEventParameters>();
            SceneObjectRemoved = new UnityEvent<SceneObjectRemovedEventParameters>();
            SceneObjectAdded = new UnityEvent<SceneObjectAddedEventParameters>();
            SceneEntered = new UnityEvent<EnterSceneResponseParameters>();
        }

        protected override void OnAwake()
        {
            base.OnAwake();

            SetEventsHandlers();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            RemoveEventsHandlers();
        }

        private void SetEventsHandlers()
        {
            ServerPeerHandler
                .SetEventHandler(GameEvents.SceneObjectAdded, SceneObjectAdded);
            ServerPeerHandler
                .SetEventHandler(GameEvents.SceneObjectRemoved, SceneObjectRemoved);
            ServerPeerHandler
                .SetEventHandler(GameEvents.SceneObjectsAdded, SceneObjectsAdded);
            ServerPeerHandler
                .SetEventHandler(GameEvents.SceneObjectsRemoved, SceneObjectsRemoved);
            ServerPeerHandler
                .SetEventHandler(GameEvents.PositionChanged, PositionChanged);
            ServerPeerHandler
                .SetEventHandler(GameEvents.PlayerStateChanged, PlayerStateChanged);
            ServerPeerHandler
                .SetEventHandler(GameEvents.PlayerAttacked, PlayerAttacked);
            ServerPeerHandler
                .SetEventHandler(GameEvents.CharacterAdded, CharacterAdded);
            ServerPeerHandler
                .SetEventHandler(GameEvents.CharactersAdded, CharactersAdded);
            ServerPeerHandler
                .SetEventHandler(GameEvents.BubbleMessage, BubbleMessageReceived);
        }

        private void RemoveEventsHandlers()
        {
            ServerPeerHandler.RemoveEventHandler((byte)GameEvents.SceneObjectAdded);
            ServerPeerHandler.RemoveEventHandler((byte)GameEvents.SceneObjectRemoved);
            ServerPeerHandler.RemoveEventHandler((byte)GameEvents.SceneObjectsAdded);
            ServerPeerHandler.RemoveEventHandler((byte)GameEvents.SceneObjectsRemoved);
            ServerPeerHandler.RemoveEventHandler((byte)GameEvents.PositionChanged);
            ServerPeerHandler.RemoveEventHandler((byte)GameEvents.PlayerStateChanged);
            ServerPeerHandler.RemoveEventHandler((byte)GameEvents.PlayerAttacked);
            ServerPeerHandler.RemoveEventHandler((byte)GameEvents.CharacterAdded);
            ServerPeerHandler.RemoveEventHandler((byte)GameEvents.CharactersAdded);
            ServerPeerHandler.RemoveEventHandler((byte)GameEvents.BubbleMessage);
        }

        public async Task EnterScene(IYield yield)
        {
            var responseParameters =
                await ServerPeerHandler
                    .SendOperation<EmptyParameters, EnterSceneResponseParameters>(
                        yield,
                        GameOperations.EnterScene,
                        new EmptyParameters(),
                        MessageSendOptions.DefaultReliable());

            SceneEntered?.Invoke(responseParameters);
        }

        public async Task<ChangeSceneResponseParameters> ChangeScene(
            IYield yield,
            ChangeSceneRequestParameters parameters)
        {
            return
                await ServerPeerHandler
                    .SendOperation<ChangeSceneRequestParameters, ChangeSceneResponseParameters>(
                        yield,
                        GameOperations.ChangeScene,
                        parameters,
                        MessageSendOptions.DefaultReliable());
        }

        public void UpdatePosition(UpdatePositionRequestParameters parameters)
        {
            ServerPeerHandler.SendOperation(
                GameOperations.PositionChanged,
                parameters,
                MessageSendOptions.DefaultUnreliable((byte)GameDataChannels.Position));
        }

        public void UpdatePlayerState(
            UpdatePlayerStateRequestParameters parameters)
        {
            ServerPeerHandler.SendOperation(
                GameOperations.PlayerStateChanged,
                parameters,
                MessageSendOptions.DefaultUnreliable((byte)GameDataChannels.Animations));
        }
    }
}