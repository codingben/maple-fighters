namespace InterestManagement.Components.Interfaces
{
    public interface IDirectionTransform
    {
        Direction Direction { get; }
        void SetDirection(Direction direction);
    }
}