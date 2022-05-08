using System.Collections.Generic;
using Game.Application.Components;
using Game.Logger;
using InterestManagement;

namespace Game.Application.Objects
{
    public class ProximityChecker : ComponentBase, IProximityChecker
    {
        private IInterestArea<IGameObject> interestArea;

        protected override void OnAwake()
        {
            var gameObjectGetter = Components.Get<IGameObjectGetter>();
            var gameObject = gameObjectGetter.Get();
            var logger = InterestManagementLogger.GetLogger();

            interestArea = new InterestArea<IGameObject>(gameObject, logger);
        }

        protected override void OnRemoved()
        {
            interestArea?.Dispose();
        }

        public void SetMatrixRegion(IMatrixRegion<IGameObject> region)
        {
            interestArea?.Dispose();
            interestArea?.SetMatrixRegion(region);
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