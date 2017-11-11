using Box2DX.Dynamics;
using ComponentModel.Common;

namespace Physics.Box2D
{
    public interface IPhysicsWorldProvider : IExposableComponent
    {
        World GetWorld();
    }
}