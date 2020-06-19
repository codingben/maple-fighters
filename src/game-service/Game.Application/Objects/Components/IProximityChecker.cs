using System.Collections.Generic;
using InterestManagement;

namespace Game.Application.Objects.Components
{
    public interface IProximityChecker
    {
        void ChangeScene();

        IEnumerable<IGameObject> GetNearbyGameObjects();

        INearbySceneObjectsEvents<IGameObject> GetNearbyGameObjectsEvents();
    }
}