using System;
using Box2DX.Collision;
using Box2DX.Dynamics;
using Common.MathematicsHelper;

namespace Game.Physics
{
    public class WorldWrapper : IDisposable
    {
        private readonly World world;

        public WorldWrapper(
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

        public void Dispose()
        {
            world.SetDebugDraw(null);
            world.Dispose();
        }
    }
}