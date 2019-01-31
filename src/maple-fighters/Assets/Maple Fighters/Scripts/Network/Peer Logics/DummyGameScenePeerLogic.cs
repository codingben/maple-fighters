using System.Threading.Tasks;
using CommonCommunicationInterfaces;
using CommonTools.Coroutines;
using Game.Common;

namespace Scripts.Services
{
    public sealed class DummyGameScenePeerLogic : PeerLogicBase, IGameScenePeerLogicAPI, IDummyGameScenePeerLogicAPI
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

        public DummyGameScenePeerLogic()
        {
            SceneEntered = new UnityEvent<EnterSceneResponseParameters>();
            SceneObjectAdded = new UnityEvent<SceneObjectAddedEventParameters>();
            SceneObjectRemoved = new UnityEvent<SceneObjectRemovedEventParameters>();
            SceneObjectsAdded = new UnityEvent<SceneObjectsAddedEventParameters>();
            SceneObjectsRemoved = new UnityEvent<SceneObjectsRemovedEventParameters>();
            PositionChanged = new UnityEvent<SceneObjectPositionChangedEventParameters>();
            PlayerStateChanged = new UnityEvent<PlayerStateChangedEventParameters>();
            PlayerAttacked = new UnityEvent<PlayerAttackedEventParameters>();
            CharacterAdded = new UnityEvent<CharacterAddedEventParameters>();
            CharactersAdded = new UnityEvent<CharactersAddedEventParameters>();
            BubbleMessageReceived = new UnityEvent<BubbleMessageEventParameters>();
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
            ServerPeerHandler.RemoveEventHandler(GameEvents.SceneObjectAdded);
            ServerPeerHandler.RemoveEventHandler(GameEvents.SceneObjectRemoved);
            ServerPeerHandler.RemoveEventHandler(GameEvents.SceneObjectsAdded);
            ServerPeerHandler.RemoveEventHandler(GameEvents.SceneObjectsRemoved);
            ServerPeerHandler.RemoveEventHandler(GameEvents.PositionChanged);
            ServerPeerHandler.RemoveEventHandler(GameEvents.PlayerStateChanged);
            ServerPeerHandler.RemoveEventHandler(GameEvents.PlayerAttacked);
            ServerPeerHandler.RemoveEventHandler(GameEvents.CharacterAdded);
            ServerPeerHandler.RemoveEventHandler(GameEvents.CharactersAdded);
            ServerPeerHandler.RemoveEventHandler(GameEvents.BubbleMessage);
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

        public Task<ChangeSceneResponseParameters> ChangeScene(
            IYield yield,
            ChangeSceneRequestParameters parameters)
        {
            var id = parameters.PortalId;
            var map = DummyPortalContainer.GetInstance().GetMap(id);

            return Task.FromResult(new ChangeSceneResponseParameters(map));
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