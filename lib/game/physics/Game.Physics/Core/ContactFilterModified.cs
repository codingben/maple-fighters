using Box2DX.Dynamics;

namespace Physics.Box2D.Core
{
    public class ContactFilterModified : ContactFilter
    {
        public override bool ShouldCollide(Fixture fixtureA, Fixture fixtureB)
        {
            return fixtureA.Filter.GroupIndex != fixtureB.Filter.GroupIndex;
        }
    }
}