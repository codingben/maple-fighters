using System.Net;
using Game.Physics;
using InterestManagement;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Game.Application.Components
{
    public class ScenePhysicsCreator : ComponentBase, IScenePhysicsCreator
    {
        private IPhysicsWorldManager physicsWorldManager;
        private IPhysicsExecutor physicsExecutor;

        protected override void OnAwake()
        {
            var yamlConfig = LoadYamlConfig();
            var configData = ParseConfigData(yamlConfig);
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

        private PhysicsData ParseConfigData(string data)
        {
            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();

            return deserializer.Deserialize<PhysicsData>(data);
        }

        private string LoadYamlConfig()
        {
            var config = string.Empty;
            var url = "https://raw.githubusercontent.com/benukhanov/maple-fighters-configs/main/{0}";
            var yamlPath = string.Format(url, "physics.yml");

            using (var client = new WebClient())
            {
                config = client.DownloadString(yamlPath);
            }

            return config;
        }
    }
}