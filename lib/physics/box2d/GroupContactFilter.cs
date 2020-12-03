using Box2DX.Dynamics;

namespace Physics.Box2D
{
    public class GroupContactFilter : ContactFilter
    {
        public override bool ShouldCollide(Fixture fixtureA, Fixture fixtureB)
        {
            return fixtureA.Filter.GroupIndex != fixtureB.Filter.GroupIndex;
        }
    }
}