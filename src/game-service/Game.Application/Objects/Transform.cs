using System;
using InterestManagement;

namespace Game.Application.Objects
{
    public class Transform : ITransform
    {
        public Vector2 Position { get; private set; }

        public Vector2 Size { get; private set; }

        public Vector2 Direction { get; private set; }

        public event Action PositionChanged;

        public Transform()
        {
            Position = Vector2.Zero;
            Size = Vector2.Zero;
        }

        public void SetPosition(Vector2 position)
        {
            Position = position;

            PositionChanged?.Invoke();
        }

        public void SetSize(Vector2 size)
        {
            Size = size;
        }

        public void SetDirection(Vector2 direction)
        {
            Direction = direction;
        }
    }
}