namespace Physics.Box2D
{
    public interface IPhysicsCollision
    {
        void OnCollisionEnter(CollisionInfo body);
        void OnCollisionExit(CollisionInfo body);
    }
}