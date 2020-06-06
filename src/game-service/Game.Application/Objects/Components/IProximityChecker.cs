using InterestManagement;

namespace Game.Application.Objects.Components
{
    public interface IProximityChecker
    {
        INearbySceneObjectsEvents<IGameObject> GetEvents();
    }
}