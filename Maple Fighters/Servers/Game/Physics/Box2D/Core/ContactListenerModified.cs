using Box2DX.Collision;
using Box2DX.Dynamics;

namespace Physics.Box2D
{
    public class ContactListenerModified : ContactListener
    {
        public override void Add(ContactPoint point)
        {
            base.Add(point);

            if (!CanContact(point.Shape1.FilterData, point.Shape2.FilterData))
            {
                return;
            }

            var userData1 = point.Shape1.UserData as IPhysicsCollisionCallback;
            var userData2 = point.Shape2.UserData as IPhysicsCollisionCallback;

            userData1?.OnCollisionEnter(new CollisionInfo(point.Shape2.GetBody(), point.Position.ToVector2(), point.Velocity.ToVector2(), point.Normal.ToVector2()));
            userData2?.OnCollisionEnter(new CollisionInfo(point.Shape1.GetBody(), point.Position.ToVector2(), point.Velocity.ToVector2(), point.Normal.ToVector2()));
        }

        public override void Remove(ContactPoint point)
        {
            base.Remove(point);

            if (!CanContact(point.Shape1.FilterData, point.Shape2.FilterData))
            {
                return;
            }

            var userData1 = point.Shape1.UserData as IPhysicsCollisionCallback;
            var userData2 = point.Shape2.UserData as IPhysicsCollisionCallback;

            userData1?.OnCollisionExit(new CollisionInfo(point.Shape2.GetBody(), point.Position.ToVector2(), point.Velocity.ToVector2(), point.Normal.ToVector2()));
            userData2?.OnCollisionExit(new CollisionInfo(point.Shape1.GetBody(), point.Position.ToVector2(), point.Velocity.ToVector2(), point.Normal.ToVector2()));
        }

        /// <summary>
        /// Disallow collision with fixtures with same layer mask and with ground.
        /// </summary>
        private bool CanContact(FilterData filterData1, FilterData filterData2)
        {
            if (filterData1.GroupIndex == (short)LayerMask.Ground
                || filterData2.GroupIndex == (short)LayerMask.Ground)
            {
                return false;
            }

            return filterData1.GroupIndex != filterData2.GroupIndex;
        }
    }
}