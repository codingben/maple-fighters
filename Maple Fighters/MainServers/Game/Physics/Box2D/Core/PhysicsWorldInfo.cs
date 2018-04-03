using MathematicsHelper;

namespace Physics.Box2D.Core
{
    public struct PhysicsWorldInfo
    {
        public readonly Vector2 LowerBound;
        public readonly Vector2 UpperBound;
        public readonly Vector2 Gravity;
        public readonly bool DoSleep;

        public PhysicsWorldInfo(Vector2 lowerBound, Vector2 upperBound, Vector2 gravity, bool doSleep = true)
        {
            LowerBound = lowerBound;
            UpperBound = upperBound;
            Gravity = gravity;
            DoSleep = doSleep;
        }
    }
}