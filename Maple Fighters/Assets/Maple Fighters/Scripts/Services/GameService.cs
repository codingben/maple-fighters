using System.Threading.Tasks;
using CommonCommunicationInterfaces;
using CommonTools.Coroutines;
using CommonTools.Log;
using Scripts.ScriptableObjects;
using Shared.Game.Common;

namespace Scripts.Services
{
    public sealed class GameService : ServiceBase<GameOperations, GameEvents>, IGameService
    {
        public UnityEvent<EnterWorldOperationResponseParameters> EntitiyInitialInformation { get; } = new UnityEvent<EnterWorldOperationResponseParameters>();
        public UnityEvent<EntityAddedEventParameters> EntityAdded { get; } = new UnityEvent<EntityAddedEventParameters>();
        public UnityEvent<EntityRemovedEventParameters> EntityRemoved { get; } = new UnityEvent<EntityRemovedEventParameters>();
        public UnityEvent<EntitiesAddedEventParameters> EntitiesAdded { get; } = new UnityEvent<EntitiesAddedEventParameters>();
        public UnityEvent<EntitiesRemovedEventParameters> EntitiesRemoved { get; } = new UnityEvent<EntitiesRemovedEventParameters>();
        public UnityEvent<EntityPositionChangedEventParameters> EntityPositionChanged { get; } = new UnityEvent<EntityPositionChangedEventParameters>();
        public UnityEvent<PlayerStateChangedEventParameters> PlayerStateChanged { get; } = new UnityEvent<PlayerStateChangedEventParameters>();
        
        public void Connect()
        {
            var gameConnectionInformation = ServicesConfiguration.GetInstance().GameConnectionInformation;
            Connect(gameConnectionInformation);
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
            EventHandlerRegister.SetHandler(GameEvents.EntityAdded, new EventInvoker<EntityAddedEventParameters>(unityEvent =>
            {
                EntityAdded?.Invoke(unityEvent.Parameters);
                return true;
            }));

            EventHandlerRegister.SetHandler(GameEvents.EntityRemoved, new EventInvoker<EntityRemovedEventParameters>(unityEvent =>
            {
                EntityRemoved?.Invoke(unityEvent.Parameters);
                return true;
            }));

            EventHandlerRegister.SetHandler(GameEvents.EntitiesAdded, new EventInvoker<EntitiesAddedEventParameters>(unityEvent =>
            {
                EntitiesAdded?.Invoke(unityEvent.Parameters);
                return true;
            }));

            EventHandlerRegister.SetHandler(GameEvents.EntitiesRemoved, new EventInvoker<EntitiesRemovedEventParameters>(unityEvent =>
            {
                EntitiesRemoved?.Invoke(unityEvent.Parameters);
                return true;
            }));

            EventHandlerRegister.SetHandler(GameEvents.EntityPositionChanged, new EventInvoker<EntityPositionChangedEventParameters>(unityEvent =>
            {
                EntityPositionChanged?.Invoke(unityEvent.Parameters);
                return true;
            }));

            EventHandlerRegister.SetHandler(GameEvents.EntityStateChanged, new EventInvoker<PlayerStateChangedEventParameters>(unityEvent =>
            {
                PlayerStateChanged?.Invoke(unityEvent.Parameters);
                return true;
            }));
        }

        private void RemoveEventsHandlers()
        {
            EventHandlerRegister.RemoveHandler(GameEvents.EntityAdded);
            EventHandlerRegister.RemoveHandler(GameEvents.EntityRemoved);
            EventHandlerRegister.RemoveHandler(GameEvents.EntitiesAdded);
            EventHandlerRegister.RemoveHandler(GameEvents.EntitiesRemoved);
            EventHandlerRegister.RemoveHandler(GameEvents.EntityPositionChanged);
            EventHandlerRegister.RemoveHandler(GameEvents.EntityStateChanged);
        }

        public async Task EnterWorld(IYield yield)
        {
            if (!IsConnected())
            {
                LogUtils.Log(MessageBuilder.Trace("Peer is not connected to a server."));
                return;
            }

            var requestId = OperationRequestSender.Send(GameOperations.EnterWorld, new EmptyParameters(), MessageSendOptions.DefaultReliable());
            var response = await SubscriptionProvider.ProvideSubscription<EnterWorldOperationResponseParameters>(yield, requestId);

            EntitiyInitialInformation.Invoke(response);
        }

        public void UpdateEntityPosition(UpdateEntityPositionRequestParameters parameters)
        {
            if (!IsConnected())
            {
                LogUtils.Log(MessageBuilder.Trace("Peer is not connected to a server."));
                return;
            }

            OperationRequestSender.Send(GameOperations.PositionChanged, parameters, MessageSendOptions.DefaultUnreliable((byte)GameDataChannels.Position));
        }

        public void UpdatePlayerState(UpdatePlayerStateRequestParameters parameters)
        {
            if (!IsConnected())
            {
                LogUtils.Log(MessageBuilder.Trace("Peer is not connected to a server."));
                return;
            }

            OperationRequestSender.Send(GameOperations.PlayerStateChanged, parameters, MessageSendOptions.DefaultReliable());
        }

        public async Task ChangeScene(IYield yield, ChangeSceneRequestParameters parameters)
        {
            if (!IsConnected())
            {
                LogUtils.Log(MessageBuilder.Trace("Peer is not connected to a server."));
                return;
            }

            var requestId = OperationRequestSender.Send(GameOperations.ChangeScene, parameters, MessageSendOptions.DefaultReliable());
            await SubscriptionProvider.ProvideSubscription<EmptyParameters>(yield, requestId);
        }
    }
}