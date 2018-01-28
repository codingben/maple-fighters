using System.Threading.Tasks;
using CommonTools.Coroutines;
using Shared.Game.Common;

namespace Scripts.Services
{
    public interface IGameService : IServiceBase
    {
        Task<AuthenticationStatus> Authenticate(IYield yield);

        Task<EnterSceneResponseParameters?> EnterScene(IYield yield);

        void UpdatePosition(UpdatePositionRequestParameters parameters);
        void UpdatePlayerState(UpdatePlayerStateRequestParameters parameters);

        Task<FetchCharactersResponseParameters> FetchCharacters(IYield yield);
        Task<ValidateCharacterStatus> ValidateCharacter(IYield yield, ValidateCharacterRequestParameters parameters);
        Task<CreateCharacterResponseParameters> CreateCharacter(IYield yield, CreateCharacterRequestParameters parameters);
        Task<RemoveCharacterResponseParameters> RemoveCharacter(IYield yield, RemoveCharacterRequestParameters parameters);
        Task<ChangeSceneResponseParameters> ChangeScene(IYield yield, ChangeSceneRequestParameters parameters);

        UnityEvent<EnterSceneResponseParameters> EnteredScene { get; }

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