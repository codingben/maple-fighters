using System;
using Game.Application.Components;

namespace Game.Application.Objects.Components
{
    public class AnimationData : ComponentBase, IAnimationData
    {
        public event Action AnimationStateChanged;

        private byte animationState;

        public AnimationData()
        {
            // Left blank intentionally
        }

        public void SetAnimationState(byte animationState)
        {
            this.animationState = animationState;

            AnimationStateChanged?.Invoke();
        }

        public byte GetAnimationState()
        {
            return animationState;
        }
    }
}