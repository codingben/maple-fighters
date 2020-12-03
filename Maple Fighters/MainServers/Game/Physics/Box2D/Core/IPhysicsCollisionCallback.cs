namespace Physics.Box2D.Core
{
    public interface IPhysicsCollisionCallback
    {
        void OnCollisionEnter(CollisionInfo collisionInfo);
        void OnCollisionExit(CollisionInfo collisionInfo);
    }
}