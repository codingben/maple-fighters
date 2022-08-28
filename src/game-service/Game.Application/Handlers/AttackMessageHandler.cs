using Game.Application.Objects;
using Game.Messages;
using Game.MessageTools;

namespace Game.Application.Handlers
{
    public class AttackMessageHandler : IMessageHandler<AttackMessage>
    {
        private readonly IGameObject player;

        public AttackMessageHandler(IGameObject player)
        {
            this.player = player;
        }

        public void Handle(AttackMessage message)
        {
            // TODO: Implement
        }
    }
}