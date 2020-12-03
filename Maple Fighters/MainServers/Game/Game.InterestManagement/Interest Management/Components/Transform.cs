using System;
using ComponentModel.Common;
using InterestManagement.Components.Interfaces;
using MathematicsHelper;

namespace InterestManagement.Components
{
    public class Transform : Component<ISceneObject>, IPositionTransform, IPositionChangesNotifier, ISizeTransform, IDirectionTransform
    {
        public event Action<Vector2> PositionChanged;

        public Vector2 Position { get; set; }
        public Vector2 Size { get; }
        public Direction Direction { get; private set; }

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

        public void SetPosition(Vector2 position)
        {
            Position = position;

            PositionChanged?.Invoke(position);
        }

        public void SetDirection(Direction direction)
        {
            Direction = direction;
        }
    }
}