using System;
using ComponentModel.Common;
using MathematicsHelper;
using Shared.Game.Common;

namespace Game.InterestManagement
{
    public class Transform : Component<ISceneObject>, ITransform
    {
        public Vector2 Position { get; private set; } = Vector2.Zero;
        public Vector2 InitialPosition { get; set; }

        public Directions Direction { get; set; }

        private Vector2 lastPosition = Vector2.Zero;

        public event Action<Vector2, Directions> PositionChanged;
        public event Action<Vector2> PositionChangedOnly;

        public Transform()
        {
            // Left blank intentionally
        }

        public Transform(Vector2 startPosition, Directions direction)
        {
            InitialPosition = startPosition;
            Direction = direction;
            Position = startPosition;
            lastPosition = startPosition;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            PositionChanged = null;
            PositionChangedOnly = null;
        }

        public void SetPosition(Vector2 position, Directions direction)
        {
            Position = position;

            if (Vector2.Distance(Position, lastPosition) < 0.1f)
            {
                return;
            }

            PositionChanged?.Invoke(position, direction);
            PositionChangedOnly?.Invoke(position);

            lastPosition = position;
        }
    }
}