using Box2DX.Dynamics;

namespace Physics.Box2D
{
    public interface IPhysicsWorldProvider
    {
        World GetWorld();
    }
}