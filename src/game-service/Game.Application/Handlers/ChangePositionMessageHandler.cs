using Game.Application.Objects;
using Common.MathematicsHelper;

namespace Game.Application
{
    public class ChangePositionMessageHandler : IMessageHandler
    {
        private readonly IGameObject player;

        public ChangePositionMessageHandler(IGameObject player)
        {
            this.player = player;
        }

        public void Handle(byte[] rawData)
        {
            var message = MessageUtils.GetMessage<ChangePlayerPositionMessage>(rawData);
            var x = message.X;
            var y = message.Y;

            player.Transform.SetPosition(new Vector2(x, y));
        }
    }
}