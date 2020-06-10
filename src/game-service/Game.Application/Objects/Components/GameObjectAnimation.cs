using Common.ComponentModel;

namespace Game.Application.Objects.Components
{
    [ComponentSettings(ExposedState.Exposable)]
    public class GameObjectAnimation : ComponentBase, IGameObjectAnimation
    {
        private byte animationState;

        public void SetAnimationState(byte animationState)
        {
            this.animationState = animationState;
        }

        public byte GetAnimationState()
        {
            return animationState;
        }
    }
}