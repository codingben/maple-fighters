namespace Game.Application.Objects.Components
{
    public interface IGameObjectAnimation
    {
        void SetAnimationState(byte animationState);

        byte GetAnimationState();
    }
}