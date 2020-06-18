using Common.ComponentModel;
using Game.Application.Messages;
using InterestManagement;

namespace Game.Application.Objects.Components
{
    [ComponentSettings(ExposedState.Unexposable)]
    public class BubbleNotificationSender : ComponentBase
    {
        private readonly string text;
        private readonly int time;

        private IMessageSender messageSender;
        private IGameObject gameObject;
        private INearbySceneObjectsEvents<IGameObject> nearbySceneObjectsEvents;

        public BubbleNotificationSender(string text, int time)
        {
            this.text = text;
            this.time = time;
        }

        protected override void OnAwake()
        {
            messageSender = Components.Get<IMessageSender>();

            var gameObjectGetter = Components.Get<IGameObjectGetter>();
            gameObject = gameObjectGetter.Get();

            var proximityChecker = Components.Get<IProximityChecker>();
            nearbySceneObjectsEvents = proximityChecker.GetNearbyGameObjectsEvents();

            SubscribeToNearbyGameObjectAdded();
        }

        protected override void OnRemoved()
        {
            UnsubscribeFromNearbyGameObjectAdded();
        }

        private void SubscribeToNearbyGameObjectAdded()
        {
            nearbySceneObjectsEvents.SceneObjectAdded += OnNearbyGameObjectAdded;
        }

        private void UnsubscribeFromNearbyGameObjectAdded()
        {
            nearbySceneObjectsEvents.SceneObjectAdded -= OnNearbyGameObjectAdded;
        }

        private void OnNearbyGameObjectAdded(IGameObject _)
        {
            var message = new BubbleNotificationMessage()
            {
                RequesterId = gameObject.Id,
                Message = text,
                Time = time
            };

            messageSender.SendMessage((byte)MessageCodes.BubbleNotification, message);
        }
    }
}