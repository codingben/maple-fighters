using System.Collections.Generic;
using InterestManagement;

namespace Game.Application.Objects.Components
{
    public interface IProximityChecker
    {
        IEnumerable<IGameObject> GetNearbyGameObjects();

        INearbySceneObjectsEvents<IGameObject> GetNearbyGameObjectsEvents();
    }
}