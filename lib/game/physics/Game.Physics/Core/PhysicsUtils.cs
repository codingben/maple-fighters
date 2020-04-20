using Box2DX.Dynamics;
using Common.MathematicsHelper;

namespace Physics.Box2D.Core
{
    public static class PhysicsUtils
    {
        public static void CreateGround(
            World world,
            Vector2 position,
            Vector2 size)
        {
            var bodyDef = new BodyDef();
            bodyDef.Position.Set(position.X, position.Y);

            var boxDef = new PolygonDef();
            boxDef.SetAsBox(size.X, size.Y);
            boxDef.Density = 0.0f;
            boxDef.Filter = new FilterData
            {
                GroupIndex = (short)LayerMask.Ground
            };

            var body = world.CreateBody(bodyDef);
            body.CreateFixture(boxDef);
            body.SetMassFromShapes();
        }

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

        public static PolygonDef CreateFixtureDefinition(
            Vector2 size,
            LayerMask layerMask,
            object userData = null)
        {
            var polygonDefinition = new PolygonDef
            {
                UserData = userData,
                Density = 1.0f,
                Friction = 4.0f,
                Filter = new FilterData
                {
                    GroupIndex = (short)layerMask
                }
            };
            polygonDefinition.SetAsBox(size.X, size.Y);

            return polygonDefinition;
        }

        public static Body CreateCharacter(
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