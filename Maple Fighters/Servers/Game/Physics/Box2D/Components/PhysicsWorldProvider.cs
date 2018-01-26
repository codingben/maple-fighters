using Box2DX.Dynamics;
using ComponentModel.Common;

namespace Physics.Box2D
{
    public class PhysicsWorldProvider : Component, IPhysicsWorldProvider
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