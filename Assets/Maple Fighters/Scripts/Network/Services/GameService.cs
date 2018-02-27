using System.Threading.Tasks;
using Authorization.Client.Common;
using CommonCommunicationInterfaces;
using CommonTools.Coroutines;
using CommunicationHelper;
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

        private AuthorizationStatus authorizationStatus = AuthorizationStatus.Failed;

        protected override void OnConnected()
        {
            SetEventsHandlers();
        }

        protected override void OnDisconnected()
        {
            RemoveEventsHandlers();
            GoBackToLogin();
        }

        private void GoBackToLogin()
        {
            if (authorizationStatus == AuthorizationStatus.Succeed)
            {
                SavedObjects.DestroyAll();
            }
        }

        private void SetEventsHandlers()
        {
            SetEventHandler(GameEvents.SceneObjectAdded, SceneObjectAdded);
            SetEventHandler(GameEvents.SceneObjectRemoved, SceneObjectRemoved);
            SetEventHandler(GameEvents.SceneObjectsAdded, SceneObjectsAdded);
            SetEventHandler(GameEvents.SceneObjectsRemoved, SceneObjectsRemoved);
            SetEventHandler(GameEvents.PositionChanged, PositionChanged);
            SetEventHandler(GameEvents.PlayerStateChanged, PlayerStateChanged);
            SetEventHandler(GameEvents.PlayerAttacked, PlayerAttacked);
            SetEventHandler(GameEvents.CharacterAdded, CharacterAdded);
            SetEventHandler(GameEvents.CharactersAdded, CharactersAdded);
        }

        private void RemoveEventsHandlers()
        {
            RemoveEventHandler(GameEvents.SceneObjectAdded);
            RemoveEventHandler(GameEvents.SceneObjectRemoved);
            RemoveEventHandler(GameEvents.SceneObjectsAdded);
            RemoveEventHandler(GameEvents.SceneObjectsRemoved);
            RemoveEventHandler(GameEvents.PositionChanged);
            RemoveEventHandler(GameEvents.PlayerStateChanged);
            RemoveEventHandler(GameEvents.PlayerAttacked);
            RemoveEventHandler(GameEvents.CharacterAdded);
            RemoveEventHandler(GameEvents.CharactersAdded);
        }

        public async Task<AuthorizationStatus> Authorize(IYield yield)
        {
            if (!IsConnected())
            {
                return AuthorizationStatus.Failed;
            }

            var parameters = new AuthorizeRequestParameters(AccessTokenProvider.AccessToken);
            var requestId = OperationRequestSender.Send(GameOperations.Authorize, parameters, MessageSendOptions.DefaultReliable());
            var responseParameters = await SubscriptionProvider.ProvideSubscription<AuthorizeResponseParameters>(yield, requestId);
            authorizationStatus = responseParameters.Status;
            return responseParameters.Status;
        }

        public async Task<EnterSceneResponseParameters?> EnterScene(IYield yield)
        {
            if (!IsConnected())
            {
                return null;
            }

            var enteredSceneRequestId = OperationRequestSender.Send(GameOperations.EnterScene, new EmptyParameters(), MessageSendOptions.DefaultReliable());
            var responseParameters = await SubscriptionProvider.ProvideSubscription<EnterSceneResponseParameters>(yield, enteredSceneRequestId);
            return responseParameters;
        }

        public async Task<CharacterValidationStatus> ValidateCharacter(IYield yield, ValidateCharacterRequestParameters parameters)
        {
            if (!IsConnected())
            {
                return CharacterValidationStatus.Wrong;
            }

            var requestId = OperationRequestSender.Send(GameOperations.ValidateCharacter, parameters, MessageSendOptions.DefaultReliable());
            var responseParameters = await SubscriptionProvider.ProvideSubscription<ValidateCharacterResponseParameters>(yield, requestId);
            return responseParameters.Status;
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