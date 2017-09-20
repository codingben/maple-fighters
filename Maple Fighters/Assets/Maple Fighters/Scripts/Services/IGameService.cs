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

        UnityEvent<EntityAddedEventParameters> EntityAdded { get; }
        UnityEvent<EntityRemovedEventParameters> EntityRemoved { get; }
        UnityEvent<EntitiesAddedEventParameters> EntitiesAdded { get; }
        UnityEvent<EntitiesRemovedEventParameters> EntitiesRemoved { get; }

        UnityEvent<EntityPositionChangedEventParameters> EntityPositionChanged { get; }

        UnityEvent<PlayerStateChangedEventParameters> PlayerStateChanged { get; }
    }
}