namespace Game.Application.Objects.Components
{
    public interface IAnimationData
    {
        void SetAnimationState(byte animationState);

        byte GetAnimationState();
    }
}