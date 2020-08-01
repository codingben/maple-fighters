using Game.Messages;
using Game.Network;
using Game.Application.Objects;
using Game.Application.Objects.Components;

namespace Game.Application.Handlers
{
    public class ChangeAnimationStateHandler : IMessageHandler<ChangeAnimationStateMessage>
    {
        private readonly IAnimationData animationData;

        public ChangeAnimationStateHandler(IGameObject player)
        {
            animationData = player.Components.Get<IAnimationData>();
        }

        public void Handle(ChangeAnimationStateMessage message)
        {
            var animationState = message.AnimationState;

            animationData.SetAnimationState(animationState);
        }
    }
}