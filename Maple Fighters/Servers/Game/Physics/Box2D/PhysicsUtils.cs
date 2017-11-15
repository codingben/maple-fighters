using Box2DX.Collision;
using Box2DX.Common;
using Box2DX.Dynamics;
using MathematicsHelper;

namespace Physics.Box2D
{
    public static class PhysicsUtils
    {
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

        public static Body CreateCharacter(this World world, Vector2 position, Vector2 size, LayerMask layerMask, object userData = null)
        {
            lock (locker)
            {
                var bodyDefinition = new BodyDef();
                bodyDefinition.Position.Set(position.X, position.Y);
                bodyDefinition.FixedRotation = true;

                var polygonDefinition = new PolygonDef();
                polygonDefinition.SetAsBox(size.X, size.Y);
                polygonDefinition.Density = 1.0f;
                polygonDefinition.Friction = 4.0f;
                polygonDefinition.Filter = new FilterData
                {
                    GroupIndex = (short) layerMask
                };
                polygonDefinition.UserData = userData;

                var body = world.CreateBody(bodyDefinition);
                body.CreateShape(polygonDefinition);
                body.SetMassFromShapes();
                return body;
            }
        }

        public static void MoveBody(this Body body, Vector2 position, float speed, bool teleport = true)
        {
            lock (locker)
            {
                const float PHYSICS_SIMULATION_FPS = 60.0f; // TODO: Get this data from another source
                const float TELEPORT_DISTANCE = 1;

                var direction = position - body.GetPosition().ToVector2();
                var distanceToTravel = direction.FromVector2().Normalize();
                if (teleport && (distanceToTravel > TELEPORT_DISTANCE))
                {
                    body.SetXForm(position.FromVector2(), body.GetAngle());
                    return;
                }

                var distancePerTimestep = speed / PHYSICS_SIMULATION_FPS;
                if (distancePerTimestep > distanceToTravel)
                {
                    speed *= (distanceToTravel / distancePerTimestep);
                }

                var desiredVelocity = speed * direction;
                var changeInVelocity = desiredVelocity - body.GetLinearVelocity().ToVector2();

                var force = body.GetMass() * PHYSICS_SIMULATION_FPS * changeInVelocity;
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