using Game.Messages;
using Game.MessageTools;
using Game.Application.Objects;
using Game.Application.Objects.Components;

namespace Game.Application.Handlers
{
    public class ChangeAnimationStateHandler : IMessageHandler<ChangeAnimationStateMessage>
    {
        private readonly IAnimationStateProvider animationStateProvider;

        public ChangeAnimationStateHandler(IGameObject player)
        {
            animationStateProvider =
                player.Components.Get<IAnimationStateProvider>();
        }

        public void Handle(ChangeAnimationStateMessage message)
        {
            var animationState = message.AnimationState;

            animationStateProvider.SetState(animationState);
        }
    }
}