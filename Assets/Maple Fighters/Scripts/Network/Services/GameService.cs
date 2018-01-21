using System.Threading.Tasks;
using CommonCommunicationInterfaces;
using CommonTools.Coroutines;
using Scripts.Utils;
using Shared.Game.Common;

namespace Scripts.Services
{
    public sealed class GameService : ServiceBase<GameOperations, GameEvents>, IGameService
    {
        public UnityEvent<EnterSceneResponseParameters> EnteredScene { get; } = new UnityEvent<EnterSceneResponseParameters>();
        public UnityEvent<SceneObjectAddedEventParameters> SceneObjectAdded { get; } = new UnityEvent<SceneObjectAddedEventParameters>();
        public UnityEvent<SceneObjectRemovedEventParameters> SceneObjectRemoved { get; } = new UnityEvent<SceneObjectRemovedEventParameters>();
        public UnityEvent<SceneObjectsAddedEventParameters> SceneObjectsAdded { get; } = new UnityEvent<SceneObjectsAddedEventParameters>();
        public UnityEvent<SceneObjectsRemovedEventParameters> SceneObjectsRemoved { get; } = new UnityEvent<SceneObjectsRemovedEventParameters>();
        public UnityEvent<SceneObjectPositionChangedEventParameters> PositionChanged { get; } = new UnityEvent<SceneObjectPositionChangedEventParameters>();
        public UnityEvent<PlayerStateChangedEventParameters> PlayerStateChanged { get; } = new UnityEvent<PlayerStateChangedEventParameters>();
        public UnityEvent<PlayerAttackedEventParameters> PlayerAttacked { get; } = new UnityEvent<PlayerAttackedEventParameters>();
        public UnityEvent<CharacterAddedEventParameters> CharacterAdded { get; } = new UnityEvent<CharacterAddedEventParameters>();
        public UnityEvent<CharactersAddedEventParameters> CharactersAdded { get; } = new UnityEvent<CharactersAddedEventParameters>();

        private AuthenticationStatus authenticationStatus;

        protected override void OnConnected()
        {
            AddEventsHandlers();
        }

        protected override void OnDisconnected()
        {
            RemoveEventsHandlers();

            if (authenticationStatus == AuthenticationStatus.Succeed)
            {
                SavedObjects.DestroyAll();
            }
        }

        private void AddEventsHandlers()
        {
            EventHandlerRegister.SetHandler(GameEvents.SceneObjectAdded, new EventInvoker<SceneObjectAddedEventParameters>(unityEvent =>
            {
                SceneObjectAdded?.Invoke(unityEvent.Parameters);
                return true;
            }));

            EventHandlerRegister.SetHandler(GameEvents.SceneObjectRemoved, new EventInvoker<SceneObjectRemovedEventParameters>(unityEvent =>
            {
                SceneObjectRemoved?.Invoke(unityEvent.Parameters);
                return true;
            }));

            EventHandlerRegister.SetHandler(GameEvents.SceneObjectsAdded, new EventInvoker<SceneObjectsAddedEventParameters>(unityEvent =>
            {
                SceneObjectsAdded?.Invoke(unityEvent.Parameters);
                return true;
            }));

            EventHandlerRegister.SetHandler(GameEvents.SceneObjectsRemoved, new EventInvoker<SceneObjectsRemovedEventParameters>(unityEvent =>
            {
                SceneObjectsRemoved?.Invoke(unityEvent.Parameters);
                return true;
            }));

            EventHandlerRegister.SetHandler(GameEvents.PositionChanged, new EventInvoker<SceneObjectPositionChangedEventParameters>(unityEvent =>
            {
                PositionChanged?.Invoke(unityEvent.Parameters);
                return true;
            }));

            EventHandlerRegister.SetHandler(GameEvents.PlayerStateChanged, new EventInvoker<PlayerStateChangedEventParameters>(unityEvent =>
            {
                PlayerStateChanged?.Invoke(unityEvent.Parameters);
                return true;
            }));

            EventHandlerRegister.SetHandler(GameEvents.PlayerAttacked, new EventInvoker<PlayerAttackedEventParameters>(unityEvent =>
            {
                PlayerAttacked?.Invoke(unityEvent.Parameters);
                return true;
            }));

            EventHandlerRegister.SetHandler(GameEvents.CharacterAdded, new EventInvoker<CharacterAddedEventParameters>(unityEvent =>
            {
                CharacterAdded?.Invoke(unityEvent.Parameters);
                return true;
            }));

            EventHandlerRegister.SetHandler(GameEvents.CharactersAdded, new EventInvoker<CharactersAddedEventParameters>(unityEvent =>
            {
                CharactersAdded?.Invoke(unityEvent.Parameters);
                return true;
            }));
        }

