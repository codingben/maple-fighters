using System;
using ComponentModel.Common;
using MathematicsHelper;

namespace Game.InterestManagement
{
    public interface ITransform : IExposableComponent
    {
        Vector2 Position { get; set; }
        Vector2 Size { get; }

        Direction Direction { get; set; }

        event Action<Vector2> PositionChanged;

        void SetPosition(Vector2 position);
    }
}