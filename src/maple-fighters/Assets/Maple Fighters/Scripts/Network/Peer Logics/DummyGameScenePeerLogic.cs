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
                .SetEventHandler((byte)GameEvents.SceneObjectAdded, SceneObjectAdded);
            ServerPeerHandler
                .SetEventHandler((byte)GameEvents.SceneObjectRemoved, SceneObjectRemoved);
            ServerPeerHandler
                .SetEventHandler((byte)GameEvents.SceneObjectsAdded, SceneObjectsAdded);
            ServerPeerHandler
                .SetEventHandler((byte)GameEvents.SceneObjectsRemoved, SceneObjectsRemoved);
            ServerPeerHandler
                .SetEventHandler((byte)GameEvents.PositionChanged, PositionChanged);
            ServerPeerHandler
                .SetEventHandler((byte)GameEvents.PlayerStateChanged, PlayerStateChanged);
            ServerPeerHandler
                .SetEventHandler((byte)GameEvents.PlayerAttacked, PlayerAttacked);
            ServerPeerHandler
                .SetEventHandler((byte)GameEvents.CharacterAdded, CharacterAdded);
            ServerPeerHandler
                .SetEventHandler((byte)GameEvents.CharactersAdded, CharactersAdded);
            ServerPeerHandler
                .SetEventHandler((byte)GameEvents.BubbleMessage, BubbleMessageReceived);
        }

        private void RemoveEventsHandlers()
        {
            ServerPeerHandler
                .RemoveEventHandler((byte)GameEvents.SceneObjectAdded);
            ServerPeerHandler
                .RemoveEventHandler((byte)GameEvents.SceneObjectRemoved);
            ServerPeerHandler
                .RemoveEventHandler((byte)GameEvents.SceneObjectsAdded);
            ServerPeerHandler
                .RemoveEventHandler((byte)GameEvents.SceneObjectsRemoved);
            ServerPeerHandler
                .RemoveEventHandler((byte)GameEvents.PositionChanged);
            ServerPeerHandler
                .RemoveEventHandler((byte)GameEvents.PlayerStateChanged);
            ServerPeerHandler
                .RemoveEventHandler((byte)GameEvents.PlayerAttacked);
            ServerPeerHandler
                .RemoveEventHandler((byte)GameEvents.CharacterAdded);
            ServerPeerHandler
                .RemoveEventHandler((byte)GameEvents.CharactersAdded);
            ServerPeerHandler
                .RemoveEventHandler((byte)GameEvents.BubbleMessage);
        }

        public async Task EnterScene(IYield yield)
        {
            var responseParameters =
                await ServerPeerHandler
                    .SendOperation<EmptyParameters, EnterSceneResponseParameters>(
                        yield,
                        (byte)GameOperations.EnterScene,
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
                (byte)GameOperations.PositionChanged,
                parameters,
                MessageSendOptions.DefaultUnreliable((byte)GameDataChannels.Position));
        }

        public void UpdatePlayerState(
            UpdatePlayerStateRequestParameters parameters)
        {
            ServerPeerHandler.SendOperation(
                (byte)GameOperations.PlayerStateChanged,
                parameters,
                MessageSendOptions.DefaultUnreliable((byte)GameDataChannels.Animations));
        }
    }
}