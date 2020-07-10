using Common.ComponentModel;
using Game.Application.Messages;

namespace Game.Application.Objects.Components
{
    [ComponentSettings(ExposedState.Exposable)]
    public class BubbleNotificationSender : ComponentBase
    {
        private readonly string text;
        private readonly int time;

        private IProximityChecker proximityChecker;
        private IGameObjectGetter gameObjectGetter;
        private IMessageSender messageSender;

        public BubbleNotificationSender(string text, int time)
        {
            this.text = text;
            this.time = time;
        }

        protected override void OnAwake()
        {
            proximityChecker = Components.Get<IProximityChecker>();
            gameObjectGetter = Components.Get<IGameObjectGetter>();
            messageSender = Components.Get<IMessageSender>();

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

        private void OnNearbyGameObjectAdded(IGameObject _)
        {
            var id = gameObjectGetter.Get().Id;
            var messageCode = (byte)MessageCodes.BubbleNotification;
            var message = new BubbleNotificationMessage()
            {
                RequesterId = id,
                Message = text,
                Time = time
            };

            messageSender.SendMessage(messageCode, message);
        }
    }
}