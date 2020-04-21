using Box2DX.Dynamics;

namespace Game.Physics
{
    public class GroupContactFilter : ContactFilter
    {
        public override bool ShouldCollide(Fixture fixtureA, Fixture fixtureB)
        {
            if (fixtureA.Filter.GroupIndex == (short)LayerMask.Ground)
            {
                return false;
            }

            if (fixtureB.Filter.GroupIndex == (short)LayerMask.Ground)
            {
                return false;
            }

            return fixtureA.Filter.GroupIndex != fixtureB.Filter.GroupIndex;
        }
    }
}