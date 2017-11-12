using Box2DX.Collision;
using Box2DX.Dynamics;

namespace Physics.Box2D
{
    public class ContactFilterModified : ContactFilter
    {
        public override bool ShouldCollide(Shape shape1, Shape shape2)
        {
            var filterData1 = shape1.FilterData;
            var filterData2 = shape2.FilterData;
            return filterData1.GroupIndex != filterData2.GroupIndex;
        }
    }
}