using Common.MathematicsHelper;
using InterestManagement;
using Game.Network;
using Game.Messages;
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

            transform.SetPosition(position);
        }
    }
}