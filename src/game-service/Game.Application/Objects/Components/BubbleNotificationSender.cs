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
        private INearbySceneObjectsEvents<IGameObject> nearbySceneObjectsEvents;

        public BubbleNotificationSender(string text, int time)
        {
            this.text = text;
            this.time = time;
        }

        protected override void OnAwake()
        {
            messageSender = Components.Get<IMessageSender>();

            var proximityChecker = Components.Get<IProximityChecker>();
            nearbySceneObjectsEvents = proximityChecker.GetNearbyGameObjectsEvents();

            SubscribeToGameObjectAdded();
        }

        protected override void OnRemoved()
        {
            UnsubscribeFromGameObjectAdded();
        }

        private void SubscribeToGameObjectAdded()
        {
            nearbySceneObjectsEvents.SceneObjectAdded += OnNearbyGameObjectAdded;
        }

        private void UnsubscribeFromGameObjectAdded()
        {
            nearbySceneObjectsEvents.SceneObjectAdded -= OnNearbyGameObjectAdded;
        }

        private void OnNearbyGameObjectAdded(IGameObject gameObject)
        {
            var message = new BubbleNotificationMessage()
            {
                Message = text,
                Time = time
            };

            messageSender.SendMessage((byte)MessageCodes.BubbleNotification, message);
        }
    }
}