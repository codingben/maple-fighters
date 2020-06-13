using System;

namespace Game.Application.Objects.Components
{
    public interface IAnimationData
    {
        event Action AnimationStateChanged;

        void SetAnimationState(byte animationState);

        byte GetAnimationState();
    }
}