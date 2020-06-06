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
            var scene = presenceSceneProvider.GetScene();
            var gameObjectGetter = Components.Get<IGameObjectGetter>();
            var gameObject = gameObjectGetter.GetGameObject();

            interestArea = new InterestArea<IGameObject>(scene, gameObject);
        }

        protected override void OnRemoved()
        {
            interestArea?.Dispose();
        }

        public INearbySceneObjectsEvents<IGameObject> GetEvents()
        {
            return interestArea.NearbySceneObjectsEvents;
        }
    }
}