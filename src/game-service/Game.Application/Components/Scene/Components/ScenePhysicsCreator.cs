using Game.Physics;
using InterestManagement;
using Utilities;

namespace Game.Application.Components
{
    public class ScenePhysicsCreator : ComponentBase, IScenePhysicsCreator
    {
        private IPhysicsWorldManager physicsWorldManager;
        private IPhysicsExecutor physicsExecutor;

        protected override void OnAwake()
        {
            var yamlConfigUrl = ConfigUtils.GetYamlConfigUrl(configFile: "physics.yml");
            var yamlConfig = ConfigUtils.LoadYamlConfig(url: yamlConfigUrl);
            var configData = ConfigUtils.ParseConfigData<PhysicsData>(yamlConfig);
            var upperBound = new Vector2(configData.UpperBound.X, configData.UpperBound.Y);
            var lowerBound = new Vector2(configData.LowerBound.X, configData.LowerBound.Y);
            var gravity = new Vector2(configData.Gravity.X, configData.Gravity.Y);

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