using Common.ComponentModel;

namespace Game.Application.Objects.Components
{
    public class GuardianIdleBehaviour : ComponentBase
    {
        private readonly string text;
        private readonly int time;

        private IProximityChecker proximityChecker;
        private IGameObjectGetter gameObjectGetter;

        public GuardianIdleBehaviour(string text, int time)
        {
            this.text = text;
            this.time = time;
        }

        protected override void OnAwake()
        {
            proximityChecker = Components.Get<IProximityChecker>();
            gameObjectGetter = Components.Get<IGameObjectGetter>();

            SubscribeToNearbyGameObjectAdded();
        }

        protected override void OnRemoved()
        {
            UnsubscribeFromNearbyGameObjectAdded();
        }

        private void SubscribeToNearbyGameObjectAdded()
        {
            var nearbySceneObjectsEvents = proximityChecker.GetNearbyGameObjectsEvents();
            nearbySceneObjectsEvents.SceneObjectAdded += OnNearbyGameObjectAdded;
        }

        private void UnsubscribeFromNearbyGameObjectAdded()
        {
            var nearbySceneObjectsEvents = proximityChecker.GetNearbyGameObjectsEvents();
            nearbySceneObjectsEvents.SceneObjectAdded -= OnNearbyGameObjectAdded;
        }

        private void OnNearbyGameObjectAdded(IGameObject gameObject)
        {
            var id = gameObjectGetter.Get().Id;
            var bubbleNotifier = gameObject.Components.Get<IBubbleNotifier>();
            bubbleNotifier?.Notify(id, text, time);
        }
    }
}