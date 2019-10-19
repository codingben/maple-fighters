using System.Threading.Tasks;
using CommonTools.Coroutines;
using Game.Common;
using Network.Scripts;

namespace Scripts.Services.Game
{
    public interface IGameSceneApi
    {
        UnityEvent<EnterSceneResponseParameters> SceneEntered { get; }

        UnityEvent<SceneObjectAddedEventParameters> SceneObjectAdded { get; }

        UnityEvent<SceneObjectRemovedEventParameters> SceneObjectRemoved { get; }

        UnityEvent<SceneObjectsAddedEventParameters> SceneObjectsAdded { get; }

        UnityEvent<SceneObjectsRemovedEventParameters> SceneObjectsRemoved { get; }

        UnityEvent<SceneObjectPositionChangedEventParameters> PositionChanged { get; }

        UnityEvent<PlayerStateChangedEventParameters> PlayerStateChanged { get; }

        UnityEvent<PlayerAttackedEventParameters> PlayerAttacked { get; }

        UnityEvent<CharacterAddedEventParameters> CharacterAdded { get; }

        UnityEvent<CharactersAddedEventParameters> CharactersAdded { get; }

        UnityEvent<BubbleMessageEventParameters> BubbleMessageReceived { get; }

        Task EnterSceneAsync(IYield yield);

        Task<ChangeSceneResponseParameters> ChangeSceneAsync(IYield yield, ChangeSceneRequestParameters parameters);

        void UpdatePosition(UpdatePositionRequestParameters parameters);

        void UpdatePlayerState(UpdatePlayerStateRequestParameters parameters);
    }
}