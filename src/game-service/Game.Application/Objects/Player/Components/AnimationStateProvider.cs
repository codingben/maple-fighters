using System;
using Game.Application.Components;

namespace Game.Application.Objects.Components
{
    public class AnimationStateProvider : ComponentBase, IAnimationStateProvider
    {
        public event Action AnimationStateChanged;

        private byte animationState;

        public AnimationStateProvider()
        {
            // Left blank intentionally
        }

        public void SetState(byte animationState)
        {
            this.animationState = animationState;

            AnimationStateChanged?.Invoke();
        }

        public byte GetState()
        {
            return animationState;
        }
    }
}