using System;
using Game.InterestManagement;

namespace Assets.Scripts.Game
{
    using Vector2 = Common.MathematicsHelper.Vector2;

    public class GameTransform : ITransform
    {
        public event Action PositionChanged;

        public Vector2 Position { get; private set; }

        public Vector2 Size { get; private set; }

        public GameTransform(Vector2 position, Vector2 size)
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