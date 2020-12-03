using System;
using MathematicsHelper;

namespace InterestManagement.Components.Interfaces
{
    public interface IPositionChangesNotifier
    {
        event Action<Vector2> PositionChanged;
    }
}