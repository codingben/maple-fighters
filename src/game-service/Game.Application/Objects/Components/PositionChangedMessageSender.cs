using Common.ComponentModel;
using Game.Application.Messages;

namespace Game.Application.Objects.Components
{
    [ComponentSettings(ExposedState.Unexposable)]
    public class PositionChangedMessageSender : ComponentBase
    {
        private IGameObjectGetter gameObjectGetter;
        private IMessageSender messageSender;

        protected override void OnAwake()
        {
            gameObjectGetter = Components.Get<IGameObjectGetter>();
            messageSender = Components.Get<IMessageSender>();

            SubscribeToPositionChanged();
        }

        protected override void OnRemoved()
        {
            UnsubscribeFromPositionChanged();
        }

        private void SubscribeToPositionChanged()
        {
            var transform = gameObjectGetter.Get().Transform;
            transform.PositionChanged += OnPositionChanged;
        }

        private void UnsubscribeFromPositionChanged()
        {
            var transform = gameObjectGetter.Get().Transform;
            transform.PositionChanged -= OnPositionChanged;
        }

        private void OnPositionChanged()
        {
            var id = gameObjectGetter.Get().Id;
            var transform = gameObjectGetter.Get().Transform;
            var messageCode = (byte)MessageCodes.PositionChanged;
            var message = new PositionChangedMessage()
            {
                GameObjectId = id,
                X = transform.Position.X,
                Y = transform.Position.Y
            };

            messageSender.SendMessageToNearbyGameObjects(messageCode, message);
        }
    }
}