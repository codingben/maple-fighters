using System.Threading.Tasks;
using Authorization.Client.Common;
using CommonTools.Coroutines;
using Game.Common;

namespace Scripts.Services
{
    public interface IGameServiceAPI : IServiceBase
    {
        Task<AuthorizeResponseParameters> Authorize(IYield yield, AuthorizeRequestParameters parameters);
        Task EnterScene(IYield yield);
        Task<ChangeSceneResponseParameters> ChangeScene(IYield yield, ChangeSceneRequestParameters parameters);

        void UpdatePosition(UpdatePositionRequestParameters parameters);
        void UpdatePlayerState(UpdatePlayerStateRequestParameters parameters);

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
    }
}