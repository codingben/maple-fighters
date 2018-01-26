using Box2DX.Collision;
using Box2DX.Common;
using Box2DX.Dynamics;
using MathematicsHelper;

namespace Physics.Box2D
{
    public static class PhysicsUtils
    {
        public const float UPDATES_PER_SECOND = 30.0f;
        public const float FRAMES_PER_SECOND = 30.0f;

        private static readonly object locker = new object();

        public static void CreateGround(this World world, Vector2 position, Vector2 size)
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
            body.CreateShape(boxDef);
            body.SetMassFromShapes();
        }

        public static BodyDefinitionWrapper CreateBodyDefinitionWrapper(PolygonDef fixture, Vector2 position, object userData = null)
        {
            var bodyDefinition = new BodyDef();
            bodyDefinition.Position.Set(position.X, position.Y);
            bodyDefinition.FixedRotation = true;

            var bodyDefinitionWrapper = new BodyDefinitionWrapper(bodyDefinition, fixture, userData);
            return bodyDefinitionWrapper;
        }

        public static PolygonDef CreateFixtureDefinition(Vector2 size, LayerMask layerMask, object userData = null)
        {
            var polygonDefinition = new PolygonDef();
            polygonDefinition.SetAsBox(size.X, size.Y);
            polygonDefinition.Density = 1.0f;
            polygonDefinition.Friction = 4.0f;
            polygonDefinition.Filter = new FilterData
            {
                GroupIndex = (short) layerMask
            };
            polygonDefinition.UserData = userData;
            return polygonDefinition;
        }

        public static Body CreateCharacter(this World world, BodyDefinitionWrapper bodyDefinition, PolygonDef polygonDefinition)
        {
            lock (locker)
            {
                var body = world.CreateBody(bodyDefinition.BodyDefiniton);
                body.SetUserData(bodyDefinition.UserData);
                body.CreateShape(polygonDefinition);
                body.SetMassFromShapes();
                return body;
            }
        }

        public static void MoveBody(this Body body, Vector2 position, float speed, bool teleport = true)
        {
            lock (locker)
            {
                const float TELEPORT_DISTANCE = 1;

                var direction = position - body.GetPosition().ToVector2();
                var distanceToTravel = direction.FromVector2().Normalize();
                if (teleport && (distanceToTravel > TELEPORT_DISTANCE))
                {
                    body.SetXForm(position.FromVector2(), body.GetAngle());
                    return;
                }

                var distancePerTimestep = speed / FRAMES_PER_SECOND;
                if (distancePerTimestep > distanceToTravel)
                {
                    speed *= (distanceToTravel / distancePerTimestep);
                }

                var desiredVelocity = speed * direction;
                var changeInVelocity = desiredVelocity - body.GetLinearVelocity().ToVector2();

                var force = body.GetMass() * FRAMES_PER_SECOND * changeInVelocity;
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