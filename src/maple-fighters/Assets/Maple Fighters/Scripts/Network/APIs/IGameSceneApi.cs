using System.Threading.Tasks;
using CommonTools.Coroutines;
using Game.Common;

namespace Scripts.Network.APIs
{
    public interface IGameSceneApi : IApiBase
    {
        Task EnterSceneAsync(IYield yield);

        Task<ChangeSceneResponseParameters> ChangeSceneAsync(
            IYield yield,
            ChangeSceneRequestParameters parameters);

        Task UpdatePosition(UpdatePositionRequestParameters parameters);

        Task UpdatePlayerState(UpdatePlayerStateRequestParameters parameters);

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
    }
}