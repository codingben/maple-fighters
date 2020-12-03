using Box2DX.Dynamics;
using ComponentModel.Common;
using Physics.Box2D.Components.Interfaces;

namespace Physics.Box2D.Components
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