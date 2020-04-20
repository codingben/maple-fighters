using Box2DX.Dynamics;
using Common.MathematicsHelper;

namespace Physics.Box2D.Core
{
    public struct CollisionInfo
    {
        public Body Body { get; }

        public Vector2 Position { get; }

        public Vector2 Velocity { get; }

        public Vector2 Normal { get; }

        public CollisionInfo(
            Body body,
            Vector2 position,
            Vector2 velocity,
            Vector2 normal)
        {
            Body = body;
            Position = position;
            Velocity = velocity;
            Normal = normal;
        }
    }
}