using MathematicsHelper;

namespace InterestManagement
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

        public static TransformDetails Empty()
        {
            return new TransformDetails(Vector2.Zero, Vector2.Zero, Direction.Left);
        }
    }
}