using Box2DX.Collision;
using Box2DX.Dynamics;
using ComponentModel.Common;
using Game.InterestManagement;
using MathematicsHelper;

namespace Physics.Box2D
{
    public class PhysicsWorldCreator : Component<ISceneEntity>, IPhysicsWorldCreator
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
            world.CreateGround(new Vector2(0.0f, -10.0f), new Vector2(50.0f, 10.0f)); // TODO: Remove
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            world.Dispose();
        }

        public World GetWorld()
        {
            return world;
        }
    }
}