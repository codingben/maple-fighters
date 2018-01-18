using System;
using ComponentModel.Common;
using MathematicsHelper;

namespace Game.InterestManagement
{
    public interface ITransform : IExposableComponent
    {
        Vector2 Position { get; set; }
        event Action<Vector2> PositionChanged;

        void SetPosition(Vector2 position);
    }
}