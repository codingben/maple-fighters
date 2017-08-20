using MathematicsHelper;

namespace Game.InterestManagement
{
    public struct Boundaries
    {
        public Vector2 TopLeft { get; }
        public Vector2 BottomRight { get; }

        public Boundaries(Vector2 topLeft, Vector2 bottomRight)
        {
            TopLeft = topLeft;
            BottomRight = bottomRight;
        }
    }
}