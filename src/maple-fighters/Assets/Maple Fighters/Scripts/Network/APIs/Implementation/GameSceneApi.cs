using System.Threading.Tasks;
using CommonCommunicationInterfaces;
using CommonTools.Coroutines;
using Game.Common;

namespace Scripts.Services
{
    public class GameSceneApi : IGameSceneApi
    {
        public ServerPeerHandler<GameOperations, GameEvents> ServerPeer
        {
            get;
        }

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

        public GameSceneApi()
        {
            ServerPeer =
                new ServerPeerHandler<GameOperations, GameEvents>();

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

        private void SetEventHandlers()
        {
            ServerPeer
                .SetEventHandler(GameEvents.SceneObjectAdded, SceneObjectAdded);
            ServerPeer
                .SetEventHandler(GameEvents.SceneObjectRemoved, SceneObjectRemoved);
            ServerPeer
                .SetEventHandler(GameEvents.SceneObjectsAdded, SceneObjectsAdded);
            ServerPeer
                .SetEventHandler(GameEvents.SceneObjectsRemoved, SceneObjectsRemoved);
            ServerPeer
                .SetEventHandler(GameEvents.PositionChanged, PositionChanged);
            ServerPeer
                .SetEventHandler(GameEvents.PlayerStateChanged, PlayerStateChanged);
            ServerPeer
                .SetEventHandler(GameEvents.PlayerAttacked, PlayerAttacked);
            ServerPeer
                .SetEventHandler(GameEvents.CharacterAdded, CharacterAdded);
            ServerPeer
                .SetEventHandler(GameEvents.CharactersAdded, CharactersAdded);
            ServerPeer
                .SetEventHandler(GameEvents.BubbleMessage, BubbleMessageReceived);
        }

        private void RemoveEventHandlers()
        {
            ServerPeer
                .RemoveEventHandler(GameEvents.SceneObjectAdded);
            ServerPeer
                .RemoveEventHandler(GameEvents.SceneObjectRemoved);
            ServerPeer
                .RemoveEventHandler(GameEvents.SceneObjectsAdded);
            ServerPeer
                .RemoveEventHandler(GameEvents.SceneObjectsRemoved);
            ServerPeer
                .RemoveEventHandler(GameEvents.PositionChanged);
            ServerPeer
                .RemoveEventHandler(GameEvents.PlayerStateChanged);
            ServerPeer
                .RemoveEventHandler(GameEvents.PlayerAttacked);
            ServerPeer
                .RemoveEventHandler(GameEvents.CharacterAdded);
            ServerPeer
                .RemoveEventHandler(GameEvents.CharactersAdded);
            ServerPeer
                .RemoveEventHandler(GameEvents.BubbleMessage);
        }

        public async Task EnterSceneAsync(IYield yield)
        {
            var responseParameters =
                await ServerPeer
                    .SendOperation<EmptyParameters, EnterSceneResponseParameters>(
                        yield,
                        GameOperations.EnterScene,
                        new EmptyParameters(),
                        MessageSendOptions.DefaultReliable());

            SceneEntered?.Invoke(responseParameters);
        }

        public async Task<ChangeSceneResponseParameters> ChangeSceneAsync(
            IYield yield,
            ChangeSceneRequestParameters parameters)
        {
            return
                await ServerPeer
                    .SendOperation<ChangeSceneRequestParameters, ChangeSceneResponseParameters>(
                        yield,
                        GameOperations.ChangeScene,
                        parameters,
                        MessageSendOptions.DefaultReliable());
        }

        public Task UpdatePosition(UpdatePositionRequestParameters parameters)
        {
            ServerPeer.SendOperation(
                GameOperations.PositionChanged,
                parameters,
                MessageSendOptions.DefaultUnreliable((byte)GameDataChannels.Position));

            return Task.CompletedTask;
        }

        public Task UpdatePlayerState(
            UpdatePlayerStateRequestParameters parameters)
        {
            ServerPeer.SendOperation(
                GameOperations.PlayerStateChanged,
                parameters,
                MessageSendOptions.DefaultUnreliable((byte)GameDataChannels.Animations));

            return Task.CompletedTask;
        }
    }
}