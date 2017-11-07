using System;
using ComponentModel.Common;
using MathematicsHelper;
using Shared.Game.Common;

namespace Game.InterestManagement
{
    public class Transform : Component<ISceneObject>, ITransform
    {
        public Vector2 InitialPosition { get; set; }
        public Vector2 Position { get; private set; } = Vector2.Zero;
        public Directions Direction { get; set; }

        private Vector2 lastPosition = Vector2.Zero;

        public event Action<Vector2> PositionChanged;
        public event Action<Vector2, Directions> PositionAndDirectionChanged;

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
            PositionAndDirectionChanged = null;
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

        public void SetPosition(Vector2 position, Directions direction)
        {
            Position = position;

            if (Vector2.Distance(Position, lastPosition) < 0.1f)
            {
                return;
            }

            PositionChanged?.Invoke(position);
            PositionAndDirectionChanged?.Invoke(position, direction);

            lastPosition = position;
        }
    }
}