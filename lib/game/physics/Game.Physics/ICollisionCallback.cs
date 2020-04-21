using Box2DX.Dynamics;

namespace Game.Physics
{
    public interface ICollisionCallback
    {
        void OnCollisionEnter(Body otherBody);

        void OnCollisionExit(Body otherBody);
    }
}