        private void RemoveEventsHandlers()
        {
            EventHandlerRegister.RemoveHandler(GameEvents.SceneObjectAdded);
            EventHandlerRegister.RemoveHandler(GameEvents.SceneObjectRemoved);
            EventHandlerRegister.RemoveHandler(GameEvents.SceneObjectsAdded);
            EventHandlerRegister.RemoveHandler(GameEvents.SceneObjectsRemoved);
            EventHandlerRegister.RemoveHandler(GameEvents.PositionChanged);
            EventHandlerRegister.RemoveHandler(GameEvents.PlayerStateChanged);
            EventHandlerRegister.RemoveHandler(GameEvents.CharacterAdded);
            EventHandlerRegister.RemoveHandler(GameEvents.CharactersAdded);
        }

        public async Task<AuthenticationStatus> Authenticate(IYield yield)
        {
            if (!IsConnected())
            {
                return AuthenticationStatus.Failed;
            }

            var parameters = new AuthenticateRequestParameters(AccessTokenProvider.AccessToken);
            var requestId = OperationRequestSender.Send(GameOperations.Authenticate, parameters, MessageSendOptions.DefaultReliable());
            var responseParameters = await SubscriptionProvider.ProvideSubscription<AuthenticateResponseParameters>(yield, requestId);
            authenticationStatus = responseParameters.Status;
            return responseParameters.Status;
        }

        public async Task<EnterSceneResponseParameters> EnterScene(IYield yield)
        {
            if (!IsConnected())
            {
                return new EnterSceneResponseParameters();
            }

            var enteredSceneRequestId = OperationRequestSender.Send(GameOperations.EnterScene, new EmptyParameters(), MessageSendOptions.DefaultReliable());
            var responseParameters = await SubscriptionProvider.ProvideSubscription<EnterSceneResponseParameters>(yield, enteredSceneRequestId);
            return responseParameters;
        }

        public async Task<FetchCharactersResponseParameters> FetchCharacters(IYield yield)
        {
            if (!IsConnected())
            {
                return new FetchCharactersResponseParameters(new Character[0]);
            }

            var requestId = OperationRequestSender.Send(GameOperations.FetchCharacters, new EmptyParameters(), MessageSendOptions.DefaultReliable());
            var responseParameters = await SubscriptionProvider.ProvideSubscription<FetchCharactersResponseParameters>(yield, requestId);
            return responseParameters;
        }

        public async Task<ValidateCharacterStatus> ValidateCharacter(IYield yield, ValidateCharacterRequestParameters parameters)
        {
            if (!IsConnected())
            {
                return ValidateCharacterStatus.Wrong;
            }

            var requestId = OperationRequestSender.Send(GameOperations.ValidateCharacter, parameters, MessageSendOptions.DefaultReliable());
            var responseParameters = await SubscriptionProvider.ProvideSubscription<ValidateCharacterResponseParameters>(yield, requestId);
            return responseParameters.Status;
        }

        public async Task<CreateCharacterResponseParameters> CreateCharacter(IYield yield, CreateCharacterRequestParameters parameters)
        {
            if (!IsConnected())
            {
                return new CreateCharacterResponseParameters(CharacterCreationStatus.Failed);
            }

            var requestId = OperationRequestSender.Send(GameOperations.CreateCharacter, parameters, MessageSendOptions.DefaultReliable());
            var responseParameters = await SubscriptionProvider.ProvideSubscription<CreateCharacterResponseParameters>(yield, requestId);
            return responseParameters;
        }

        public async Task<RemoveCharacterResponseParameters> RemoveCharacter(IYield yield, RemoveCharacterRequestParameters parameters)
        {
            if (!IsConnected())
            {
                return new RemoveCharacterResponseParameters(RemoveCharacterStatus.Failed);
            }

            var requestId = OperationRequestSender.Send(GameOperations.RemoveCharacter, parameters, MessageSendOptions.DefaultReliable());
            var responseParameters = await SubscriptionProvider.ProvideSubscription<RemoveCharacterResponseParameters>(yield, requestId);
            return responseParameters;
        }

        public void UpdatePosition(UpdatePositionRequestParameters parameters)
        {
            if (!IsConnected())
            {
                return;
            }

            OperationRequestSender.Send(GameOperations.PositionChanged, parameters, MessageSendOptions.DefaultUnreliable((byte)GameDataChannels.Position));
        }

        public void UpdatePlayerState(UpdatePlayerStateRequestParameters parameters)
        {
            if (!IsConnected())
            {
                return;
            }

            OperationRequestSender.Send(GameOperations.PlayerStateChanged, parameters, MessageSendOptions.DefaultUnreliable((byte)GameDataChannels.Animations));
        }

        public async Task<ChangeSceneResponseParameters> ChangeScene(IYield yield, ChangeSceneRequestParameters parameters)
        {
            if (!IsConnected())
            {
                return new ChangeSceneResponseParameters(0);
            }

            var requestId = OperationRequestSender.Send(GameOperations.ChangeScene, parameters, MessageSendOptions.DefaultReliable());
            var responseParameters = await SubscriptionProvider.ProvideSubscription<ChangeSceneResponseParameters>(yield, requestId);
            return responseParameters;
        }
    }
}