using System.Collections.Generic;
using Common.ComponentModel;
using InterestManagement;

namespace Game.Application.Objects.Components
{
    [ComponentSettings(ExposedState.Exposable)]
    public class ProximityChecker : ComponentBase, IProximityChecker
    {
        private IInterestArea<IGameObject> interestArea;

        protected override void OnAwake()
        {
            var presenceSceneProvider = Components.Get<IPresenceSceneProvider>();
            var gameScene = presenceSceneProvider.GetScene();
            var gameObjectGetter = Components.Get<IGameObjectGetter>();
            var gameObject = gameObjectGetter.Get();

            interestArea = new InterestArea<IGameObject>(gameObject);
            interestArea.SetScene(gameScene);
        }

        protected override void OnRemoved()
        {
            interestArea?.Dispose();
        }

        public void ChangeScene()
        {
            var presenceSceneProvider = Components.Get<IPresenceSceneProvider>();
            var gameScene = presenceSceneProvider.GetScene();

            interestArea?.Dispose();
            interestArea?.SetScene(gameScene);
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