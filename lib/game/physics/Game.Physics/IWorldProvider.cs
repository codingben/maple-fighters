using Box2DX.Dynamics;

namespace Game.Physics
{
    public interface IWorldProvider
    {
        World Provide();
    }
}