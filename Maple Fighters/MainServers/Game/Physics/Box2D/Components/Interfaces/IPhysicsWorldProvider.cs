using Box2DX.Dynamics;

namespace Physics.Box2D.Components.Interfaces
{
    public interface IPhysicsWorldProvider
    {
        World GetWorld();
    }
}