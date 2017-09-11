using System;
using MathematicsHelper;
using ServerApplication.Common.ComponentModel;

namespace Game.InterestManagement
{
    public class Transform : Component<IGameObject>
    {
        public Vector2 Position { get; private set; } = Vector2.Zero;
        private Vector2 lastPosition = Vector2.Zero;

        public Action<Vector2> PositionChanged = delegate {  };

        public Transform()
        {
            // Left blank intentionally
        }

        public Transform(Vector2 startPosition)
        {
            Position = startPosition;
            lastPosition = startPosition;
        }

        public void SetPosition(Vector2 position)
        {
            Position = position;

            if (Vector2.Distance(Position, lastPosition) < 0.1f)
            {
                return;
            }

            PositionChanged?.Invoke(position);

            lastPosition = position;
        }
    }
}