using System;
using Game.Application.Network;
using Game.Application.Objects.Components;

namespace Game.Application.Handlers
{
    public class ChangeAnimationStateHandler : IMessageHandler
    {
        private IAnimationData animationData;

        public ChangeAnimationStateHandler(IAnimationData animationData)
        {
            this.animationData = animationData;
        }

        public void Handle(byte[] rawData)
        {
            throw new NotImplementedException();
        }
    }
}