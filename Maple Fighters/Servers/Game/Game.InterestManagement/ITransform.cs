using System;
using ComponentModel.Common;
using MathematicsHelper;
using Shared.Game.Common;

namespace Game.InterestManagement
{
    public interface ITransform : IExposableComponent
    {
        Vector2 Position { get; }

        event Action<Vector2> PositionChanged;
        event Action<Vector2, Directions> PositionAndDirectionChanged;

        void SetPosition(Vector2 position);
        void SetPosition(Vector2 position, Directions direction);
    }
}