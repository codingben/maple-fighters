using Common.ComponentModel;
using Game.Application.Messages;

namespace Game.Application.Objects.Components
{
    [ComponentSettings(ExposedState.Unexposable)]
    public class PositionChangedMessageSender : ComponentBase
    {
        private IMessageSender messageSender;
        private IGameObject gameObject;

        protected override void OnAwake()
        {
            messageSender = Components.Get<IMessageSender>();

            var gameObjectGetter = Components.Get<IGameObjectGetter>();
            gameObject = gameObjectGetter.Get();

            SubscribeToPositionChanged();
        }

        protected override void OnRemoved()
        {
            UnsubscribeFromPositionChanged();
        }

        private void SubscribeToPositionChanged()
        {
            gameObject.Transform.PositionChanged += OnPositionChanged;
        }

        private void UnsubscribeFromPositionChanged()
        {
            gameObject.Transform.PositionChanged -= OnPositionChanged;
        }

        private void OnPositionChanged()
        {
            var message = new PositionChangedMessage()
            {
                GameObjectId = gameObject.Id,
                X = gameObject.Transform.Position.X,
                Y = gameObject.Transform.Position.Y
            };

            messageSender.SendMessage((byte)MessageCodes.PositionChanged, message);
        }
    }
}