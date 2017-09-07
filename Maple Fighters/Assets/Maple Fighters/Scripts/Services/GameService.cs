using System.Threading.Tasks;
using CommonCommunicationInterfaces;
using CommonTools.Coroutines;
using CommonTools.Log;
using CommunicationHelper;
using Scripts.ScriptableObjects;
using Shared.Game.Common;

namespace Scripts.Services
{
    public sealed class GameService : ServiceBase<GameOperations, GameEvents>, IGameService
    {
        public UnityEvent<EnterWorldOperationResponseParameters> EntitiyInitialInformation { get; }
        public UnityEvent<EntityAddedEventParameters> EntityAdded { get; }
        public UnityEvent<EntityRemovedEventParameters> EntityRemoved { get; }
        public UnityEvent<EntitiesAddedEventParameters> EntitiesAdded { get; }
        public UnityEvent<EntitiesRemovedEventParameters> EntitiesRemoved { get; }
        public UnityEvent<EntityPositionChangedEventParameters> EntityPositionChanged { get; }

        public GameService()
        {
            EntitiyInitialInformation = new UnityEvent<EnterWorldOperationResponseParameters>();
            EntityPositionChanged = new UnityEvent<EntityPositionChangedEventParameters>();
            EntityAdded = new UnityEvent<EntityAddedEventParameters>();
            EntityRemoved = new UnityEvent<EntityRemovedEventParameters>();
            EntitiesAdded = new UnityEvent<EntitiesAddedEventParameters>();
            EntitiesRemoved = new UnityEvent<EntitiesRemovedEventParameters>();
        }

        public void Connect()
        {
            var gameConnectionInformation = ServicesConfiguration.GetInstance().GameConnectionInformation;
            Connect(gameConnectionInformation);
        }

        protected override void OnConnected()
        {
            AddEventsHandlers();

            CoroutinesExecuter.StartTask(EnterWorld);
        }

        protected override void OnDisconnected()
        {
            RemoveEventsHandlers();
        }

        private void AddEventsHandlers()
        {
            EventHandlerRegister.SetHandler(GameEvents.EntityAdded, new EventInvoker<EntityAddedEventParameters>(unityEvent =>
            {
                EntityAdded.Invoke(unityEvent.Parameters);
                return true;
            }));

            EventHandlerRegister.SetHandler(GameEvents.EntityRemoved, new EventInvoker<EntityRemovedEventParameters>(unityEvent =>
            {
                EntityRemoved.Invoke(unityEvent.Parameters);
                return true;
            }));

            EventHandlerRegister.SetHandler(GameEvents.EntitiesAdded, new EventInvoker<EntitiesAddedEventParameters>(unityEvent =>
            {
                EntitiesAdded.Invoke(unityEvent.Parameters);
                return true;
            }));

            EventHandlerRegister.SetHandler(GameEvents.EntitiesRemoved, new EventInvoker<EntitiesRemovedEventParameters>(unityEvent =>
            {
                EntitiesRemoved.Invoke(unityEvent.Parameters);
                return true;
            }));

            EventHandlerRegister.SetHandler(GameEvents.EntityPositionChanged, new EventInvoker<EntityPositionChangedEventParameters>(unityEvent =>
            {
                EntityPositionChanged.Invoke(unityEvent.Parameters);
                return true;
            }));
        }

        private void RemoveEventsHandlers()
        {
            EventHandlerRegister.RemoveHandler(GameEvents.EntityInitialInformation);
            EventHandlerRegister.RemoveHandler(GameEvents.EntityAdded);
            EventHandlerRegister.RemoveHandler(GameEvents.EntityRemoved);
            EventHandlerRegister.RemoveHandler(GameEvents.EntityPositionChanged);
        }

        public void UpdateEntityPosition(UpdateEntityPositionRequestParameters parameters)
        {
            if (!IsConnected())
            {
                LogUtils.Log(MessageBuilder.Trace("Peer is not connected to a server."));
                return;
            }

            OperationRequestSender.Send(GameOperations.UpdateEntityPosition, parameters, 
                MessageSendOptions.DefaultUnreliable((byte)GameDataChannels.Position));
        }

        public async Task EnterWorld(IYield yield)
        {
            var requestId = OperationRequestSender.Send(GameOperations.EnterWorld, new EmptyParameters(), MessageSendOptions.DefaultReliable());
            var response = await SubscriptionProvider.ProvideSubscription<EnterWorldOperationResponseParameters>(yield, requestId);

            EntitiyInitialInformation.Invoke(response);
        }
    }

    public static class ExtensionMethods
    {
        public static async Task<TParam> ProvideSubscription<TParam>(this IOperationResponseSubscriptionProvider subscriptionProvider, IYield yield, short requestId)
            where TParam : struct, IParameters
        {
            var receiver = new SafeOperationResponseReceiver<TParam>(Configuration.OperationTimeout);
            subscriptionProvider.ProvideSubscription(receiver, requestId);

            await yield.Return(receiver);

            if (receiver.HasException)
            {
                throw new OperationNotHandledException(receiver.ExceptionReturnCode);
            }

            return receiver.Value;
        }

        public static async Task ProvideSubscription(this IOperationResponseSubscriptionProvider subscriptionProvider, IYield yield, short requestId)
        {
            var receiver = new SafeOperationResponseReceiver<EmptyParameters>(Configuration.OperationTimeout);
            subscriptionProvider.ProvideSubscription(receiver, requestId);

            await yield.Return(receiver);

            if (receiver.HasException)
            {
                throw new OperationNotHandledException(receiver.ExceptionReturnCode);
            }
        }
    }
}