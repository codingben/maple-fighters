using System;
using MathematicsHelper;
using ServerApplication.Common.ComponentModel;

namespace Game.InterestManagement
{
    public class Transform : Component<IGameObject>
    {
        public Vector2 Position { get; private set; } = Vector2.Zero;

        public Action<Vector2> PositionChanged = delegate {  };

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