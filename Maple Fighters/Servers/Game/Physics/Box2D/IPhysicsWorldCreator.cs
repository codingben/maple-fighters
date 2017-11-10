using Box2DX.Dynamics;
using ComponentModel.Common;

namespace Physics.Box2D
{
    public interface IPhysicsWorldCreator : IExposableComponent
    {
        World GetWorld();
    }
}