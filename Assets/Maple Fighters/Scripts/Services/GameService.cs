using System.Threading.Tasks;
using CommonCommunicationInterfaces;
using CommonTools.Coroutines;
using Scripts.ScriptableObjects;
using Shared.Game.Common;

namespace Scripts.Services
{
    public sealed class GameService : ServiceBase<GameOperations, GameEvents>, IGameService
    {
        public UnityEvent<EnterWorldOperationResponseParameters> EntitiyInitialInformation { get; } = new UnityEvent<EnterWorldOperationResponseParameters>();
        public UnityEvent<GameObjectAddedEventParameters> EntityAdded { get; } = new UnityEvent<GameObjectAddedEventParameters>();
        public UnityEvent<GameObjectRemovedEventParameters> EntityRemoved { get; } = new UnityEvent<GameObjectRemovedEventParameters>();
        public UnityEvent<GameObjectsAddedEventParameters> EntitiesAdded { get; } = new UnityEvent<GameObjectsAddedEventParameters>();
        public UnityEvent<GameObjectsRemovedEventParameters> EntitiesRemoved { get; } = new UnityEvent<GameObjectsRemovedEventParameters>();
        public UnityEvent<GameObjectPositionChangedEventParameters> EntityPositionChanged { get; } = new UnityEvent<GameObjectPositionChangedEventParameters>();
        public UnityEvent<PlayerStateChangedEventParameters> PlayerStateChanged { get; } = new UnityEvent<PlayerStateChangedEventParameters>();
        
        public void Connect()
        {
            var connectionInformation = ServicesConfiguration.GetInstance().GetConnectionInformation(ServersType.Game);
            Connect(connectionInformation);
        }

        public void Disconnect()
        {
            Dispose();
        }

        protected override void OnConnected()
        {
            AddEventsHandlers();

            CoroutinesExecutor.StartTask(EnterWorld);
        }

        protected override void OnDisconnected()
        {
            RemoveEventsHandlers();
        }

        private void AddEventsHandlers()
        {
            EventHandlerRegister.SetHandler(GameEvents.GameObjectAdded, new EventInvoker<GameObjectAddedEventParameters>(unityEvent =>
            {
                EntityAdded?.Invoke(unityEvent.Parameters);
                return true;
            }));

            EventHandlerRegister.SetHandler(GameEvents.GameObjectRemoved, new EventInvoker<GameObjectRemovedEventParameters>(unityEvent =>
            {
                EntityRemoved?.Invoke(unityEvent.Parameters);
                return true;
            }));

            EventHandlerRegister.SetHandler(GameEvents.GameObjectsAdded, new EventInvoker<GameObjectsAddedEventParameters>(unityEvent =>
            {
                EntitiesAdded?.Invoke(unityEvent.Parameters);
                return true;
            }));

            EventHandlerRegister.SetHandler(GameEvents.GameObjectsRemoved, new EventInvoker<GameObjectsRemovedEventParameters>(unityEvent =>
            {
                EntitiesRemoved?.Invoke(unityEvent.Parameters);
                return true;
            }));

            EventHandlerRegister.SetHandler(GameEvents.PositionChanged, new EventInvoker<GameObjectPositionChangedEventParameters>(unityEvent =>
            {
                EntityPositionChanged?.Invoke(unityEvent.Parameters);
                return true;
            }));

            EventHandlerRegister.SetHandler(GameEvents.PlayerStateChanged, new EventInvoker<PlayerStateChangedEventParameters>(unityEvent =>
            {
                PlayerStateChanged?.Invoke(unityEvent.Parameters);
                return true;
            }));
        }

        private void RemoveEventsHandlers()
        {
            EventHandlerRegister.RemoveHandler(GameEvents.GameObjectAdded);
            EventHandlerRegister.RemoveHandler(GameEvents.GameObjectRemoved);
            EventHandlerRegister.RemoveHandler(GameEvents.GameObjectsAdded);
            EventHandlerRegister.RemoveHandler(GameEvents.GameObjectsRemoved);
            EventHandlerRegister.RemoveHandler(GameEvents.PositionChanged);
            EventHandlerRegister.RemoveHandler(GameEvents.PlayerStateChanged);
        }

        public async Task EnterWorld(IYield yield)
        {
            if (!IsServerConnected())
            {
                return;
            }

            var requestId = OperationRequestSender.Send(GameOperations.EnterWorld, new EmptyParameters(), MessageSendOptions.DefaultReliable());
            var responseParameters = await SubscriptionProvider.ProvideSubscription<EnterWorldOperationResponseParameters>(yield, requestId);

            EntitiyInitialInformation.Invoke(responseParameters);
        }

        public void UpdateEntityPosition(UpdateEntityPositionRequestParameters parameters)
        {
            if (!IsServerConnected())
            {
                return;
            }

            OperationRequestSender.Send(GameOperations.PositionChanged, parameters, MessageSendOptions.DefaultUnreliable((byte)GameDataChannels.Position));
        }

        public void UpdatePlayerState(UpdatePlayerStateRequestParameters parameters)
        {
            if (!IsServerConnected())
            {
                return;
            }

            OperationRequestSender.Send(GameOperations.PlayerStateChanged, parameters, MessageSendOptions.DefaultUnreliable((byte)GameDataChannels.Animations));
        }

        public async Task ChangeScene(IYield yield, ChangeSceneRequestParameters parameters)
        {
            if (!IsServerConnected())
            {
                return;
            }

            var requestId = OperationRequestSender.Send(GameOperations.ChangeScene, parameters, MessageSendOptions.DefaultReliable());
            await SubscriptionProvider.ProvideSubscription<EmptyParameters>(yield, requestId);
        }
    }
}