using Box2DX.Dynamics;

namespace Physics.Box2D
{
    public interface IWorldProvider
    {
        World Provide();
    }
}