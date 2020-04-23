using Box2DX.Dynamics;

namespace Game.Physics
{
    public class GroupContactFilter : ContactFilter
    {
        public override bool ShouldCollide(Fixture fixtureA, Fixture fixtureB)
        {
            return fixtureA.Filter.GroupIndex != fixtureB.Filter.GroupIndex;
        }
    }
}