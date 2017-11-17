using ComponentModel.Common;

namespace Physics.Box2D
{
    public interface IPhysicsWorldSimulation : IExposableComponent
    {
        void StartSimulateWorldContinuously();
    }
}