using Box2DX.Dynamics;
using MathematicsHelper;

namespace Physics.Box2D
{
    public struct CollisionInfo
    {
        public readonly Body Body;
        public readonly Vector2 Position;
        public readonly Vector2 Velocity;
        public readonly Vector2 Normal;

        public CollisionInfo(Body body, Vector2 position, Vector2 velocity, Vector2 normal)
        {
            Body = body;
            Position = position;
            Velocity = velocity;
            Normal = normal;
        }
    }
}