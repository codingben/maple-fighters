using System;
using Box2DX.Collision;
using Box2DX.Dynamics;
using Physics.Box2D.Core;

namespace Physics.Box2D.Components
{
    public class PhysicsWorldSimulation : IDisposable
    {
        private readonly World world;

        public PhysicsWorldSimulation(PhysicsWorldInfo worldInfo)
        {
            var worldAabb = new AABB
            {
                LowerBound = worldInfo.LowerBound.FromVector2(),
                UpperBound = worldInfo.UpperBound.FromVector2()
            };
            var gravity = worldInfo.Gravity.FromVector2();
            var doSleep = worldInfo.DoSleep;

            world = new World(worldAabb, gravity, doSleep);
            world.SetContactFilter(new GroupContactFilter());
            world.SetContactListener(new BodyContactListener());
            world.SetContinuousPhysics(false);
        }

        public void SimulateWorld()
        {
            var timeStep = PhysicsSettings.TimeStep;
            var velocityIterations = PhysicsSettings.VelocityIterations;
            var positionIterations = PhysicsSettings.PositionIterations;

            world.Step(timeStep, velocityIterations, positionIterations);
        }

        public void Dispose()
        {
            world.SetDebugDraw(null);
            world.Dispose();
        }
    }
}