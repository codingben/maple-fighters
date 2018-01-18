using System.Collections.Generic;
using Box2DX.Collision;
using Box2DX.Dynamics;
using CommonTools.Coroutines;
using CommonTools.Log;
using ComponentModel.Common;

namespace Physics.Box2D
{
    public class PhysicsWorldSimulationCreator : Component
    {
        private readonly World world;

        public PhysicsWorldSimulationCreator(PhysicsWorldInfo worldInfo)
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
        }

        protected override void OnAwake()
        {
            base.OnAwake();

            var executor = Entity.GetComponent<ISceneOrderExecutor>().AssertNotNull();
            executor.GetUpdateExecutor().StartCoroutine(SimulateWorld());

            Entity.AddComponent(new PhysicsWorldProvider(world));
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
    }
}