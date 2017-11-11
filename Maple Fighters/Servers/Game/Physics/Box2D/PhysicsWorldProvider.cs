using Box2DX.Dynamics;
using ComponentModel.Common;
using Game.InterestManagement;

namespace Physics.Box2D
{
    public class PhysicsWorldProvider : Component<ISceneEntity>, IPhysicsWorldProvider
    {
        private readonly World world;

        public PhysicsWorldProvider(World world)
        {
            this.world = world;
        }

        public World GetWorld()
        {
            return world;
        }
    }
}