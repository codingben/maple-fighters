using Box2DX.Dynamics;
using Common.MathematicsHelper;

namespace Physics.Box2D
{
    public static class BodyUtils
    {
        public static void MoveBody(Body body, Vector2 position, float speed)
        {
            var direction = position - body.GetPosition().ToVector2();
            var distanceToTravel = direction.FromVector2().Normalize();
            if (distanceToTravel > PhysicsSettings.MaxTravelDistance)
            {
                body.SetXForm(position.FromVector2(), body.GetAngle());
            }
            else
            {
                var distancePerTimestep =
                    speed / PhysicsSettings.FramesPerSecond;
                if (distancePerTimestep > distanceToTravel)
                {
                    speed *= distanceToTravel / distancePerTimestep;
                }

                var desiredVelocity = speed * direction;
                var changeInVelocity =
                    desiredVelocity - body.GetLinearVelocity().ToVector2();
                var force =
                    body.GetMass() * PhysicsSettings.FramesPerSecond * changeInVelocity;

                body.ApplyForce(force.FromVector2(), body.GetWorldCenter());
            }
        }
    }
}