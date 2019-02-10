using System.Threading.Tasks;
using CommonTools.Coroutines;
using Game.Common;
using Scripts.Network.Core;
using Scripts.Network.Dummies;

namespace Scripts.Network.APIs
{
    public class DummyGameSceneApi : ApiBase<GameOperations, GameEvents>, IGameSceneApi
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

        public DummyGameSceneApi()
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
            await Task.Delay(1);
        }

        public Task<ChangeSceneResponseParameters> ChangeSceneAsync(
            IYield yield,
            ChangeSceneRequestParameters parameters)
        {
            var map = 
                DummyPortalContainer.GetInstance().GetMap(parameters.PortalId);

            return Task.FromResult(new ChangeSceneResponseParameters(map));
        }

        public Task UpdatePosition(UpdatePositionRequestParameters parameters)
        {
            return Task.CompletedTask;
        }

        public Task UpdatePlayerState(
            UpdatePlayerStateRequestParameters parameters)
        {
            return Task.CompletedTask;
        }
    }
}