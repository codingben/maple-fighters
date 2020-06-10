using System;
using Common.MathematicsHelper;
using InterestManagement;

namespace Game.Application.Objects
{
    public class Transform : ITransform
    {
        public Vector2 Position { get; private set; }

        public Vector2 Size { get; private set; }

        public event Action PositionChanged;

        public void SetPosition(Vector2 position)
        {
            Position = position;

            PositionChanged?.Invoke();
        }

        public void SetSize(Vector2 size)
        {
            Size = size;
        }
    }
}