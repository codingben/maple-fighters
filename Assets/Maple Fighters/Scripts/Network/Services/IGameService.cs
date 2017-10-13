using System;
using System.Threading.Tasks;
using CommonTools.Coroutines;
using Shared.Game.Common;

namespace Scripts.Services
{
    public interface IGameService
    {
        event Action Connected;
        event Action Authenticated;

        void Connect();
        void Disconnect();

        void EnterWorld();

        void UpdatePosition(UpdatePositionRequestParameters parameters);
        void UpdatePlayerState(UpdatePlayerStateRequestParameters parameters);

        Task Authenticate(IYield yield);

        Task<FetchCharactersResponseParameters> FetchCharacters(IYield yield);
        Task<ValidateCharacterStatus> ValidateCharacter(IYield yield, ValidateCharacterRequestParameters parameters);
        Task<CreateCharacterResponseParameters> CreateCharacter(IYield yield, CreateCharacterRequestParameters parameters);
        Task<RemoveCharacterResponseParameters> RemoveCharacter(IYield yield, RemoveCharacterRequestParameters parameters);
        Task ChangeScene(IYield yield, ChangeSceneRequestParameters parameters);

        UnityEvent<GameObjectAddedEventParameters> GameObjectAdded { get; }
        UnityEvent<GameObjectRemovedEventParameters> GameObjectRemoved { get; }
        UnityEvent<GameObjectsAddedEventParameters> GameObjectsAdded { get; }
        UnityEvent<GameObjectsRemovedEventParameters> GameObjectsRemoved { get; }

        UnityEvent<GameObjectPositionChangedEventParameters> PositionChanged { get; }
        UnityEvent<PlayerStateChangedEventParameters> PlayerStateChanged { get; }
    }
}