using MathematicsHelper;

namespace Game.InterestManagement
{
    public struct TransformDetails
    {
        public Vector2 Position { get; }
        public Vector2 Size { get; }
        public Direction Direction { get; }

        public TransformDetails(Vector2 position, Vector2 size, Direction direction)
        {
            Position = position;
            Size = size;
            Direction = direction;
        }
    }
}