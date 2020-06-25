using System.Collections.Generic;
using Game.Application.Components;
using InterestManagement;

namespace Game.Application.Objects.Components
{
    public interface IProximityChecker
    {
        void ChangeScene(IGameScene gameScene);

        IEnumerable<IGameObject> GetNearbyGameObjects();

        INearbySceneObjectsEvents<IGameObject> GetNearbyGameObjectsEvents();
    }
}