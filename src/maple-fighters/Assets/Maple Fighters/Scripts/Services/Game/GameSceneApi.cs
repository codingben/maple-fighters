using System.Threading.Tasks;
using CommonCommunicationInterfaces;
using CommonTools.Coroutines;
using CommunicationHelper;
using Game.Common;
using Network.Scripts;

namespace Scripts.Services.Game
{
    internal class GameSceneApi : NetworkApi<GameOperations, GameEvents>, IGameSceneApi
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

        internal GameSceneApi(IServerPeer serverPeer, bool log = false)
            : base(serverPeer, log)
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

            EventHandlerRegister.SetHandler(GameEvents.SceneObjectAdded, SceneObjectAdded.ToEventHandler());
            EventHandlerRegister.SetHandler(GameEvents.SceneObjectRemoved, SceneObjectRemoved.ToEventHandler());
            EventHandlerRegister.SetHandler(GameEvents.SceneObjectsAdded, SceneObjectsAdded.ToEventHandler());
            EventHandlerRegister.SetHandler(GameEvents.SceneObjectsRemoved, SceneObjectsRemoved.ToEventHandler());
            EventHandlerRegister.SetHandler(GameEvents.PositionChanged, PositionChanged.ToEventHandler());
            EventHandlerRegister.SetHandler(GameEvents.PlayerStateChanged, PlayerStateChanged.ToEventHandler());
            EventHandlerRegister.SetHandler(GameEvents.PlayerAttacked, PlayerAttacked.ToEventHandler());
            EventHandlerRegister.SetHandler(GameEvents.CharacterAdded, CharacterAdded.ToEventHandler());
            EventHandlerRegister.SetHandler(GameEvents.CharactersAdded, CharactersAdded.ToEventHandler());
            EventHandlerRegister.SetHandler(GameEvents.BubbleMessage, BubbleMessageReceived.ToEventHandler());
        }

        public override void Dispose()
        {
            base.Dispose();

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

        public void UpdatePosition(UpdatePositionRequestParameters parameters)
        {
            OperationRequestSender.Send(
                GameOperations.PositionChanged,
                parameters,
                MessageSendOptions.DefaultUnreliable((byte)GameDataChannels.Position));
        }

        public void UpdatePlayerState(UpdatePlayerStateRequestParameters parameters)
        {
            OperationRequestSender.Send(
                GameOperations.PlayerStateChanged,
                parameters,
                MessageSendOptions.DefaultUnreliable((byte)GameDataChannels.Animations));
        }
    }
}