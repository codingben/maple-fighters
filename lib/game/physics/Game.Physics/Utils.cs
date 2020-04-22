using Box2DX.Dynamics;
using Common.MathematicsHelper;

namespace Game.Physics
{
    public static class Utils
    {
        public static BodyDef CreateBodyDefinition(
            Vector2 position,
            object userData = null)
        {
            var bodyDefinition = new BodyDef
            {
                UserData = userData,
                FixedRotation = true
            };
            bodyDefinition.Position.Set(position.X, position.Y);

            return bodyDefinition;
        }

        public static PolygonDef CreatePolygonDefinition(
            Vector2 size,
            short groupIndex,
            object userData = null,
            float density = 1.0f,
            float friction = 4.0f)
        {
            var polygonDefinition = new PolygonDef
            {
                UserData = userData,
                Density = density,
                Friction = friction,
                Filter = new FilterData
                {
                    GroupIndex = groupIndex
                }
            };
            polygonDefinition.SetAsBox(size.X, size.Y);

            return polygonDefinition;
        }

        public static void CreateBox(
            World world,
            Vector2 position,
            Vector2 size,
            short groupIndex,
            float density = 0.0f)
        {
            var bodyDefinition = new BodyDef();
            bodyDefinition.Position.Set(position.X, position.Y);

            var polygonDefinition = new PolygonDef();
            polygonDefinition.SetAsBox(size.X, size.Y);
            polygonDefinition.Density = density;
            polygonDefinition.Filter = new FilterData
            {
                GroupIndex = groupIndex
            };

            var body = world.CreateBody(bodyDefinition);
            body.CreateFixture(polygonDefinition);
            body.SetMassFromShapes();
        }

        public static Body CreateBody(
            World world,
            BodyDef bodyDefinition,
            PolygonDef polygonDefinition)
        {
            var body = world.CreateBody(bodyDefinition);
            body.SetUserData(bodyDefinition.UserData);
            body.CreateFixture(polygonDefinition);
            body.SetMassFromShapes();

            return body;
        }

        public static void MoveBody(
            Body body, 
            Vector2 position, 
            float speed)
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