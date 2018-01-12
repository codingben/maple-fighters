using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Box2DX.Collision;
using Box2DX.Dynamics;
using CommonTools.Coroutines;
using CommonTools.Log;
using ComponentModel.Common;
using Game.InterestManagement;
using MathematicsHelper;
using ServiceStack;
using Shared.Game.Common;

namespace Physics.Box2D
{
    public class PhysicsWorldSimulation : Component<ISceneEntity>
    {
        private readonly World world;

        public PhysicsWorldSimulation(Maps map, PhysicsWorldInfo worldInfo)
        {
            var worldAabb = new AABB
            {
                LowerBound = worldInfo.LowerBound.FromVector2(),
                UpperBound = worldInfo.UpperBound.FromVector2()
            };
            world = new World(worldAabb, worldInfo.Gravity.FromVector2(), worldInfo.DoSleep);
            world.SetContactFilter(new ContactFilterModified());
            world.SetContactListener(new ContactListenerModified());
            world.SetContinuousPhysics(false);

            CreateScenePhysicsData(map);
        }

        protected override void OnAwake()
        {
            base.OnAwake();

            var executor = Entity.Container.GetComponent<ISceneOrderExecutor>().AssertNotNull();
            executor.GetUpdateExecutor().StartCoroutine(SimulateWorld());

            Entity.Container.AddComponent(new PhysicsWorldProvider(world));
        }

        private IEnumerator<IYieldInstruction> SimulateWorld()
        {
            while (true)
            {
                // Prepare for simulation. Typically we use a time step of 1/60 of a
                // second (60Hz) and 10 iterations. This provides a high quality simulation
                // in most game scenarios.
                const float TIME_STEP = 1.0f / 30.0f;
                const int VELOCITY_ITERATIONS = 8;
                const int POSITION_ITERATIONS = 3;

                // Instruct the world to perform a single step of simulation. It is
                // generally best to keep the time step and iterations fixed.
                world.Step(TIME_STEP, VELOCITY_ITERATIONS, POSITION_ITERATIONS);
                yield return null;
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            world.Dispose();
        }

        private void CreateScenePhysicsData(Maps map)
        {
            var path = $"python/scenes/{map}/ScenePhysicsData.json";
            if (!File.Exists(path))
            {
                LogUtils.Log($"Could not find ScenePhysicsData json file for {map}.");
                return;
            }

            var json = File.ReadAllText(path);
            var scenePhysicsData = DynamicJson.Deserialize(json);

            foreach (var groundCollider in scenePhysicsData.GroundColliders)
            {
                world.CreateGround(
                    new Vector2(
                        float.Parse(groundCollider.Position.X, CultureInfo.InvariantCulture.NumberFormat),
                        float.Parse(groundCollider.Position.Y, CultureInfo.InvariantCulture.NumberFormat)),
                    new Vector2(
                        float.Parse(groundCollider.Extents.X, CultureInfo.InvariantCulture.NumberFormat),
                        float.Parse(groundCollider.Extents.Y, CultureInfo.InvariantCulture.NumberFormat))
                );
            }
        }
    }
}