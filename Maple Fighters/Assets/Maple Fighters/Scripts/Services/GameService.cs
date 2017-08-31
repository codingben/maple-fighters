using CommonCommunicationInterfaces;
using CommonTools.Log;
using Scripts.ScriptableObjects;
using Shared.Game.Common;

namespace Scripts.Services
{
    public sealed class GameService : ServiceBase<GameOperations, GameEvents>, IGameService
    {
        public UnityEvent<EntityInitialInfomraitonEventParameters> EntitiyInitialInformation { get; }
        public UnityEvent<EntityAddedEventParameters> EntityAdded { get; }
        public UnityEvent<EntityRemovedEventParameters> EntityRemoved { get; }
        public UnityEvent<EntityPositionChangedEventParameters> EntityPositionChanged { get; }

        public GameService()
        {
            EntityAdded = new UnityEvent<EntityAddedEventParameters>();
            EntityRemoved = new UnityEvent<EntityRemovedEventParameters>();
            EntitiyInitialInformation = new UnityEvent<EntityInitialInfomraitonEventParameters>();
            EntityPositionChanged = new UnityEvent<EntityPositionChangedEventParameters>();
        }

        public void Connect()
        {
            var gameConnectionInformation = ServicesConfiguration.GetInstance().GameConnectionInformation;
            Connect(gameConnectionInformation);
        }

        protected override void OnConnected()
        {
            AddEventsHandlers();
            EnterWorld();
        }

        protected override void OnDisconnected()
        {
            RemoveEventsHandlers();
        }

        private void AddEventsHandlers()
        {
            EventHandlerRegister.SetHandler(GameEvents.EntityInitialInformation, new EventInvoker<EntityInitialInfomraitonEventParameters>(unityEvent =>
            {
                EntitiyInitialInformation.Invoke(unityEvent.Parameters);
                return true;
            }));

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

        public void EnterWorld()
        {
            if (!IsConnected())
            {
                LogUtils.Log(MessageBuilder.Trace("Peer is not connected to a server."));
                return;
            }

            OperationRequestSender.Send(GameOperations.EnterWorld, new EmptyParameters(), MessageSendOptions.DefaultReliable());
        }
    }
}