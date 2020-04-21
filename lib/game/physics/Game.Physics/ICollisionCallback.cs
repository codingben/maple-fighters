using Box2DX.Dynamics;

namespace Physics.Box2D
{
    public interface ICollisionCallback
    {
        void OnCollisionEnter(Body otherBody);

        void OnCollisionExit(Body otherBody);
    }
}