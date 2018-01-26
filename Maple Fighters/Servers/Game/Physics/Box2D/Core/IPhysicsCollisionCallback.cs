namespace Physics.Box2D
{
    public interface IPhysicsCollisionCallback
    {
        void OnCollisionEnter(CollisionInfo collisionInfo);
        void OnCollisionExit(CollisionInfo collisionInfo);
    }
}