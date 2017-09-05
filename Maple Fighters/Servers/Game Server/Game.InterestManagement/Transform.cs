using System;
using MathematicsHelper;

namespace Game.InterestManagement
{
    public class Transform : GameObjectComponent
    {
        public Vector2 Position { get; private set; } = Vector2.Zero;

        public event Action<Vector2> PositionChanged;

        public Transform()
        {
            // Left blank intentionally
        }

        public Transform(Vector2 startPosition)
        {
            Position = startPosition;
        }

        public void SetPosition(Vector2 position)
        {
            Position = position;

            PositionChanged?.Invoke(position);
        }
    }
}