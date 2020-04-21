using System;
using Box2DX.Collision;
using Box2DX.Dynamics;
using Common.MathematicsHelper;
using Physics.Box2D.Core;

namespace Physics.Box2D.Components
{
    public class PhysicsWorldSimulation : IDisposable
    {
        private World world;

        public void CreateWorldSimulation(
            Vector2 lowerBound,
            Vector2 upperBound,
            Vector2 gravity,
            bool doSleep = true,
            bool continuousPhysics = false)
        {
            var worldAabb = new AABB
            {
                LowerBound = lowerBound.FromVector2(),
                UpperBound = upperBound.FromVector2()
            };

            world = new World(worldAabb, gravity.FromVector2(), doSleep);
            world.SetContactFilter(new GroupContactFilter());
            world.SetContactListener(new BodyContactListener());
            world.SetContinuousPhysics(continuousPhysics);
        }

        public World GetWorld()
        {
            return world;
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