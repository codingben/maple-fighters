using Common.MathematicsHelper;

namespace Physics.Box2D.Core
{
    public struct PhysicsWorldInfo
    {
        public Vector2 LowerBound { get; }

        public Vector2 UpperBound { get; }

        public Vector2 Gravity { get; }

        public bool DoSleep { get; }

        public PhysicsWorldInfo(
            Vector2 lowerBound,
            Vector2 upperBound,
            Vector2 gravity,
            bool doSleep = true)
        {
            LowerBound = lowerBound;
            UpperBound = upperBound;
            Gravity = gravity;
            DoSleep = doSleep;
        }
    }
}