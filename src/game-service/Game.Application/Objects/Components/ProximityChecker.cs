using System.Collections.Generic;
using Common.ComponentModel;
using Game.Application.Components;
using InterestManagement;

namespace Game.Application.Objects.Components
{
    [ComponentSettings(ExposedState.Exposable)]
    public class ProximityChecker : ComponentBase, IProximityChecker
    {
        private IGameScene gameScene;
        private IInterestArea<IGameObject> interestArea;

        public ProximityChecker(IGameScene gameScene)
        {
            this.gameScene = gameScene;
        }

        protected override void OnAwake()
        {
            var gameObjectGetter = Components.Get<IGameObjectGetter>();
            var gameObject = gameObjectGetter.Get();

            interestArea = new InterestArea<IGameObject>(gameObject);
            interestArea.SetScene(gameScene);
        }

        protected override void OnRemoved()
        {
            interestArea?.Dispose();
        }

        public void ChangeGameScene(IGameScene gameScene)
        {
            interestArea?.Dispose();
            interestArea?.SetScene(gameScene);
        }

        public IGameScene GetGameScene()
        {
            return gameScene;
        }

        public IEnumerable<IGameObject> GetNearbyGameObjects()
        {
            return interestArea?.GetNearbySceneObjects();
        }

        public INearbySceneObjectsEvents<IGameObject> GetNearbyGameObjectsEvents()
        {
            return interestArea?.GetNearbySceneObjectsEvents();
        }
    }
}