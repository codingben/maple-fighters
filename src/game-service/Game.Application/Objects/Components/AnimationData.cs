using System;
using Common.ComponentModel;

namespace Game.Application.Objects.Components
{
    [ComponentSettings(ExposedState.Exposable)]
    public class AnimationData : ComponentBase, IAnimationData
    {
        public event Action AnimationStateChanged;

        private byte animationState;

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