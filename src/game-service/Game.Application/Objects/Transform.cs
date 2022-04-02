using System;
using InterestManagement;

namespace Game.Application.Objects
{
    public class Transform : ITransform
    {
        public Vector2 Position { get; private set; }

        public Vector2 Size { get; private set; }

        public event Action PositionChanged;

        public Transform(Vector2 position, Vector2 size)
        {
            Position = position;
            Size = size;
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
    }
}