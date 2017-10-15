using System;
using System.Threading.Tasks;
using CommonCommunicationInterfaces;
using CommonTools.Coroutines;
using Scripts.ScriptableObjects;
using Shared.Game.Common;

namespace Scripts.Services
{
    public sealed class GameService : ServiceBase<GameOperations, GameEvents>, IGameService
    {
        public event Action Connected;

        public UnityEvent<LocalGameObjectAddedEventParameters> LocalGameObjectAdded { get; } = new UnityEvent<LocalGameObjectAddedEventParameters>();
        public UnityEvent<GameObjectAddedEventParameters> GameObjectAdded { get; } = new UnityEvent<GameObjectAddedEventParameters>();
        public UnityEvent<GameObjectRemovedEventParameters> GameObjectRemoved { get; } = new UnityEvent<GameObjectRemovedEventParameters>();
        public UnityEvent<GameObjectsAddedEventParameters> GameObjectsAdded { get; } = new UnityEvent<GameObjectsAddedEventParameters>();
        public UnityEvent<GameObjectsRemovedEventParameters> GameObjectsRemoved { get; } = new UnityEvent<GameObjectsRemovedEventParameters>();
        public UnityEvent<GameObjectPositionChangedEventParameters> PositionChanged { get; } = new UnityEvent<GameObjectPositionChangedEventParameters>();
        public UnityEvent<PlayerStateChangedEventParameters> PlayerStateChanged { get; } = new UnityEvent<PlayerStateChangedEventParameters>();

        public async Task<ConnectionStatus> Connect(IYield yield)
        {
            var connectionInformation = ServicesConfiguration.GetInstance().GetConnectionInformation(ServersType.Game);
            var connectionStatus = await Connect(yield, connectionInformation);
            return connectionStatus;
        }

        public void Disconnect()
        {
            Dispose();
        }

        protected override void OnConnected()
        {
            AddEventsHandlers();

            Connected?.Invoke();
        }

        protected override void OnDisconnected()
        {
            RemoveEventsHandlers();
        }

        private void AddEventsHandlers()
        {
            EventHandlerRegister.SetHandler(GameEvents.LocalGameObjectAdded, new EventInvoker<LocalGameObjectAddedEventParameters>(unityEvent =>
            {
                LocalGameObjectAdded?.Invoke(unityEvent.Parameters);
                return true;
            }));

            EventHandlerRegister.SetHandler(GameEvents.GameObjectAdded, new EventInvoker<GameObjectAddedEventParameters>(unityEvent =>
            {
                GameObjectAdded?.Invoke(unityEvent.Parameters);
                return true;
            }));

            EventHandlerRegister.SetHandler(GameEvents.GameObjectRemoved, new EventInvoker<GameObjectRemovedEventParameters>(unityEvent =>
            {
                GameObjectRemoved?.Invoke(unityEvent.Parameters);
                return true;
            }));

            EventHandlerRegister.SetHandler(GameEvents.GameObjectsAdded, new EventInvoker<GameObjectsAddedEventParameters>(unityEvent =>
            {
                GameObjectsAdded?.Invoke(unityEvent.Parameters);
                return true;
            }));

            EventHandlerRegister.SetHandler(GameEvents.GameObjectsRemoved, new EventInvoker<GameObjectsRemovedEventParameters>(unityEvent =>
            {
                GameObjectsRemoved?.Invoke(unityEvent.Parameters);
                return true;
            }));

            EventHandlerRegister.SetHandler(GameEvents.PositionChanged, new EventInvoker<GameObjectPositionChangedEventParameters>(unityEvent =>
            {
                PositionChanged?.Invoke(unityEvent.Parameters);
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
            EventHandlerRegister.RemoveHandler(GameEvents.LocalGameObjectAdded);
            EventHandlerRegister.RemoveHandler(GameEvents.GameObjectAdded);
            EventHandlerRegister.RemoveHandler(GameEvents.GameObjectRemoved);
            EventHandlerRegister.RemoveHandler(GameEvents.GameObjectsAdded);
            EventHandlerRegister.RemoveHandler(GameEvents.GameObjectsRemoved);
            EventHandlerRegister.RemoveHandler(GameEvents.PositionChanged);
            EventHandlerRegister.RemoveHandler(GameEvents.PlayerStateChanged);
        }

        public async Task<AuthenticationStatus> Authenticate(IYield yield)
        {
            if (!IsServerConnected())
            {
                return AuthenticationStatus.Failed;
            }

            var parameters = new AuthenticateRequestParameters(AccessTokenProvider.AccessToken);
            var requestId = OperationRequestSender.Send(GameOperations.Authenticate, parameters, MessageSendOptions.DefaultReliable());
            var responseParameters = await SubscriptionProvider.ProvideSubscription<AuthenticateResponseParameters>(yield, requestId);
            return responseParameters.Status;
        }

        public void EnterWorld()
        {
            if (!IsServerConnected())
            {
                return;
            }

            OperationRequestSender.Send(GameOperations.EnterWorld, new EmptyParameters(), MessageSendOptions.DefaultReliable());
        }

        public async Task<FetchCharactersResponseParameters> FetchCharacters(IYield yield)
        {
            if (!IsServerConnected())
            {
                return new FetchCharactersResponseParameters(new Character[0]);
            }

            var requestId = OperationRequestSender.Send(GameOperations.FetchCharacters, new EmptyParameters(), MessageSendOptions.DefaultReliable());
            var responseParameters = await SubscriptionProvider.ProvideSubscription<FetchCharactersResponseParameters>(yield, requestId);
            return responseParameters;
        }

        public async Task<ValidateCharacterStatus> ValidateCharacter(IYield yield, ValidateCharacterRequestParameters parameters)
        {
            if (!IsServerConnected())
            {
                return ValidateCharacterStatus.Wrong;
            }

            var requestId = OperationRequestSender.Send(GameOperations.ValidateCharacter, parameters, MessageSendOptions.DefaultReliable());
            var responseParameters = await SubscriptionProvider.ProvideSubscription<ValidateCharacterResponseParameters>(yield, requestId);
            return responseParameters.Status;
        }

        public async Task<CreateCharacterResponseParameters> CreateCharacter(IYield yield, CreateCharacterRequestParameters parameters)
        {
            if (!IsServerConnected())
            {
                return new CreateCharacterResponseParameters(CharacterCreationStatus.Failed);
            }

            var requestId = OperationRequestSender.Send(GameOperations.CreateCharacter, parameters, MessageSendOptions.DefaultReliable());
            var responseParameters = await SubscriptionProvider.ProvideSubscription<CreateCharacterResponseParameters>(yield, requestId);
            return responseParameters;
        }

        public async Task<RemoveCharacterResponseParameters> RemoveCharacter(IYield yield, RemoveCharacterRequestParameters parameters)
        {
            if (!IsServerConnected())
            {
                return new RemoveCharacterResponseParameters(RemoveCharacterStatus.Failed);
            }

            var requestId = OperationRequestSender.Send(GameOperations.RemoveCharacter, parameters, MessageSendOptions.DefaultReliable());
            var responseParameters = await SubscriptionProvider.ProvideSubscription<RemoveCharacterResponseParameters>(yield, requestId);
            return responseParameters;
        }

        public void UpdatePosition(UpdatePositionRequestParameters parameters)
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