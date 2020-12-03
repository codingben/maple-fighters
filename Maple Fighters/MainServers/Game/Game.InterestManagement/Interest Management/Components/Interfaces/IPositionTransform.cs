using MathematicsHelper;

namespace InterestManagement.Components.Interfaces
{
    public interface IPositionTransform
    {
        Vector2 Position { get; set; }
        void SetPosition(Vector2 position);
    }
}