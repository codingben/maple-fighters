using Box2DX.Common;
using Box2DX.Dynamics;
using Common.MathematicsHelper;

namespace Physics.Box2D.Core
{
    public static class PhysicsUtils
    {
        public const float UpdatesPerSecond = 30.0f;
        public const float FramesPerSecond = 30.0f;
        public const float TeleportDistance = 1.0f;

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
            this World world,
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
            float speed,
            bool teleport = true)
        {
            var direction = position - body.GetPosition().ToVector2();
            var distanceToTravel = direction.FromVector2().Normalize();

            if (teleport && (distanceToTravel > TeleportDistance))
            {
                body.SetXForm(position.FromVector2(), body.GetAngle());
            }
            else
            {
                var distancePerTimestep = speed / FramesPerSecond;
                if (distancePerTimestep > distanceToTravel)
                {
                    speed *= distanceToTravel / distancePerTimestep;
                }

                var desiredVelocity = speed * direction;
                var changeInVelocity = desiredVelocity - body.GetLinearVelocity().ToVector2();
                var force = body.GetMass() * FramesPerSecond * changeInVelocity;

                body.ApplyForce(force.FromVector2(), body.GetWorldCenter());
            }
        }

        public static Vector2 ToVector2(this Vec2 vec2)
        {
            return new Vector2(vec2.X, vec2.Y);
        }

        public static Vec2 FromVector2(this Vector2 vector2)
        {
            return new Vec2(vector2.X, vector2.Y);
        }
    }
}