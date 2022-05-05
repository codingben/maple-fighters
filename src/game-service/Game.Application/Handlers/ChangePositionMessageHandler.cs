using InterestManagement;
using Game.Messages;
using Game.MessageTools;
using Game.Application.Objects;

namespace Game.Application.Handlers
{
    public class ChangePositionMessageHandler : IMessageHandler<ChangePositionMessage>
    {
        private readonly ITransform transform;

        public ChangePositionMessageHandler(IGameObject player)
        {
            transform = player.Transform;
        }

        public void Handle(ChangePositionMessage message)
        {
            var x = message.X;
            var y = message.Y;
            var position = new Vector2(x, y);
            var direction = (transform.Position - position).Normalize();
            direction = new Vector2(direction.X > 0 ? 1 : -1, direction.Y);

            transform.SetPosition(position);
            transform.SetDirection(direction);
        }
    }
}