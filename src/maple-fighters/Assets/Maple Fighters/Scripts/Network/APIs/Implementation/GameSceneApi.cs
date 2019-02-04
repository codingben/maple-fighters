using System.Threading.Tasks;
using CommonCommunicationInterfaces;
using CommonTools.Coroutines;
using Game.Common;

namespace Scripts.Services
{
    public class GameSceneApi : ApiBase<GameOperations, GameEvents>, IGameSceneApi
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

        public GameSceneApi()
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

        private void SetEventHandlers()
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

        private void RemoveEventHandlers()
        {
            ServerPeerHandler
                .RemoveEventHandler(GameEvents.SceneObjectAdded);
            ServerPeerHandler
                .RemoveEventHandler(GameEvents.SceneObjectRemoved);
            ServerPeerHandler
                .RemoveEventHandler(GameEvents.SceneObjectsAdded);
            ServerPeerHandler
                .RemoveEventHandler(GameEvents.SceneObjectsRemoved);
            ServerPeerHandler
                .RemoveEventHandler(GameEvents.PositionChanged);
            ServerPeerHandler
                .RemoveEventHandler(GameEvents.PlayerStateChanged);
            ServerPeerHandler
                .RemoveEventHandler(GameEvents.PlayerAttacked);
            ServerPeerHandler
                .RemoveEventHandler(GameEvents.CharacterAdded);
            ServerPeerHandler
                .RemoveEventHandler(GameEvents.CharactersAdded);
            ServerPeerHandler
                .RemoveEventHandler(GameEvents.BubbleMessage);
        }

        public async Task EnterSceneAsync(IYield yield)
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

        public async Task<ChangeSceneResponseParameters> ChangeSceneAsync(
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

        public Task UpdatePosition(UpdatePositionRequestParameters parameters)
        {
            ServerPeerHandler.SendOperation(
                GameOperations.PositionChanged,
                parameters,
                MessageSendOptions.DefaultUnreliable((byte)GameDataChannels.Position));

            return Task.CompletedTask;
        }

        public Task UpdatePlayerState(
            UpdatePlayerStateRequestParameters parameters)
        {
            ServerPeerHandler.SendOperation(
                GameOperations.PlayerStateChanged,
                parameters,
                MessageSendOptions.DefaultUnreliable((byte)GameDataChannels.Animations));

            return Task.CompletedTask;
        }
    }
}