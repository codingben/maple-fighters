using System;
using MathematicsHelper;

namespace InterestManagement.Components.Interfaces
{
    public interface IPositionTransform
    {
        event Action<Vector2> PositionChanged;

        Vector2 Position { get; set; }
        void SetPosition(Vector2 position);
    }
}