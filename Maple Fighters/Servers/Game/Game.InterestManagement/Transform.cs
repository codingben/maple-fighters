using System;
using ComponentModel.Common;
using MathematicsHelper;

namespace Game.InterestManagement
{
    public class Transform : Component<ISceneObject>, ITransform
    {
        public Vector2 Position { get; set; }
        public Vector2 Size { get; }
        public Direction Direction { get; set; }

        public event Action<Vector2> PositionChanged;

        public Transform()
        {
            // Left blank intentionally
        }

        public Transform(Vector2 position, Vector2 size, Direction direction)
        {
            Position = position;
            Size = size;
            Direction = direction;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            PositionChanged = null;
        }

        public void SetPosition(Vector2 position)
        {
            Position = position;

            PositionChanged?.Invoke(position);
        }
    }
}