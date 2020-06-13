using Game.Application.Messages;
using Game.Application.Network;
using Game.Application.Objects.Components;

namespace Game.Application.Handlers
{
    public class ChangeAnimationStateHandler : IMessageHandler
    {
        private readonly IAnimationData animationData;

        public ChangeAnimationStateHandler(IAnimationData animationData)
        {
            this.animationData = animationData;
        }

        public void Handle(byte[] rawData)
        {
            var message =
                MessageUtils.DeserializeMessage<ChangeAnimationStateMessage>(rawData);
            var animationState = message.AnimationState;

            animationData.SetAnimationState(animationState);
        }
    }
}