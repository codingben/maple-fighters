using Box2DX.Collision;
using Box2DX.Dynamics;
using ComponentModel.Common;
using Game.InterestManagement;
using MathematicsHelper;

namespace Physics.Box2D
{
    public class PhysicsWorldCreator : Component<ISceneEntity>
    {
        private readonly World world;

        public PhysicsWorldCreator(Vector2 lowerBound, Vector2 upperBound, Vector2 gravity, bool doSleep = true)
        {
            var worldAabb = new AABB
            {
                LowerBound = lowerBound.FromVector2(),
                UpperBound = upperBound.FromVector2()
            };
            world = new World(worldAabb, gravity.FromVector2(), doSleep);
            world.CreateGround(new Vector2(0.0f, -2.85f), new Vector2(20.30312f, 0.25f)); // Map_1 // TODO: Get this data from another source
        }

        protected override void OnAwake()
        {
            base.OnAwake();

            Entity.Container.AddComponent(new PhysicsWorldProvider(world));
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            world.Dispose();
        }
    }
}