using System.Threading.Tasks;
using CommonTools.Coroutines;
using Shared.Game.Common;

namespace Scripts.Services
{
    public interface IGameService
    {
        void Connect();
        void Disconnect();

        void UpdateEntityPosition(UpdateEntityPositionRequestParameters parameters);
        void UpdatePlayerState(UpdatePlayerStateRequestParameters parameters);
        Task ChangeScene(IYield yield, ChangeSceneRequestParameters parameters);

        UnityEvent<EnterWorldOperationResponseParameters> EntitiyInitialInformation { get; }

        UnityEvent<GameObjectAddedEventParameters> EntityAdded { get; }
        UnityEvent<GameObjectRemovedEventParameters> EntityRemoved { get; }
        UnityEvent<GameObjectsAddedEventParameters> EntitiesAdded { get; }
        UnityEvent<GameObjectsRemovedEventParameters> EntitiesRemoved { get; }

        UnityEvent<GameObjectPositionChangedEventParameters> EntityPositionChanged { get; }

        UnityEvent<PlayerStateChangedEventParameters> PlayerStateChanged { get; }
    }
}