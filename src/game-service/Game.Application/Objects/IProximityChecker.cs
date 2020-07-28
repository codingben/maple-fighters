using System.Collections.Generic;
using InterestManagement;

namespace Game.Application.Objects
{
    public interface IProximityChecker
    {
        void SetMatrixRegion(IMatrixRegion<IGameObject> region);

        IEnumerable<IGameObject> GetNearbyGameObjects();

        INearbySceneObjectsEvents<IGameObject> GetNearbyGameObjectsEvents();
    }
}