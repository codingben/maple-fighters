using Shared.Game.Common;

namespace Scripts.Services
{
    public interface IGameService
    {
        void Connect();

        void UpdateEntityPosition(UpdateEntityPositionRequestParameters parameters);

        UnityEvent<EntityInitialInfomraitonEventParameters> EntitiyInitialInformation { get; }

        UnityEvent<EntityAddedEventParameters> EntityAdded { get; }
        UnityEvent<EntityRemovedEventParameters> EntityRemoved { get; }

        UnityEvent<EntityPositionChangedEventParameters> EntityPositionChanged { get; }
    }
}