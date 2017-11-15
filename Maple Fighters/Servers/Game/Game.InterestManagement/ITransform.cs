using System;
using ComponentModel.Common;
using MathematicsHelper;
using Shared.Game.Common;

namespace Game.InterestManagement
{
    public interface ITransform : IExposableComponent
    {
        Vector2 InitialPosition { get; set; }
        Vector2 Position { get; }

        Directions Direction { get; set; }

        event Action<Vector2> PositionChanged;
        event Action<Vector2, Directions> PositionDirectionChanged;

        void SetPosition(Vector2 position);
        void SetPosition(Vector2 position, Directions direction);
    }
}