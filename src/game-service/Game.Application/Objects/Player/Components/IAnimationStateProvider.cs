using System;

namespace Game.Application.Objects.Components
{
    public interface IAnimationStateProvider
    {
        event Action AnimationStateChanged;

        void SetState(byte animationState);

        byte GetState();
    }
}