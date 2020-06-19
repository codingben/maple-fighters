using System.Collections.Generic;
using Common.ComponentModel;
using InterestManagement;

namespace Game.Application.Objects.Components
{
    [ComponentSettings(ExposedState.Exposable)]
    public class ProximityChecker : ComponentBase, IProximityChecker
    {
        private InterestArea<IGameObject> interestArea;

        protected override void OnAwake()
        {
            var presenceSceneProvider = Components.Get<IPresenceSceneProvider>();
            var scene = presenceSceneProvider.GetScene();
            var gameObjectGetter = Components.Get<IGameObjectGetter>();
            var gameObject = gameObjectGetter.Get();

            interestArea = new InterestArea<IGameObject>(gameObject);
            interestArea.SetScene(scene);
        }

        protected override void OnRemoved()
        {
            interestArea?.Dispose();
        }

        public IEnumerable<IGameObject> GetNearbyGameObjects()
        {
            return interestArea.GetNearbySceneObjects();
        }

        public INearbySceneObjectsEvents<IGameObject> GetNearbyGameObjectsEvents()
        {
            return interestArea.GetNearbySceneObjectsEvents();
        }
    }
}