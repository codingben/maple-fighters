using Common.MathematicsHelper;
using InterestManagement;
using Game.Application.Network;
using Game.Application.Messages;
using Game.Application.Objects;

namespace Game.Application.Handlers
{
    public class ChangePositionMessageHandler : IMessageHandler
    {
        private readonly ITransform transform;

        public ChangePositionMessageHandler(IGameObject player)
        {
            this.transform = player.Transform;
        }

        public void Handle(byte[] rawData)
        {
            var message =
                MessageUtils.DeserializeMessage<ChangePositionMessage>(rawData);
            var x = message.X;
            var y = message.Y;
            var position = new Vector2(x, y);

            transform.SetPosition(position);
        }
    }
}