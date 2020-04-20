using Box2DX.Dynamics;

namespace Physics.Box2D.Core
{
    public interface IPhysicsCollisionCallback
    {
        void OnCollisionEnter(Body otherBody);

        void OnCollisionExit(Body otherBody);
    }
}