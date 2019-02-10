using System.Threading.Tasks;
using CommonCommunicationInterfaces;
using CommonTools.Coroutines;
using CommunicationHelper;
using Game.Common;
using Scripts.Network.Core;

namespace Scripts.Network.APIs
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
            EventHandlerRegister
                .SetHandler(GameEvents.SceneObjectAdded, SceneObjectAdded.ToHandler());
            EventHandlerRegister
                .SetHandler(GameEvents.SceneObjectRemoved, SceneObjectRemoved.ToHandler());
            EventHandlerRegister
                .SetHandler(GameEvents.SceneObjectsAdded, SceneObjectsAdded.ToHandler());
            EventHandlerRegister
                .SetHandler(GameEvents.SceneObjectsRemoved, SceneObjectsRemoved.ToHandler());
            EventHandlerRegister
                .SetHandler(GameEvents.PositionChanged, PositionChanged.ToHandler());
            EventHandlerRegister
                .SetHandler(GameEvents.PlayerStateChanged, PlayerStateChanged.ToHandler());
            EventHandlerRegister
                .SetHandler(GameEvents.PlayerAttacked, PlayerAttacked.ToHandler());
            EventHandlerRegister
                .SetHandler(GameEvents.CharacterAdded, CharacterAdded.ToHandler());
            EventHandlerRegister
                .SetHandler(GameEvents.CharactersAdded, CharactersAdded.ToHandler());
            EventHandlerRegister
                .SetHandler(GameEvents.BubbleMessage, BubbleMessageReceived.ToHandler());
        }

        private void RemoveEventHandlers()
        {
            EventHandlerRegister.RemoveHandler(GameEvents.SceneObjectAdded);
            EventHandlerRegister.RemoveHandler(GameEvents.SceneObjectRemoved);
            EventHandlerRegister.RemoveHandler(GameEvents.SceneObjectsAdded);
            EventHandlerRegister.RemoveHandler(GameEvents.SceneObjectsRemoved);
            EventHandlerRegister.RemoveHandler(GameEvents.PositionChanged);
            EventHandlerRegister.RemoveHandler(GameEvents.PlayerStateChanged);
            EventHandlerRegister.RemoveHandler(GameEvents.PlayerAttacked);
            EventHandlerRegister.RemoveHandler(GameEvents.CharacterAdded);
            EventHandlerRegister.RemoveHandler(GameEvents.CharactersAdded);
            EventHandlerRegister.RemoveHandler(GameEvents.BubbleMessage);
        }

        public async Task EnterSceneAsync(IYield yield)
        {
            var id =
                OperationRequestSender.Send(
                    GameOperations.EnterScene,
                    new EmptyParameters(),
                    MessageSendOptions.DefaultReliable());

            var responseParameters =
                await SubscriptionProvider
                    .ProvideSubscription<EnterSceneResponseParameters>(
                        yield,
                        id);

            SceneEntered?.Invoke(responseParameters);
        }

        public async Task<ChangeSceneResponseParameters> ChangeSceneAsync(
            IYield yield,
            ChangeSceneRequestParameters parameters)
        {
            var id =
                OperationRequestSender.Send(
                    GameOperations.ChangeScene,
                    parameters,
                    MessageSendOptions.DefaultReliable());

            return 
                await SubscriptionProvider
                    .ProvideSubscription<ChangeSceneResponseParameters>(
                        yield,
                        id);
        }

        public Task UpdatePosition(UpdatePositionRequestParameters parameters)
        {
            OperationRequestSender.Send(
                GameOperations.PositionChanged,
                parameters,
                MessageSendOptions.DefaultUnreliable((byte)GameDataChannels.Position));

            return Task.CompletedTask;
        }

        public Task UpdatePlayerState(
            UpdatePlayerStateRequestParameters parameters)
        {
            OperationRequestSender.Send(
                GameOperations.PlayerStateChanged,
                parameters,
                MessageSendOptions.DefaultUnreliable((byte)GameDataChannels.Animations));

            return Task.CompletedTask;
        }
    }
}