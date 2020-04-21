using Box2DX.Dynamics;

namespace Physics.Box2D.Components.Interfaces
{
    public interface IWorldProvider
    {
        World GetWorld();
    }
}