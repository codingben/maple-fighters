using Game.Application.Components;

namespace Game.Application.Objects.Components
{
    public class NpcIdleBehaviour : ComponentBase
    {
        private string message;
        private int messageTime;
        private IProximityChecker proximityChecker;
        private IGameObjectGetter gameObjectGetter;

        protected override void OnAwake()
        {
            proximityChecker = Components.Get<IProximityChecker>();
            gameObjectGetter = Components.Get<IGameObjectGetter>();

            var npcConfigDataProvider = Components.Get<INpcConfigDataProvider>();
            var npcConfigData = npcConfigDataProvider.Provide();
            message = npcConfigData.Message;
            messageTime = npcConfigData.MessageTime;

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
            var bubbleNotifier = gameObject?.Components?.Get<IBubbleNotifier>();
            bubbleNotifier?.Notify(id, message, messageTime);
        }
    }
}