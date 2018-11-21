using Common.MathematicsHelper;

namespace Game.InterestManagement
{
    public struct SceneBounds
    {
        private readonly Vector2 upperBound;
        private readonly Vector2 lowerBound;

        public SceneBounds(Vector2 upperBound, Vector2 lowerBound)
        {
            this.upperBound = upperBound;
            this.lowerBound = lowerBound;
        }

        public bool IsInsideBounds(Vector2 position)
        {
            return position.X < upperBound.X && position.Y < upperBound.Y &&
                   position.X > lowerBound.X && position.Y > lowerBound.Y;
        }
    }
}