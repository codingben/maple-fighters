using Game.Physics;
using InterestManagement;

namespace Game.Application.Components
{
    public class ScenePhysicsCreator : ComponentBase, IScenePhysicsCreator
    {
        private IPhysicsWorldManager physicsWorldManager;
        private IPhysicsExecutor physicsExecutor;

        protected override void OnAwake()
        {
            var lowerBound = new Vector2(-100, -100);
            var upperBound = new Vector2(100, 100);
            var gravity = new Vector2(0, 0 /* -9.81f */);

            physicsWorldManager = new PhysicsWorldManager(lowerBound, upperBound, gravity);
            physicsExecutor = new PhysicsExecutor(OnUpdateBodies, OnSimulatePhysics);
        }

        protected override void OnRemoved()
        {
            physicsWorldManager?.Dispose();
            physicsExecutor?.Dispose();
        }

        private void OnUpdateBodies()
        {
            physicsWorldManager?.UpdateBodies();
        }

        private void OnSimulatePhysics()
        {
            var timeStep = PhysicsSettings.TimeStep;
            var velocityIterations = PhysicsSettings.VelocityIterations;
            var positionIterations = PhysicsSettings.PositionIterations;

            physicsWorldManager?.Step(timeStep, velocityIterations, positionIterations);
        }

        public IPhysicsWorldManager GetPhysicsWorldManager()
        {
            return physicsWorldManager;
        }

        public IPhysicsExecutor GetPhysicsExecutor()
        {
            return physicsExecutor;
        }
    }
}