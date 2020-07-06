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
            var gameObjectGetter = Components.Get<IGameObjectGetter>();
            var gameObject = gameObjectGetter.Get();

            interestArea = new InterestArea<IGameObject>(gameObject);
        }

        protected override void OnRemoved()
        {
            interestArea?.Dispose();
        }

        public void SetMatrixRegion(IMatrixRegion<IGameObject> matrixRegion)
        {
            interestArea?.Dispose();
            interestArea?.SetMatrixRegion(matrixRegion);
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