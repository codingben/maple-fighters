using Shared.Game.Common;

namespace Scripts.Services
{
    public interface IGameService
    {
        void Connect();

        void UpdateEntityPosition(UpdateEntityPositionRequestParameters parameters);

        UnityEvent<EnterWorldOperationResponseParameters> EntitiyInitialInformation { get; }

        UnityEvent<EntityAddedEventParameters> EntityAdded { get; }
        UnityEvent<EntityRemovedEventParameters> EntityRemoved { get; }
        UnityEvent<EntitiesAddedEventParameters> EntitiesAdded { get; }
        UnityEvent<EntitiesRemovedEventParameters> EntitiesRemoved { get; }

        UnityEvent<EntityPositionChangedEventParameters> EntityPositionChanged { get; }
    }
}