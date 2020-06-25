using System.Collections.Generic;
using Game.Application.Components;
using InterestManagement;

namespace Game.Application.Objects.Components
{
    public interface IProximityChecker
    {
        void ChangeGameScene(IGameScene gameScene);

        IGameScene GetGameScene();

        IEnumerable<IGameObject> GetNearbyGameObjects();

        INearbySceneObjectsEvents<IGameObject> GetNearbyGameObjectsEvents();
    }
}