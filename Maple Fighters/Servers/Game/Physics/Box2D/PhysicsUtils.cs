using Box2DX.Collision;
using Box2DX.Common;
using Box2DX.Dynamics;
using MathematicsHelper;

namespace Physics.Box2D
{
    public static class PhysicsUtils
    {
        public static void CreateGround(this World world, Vector2 position, Vector2 size)
        {
            var bodyDef = new BodyDef();
            bodyDef.Position.Set(position.X, position.Y);

            var boxDef = new PolygonDef();
            boxDef.SetAsBox(size.X, size.Y);
            boxDef.Density = 0.0f;

            var body = world.CreateBody(bodyDef);
            body.CreateShape(boxDef);
            body.SetMassFromShapes();
        }

        public static Body CreateCharacter(this World world, Vector2 position, Vector2 size)
        {
            var bodyDef = new BodyDef();
            bodyDef.Position.Set(position.X, position.Y);
            bodyDef.FixedRotation = true;

            var boxDef = new PolygonDef();
            boxDef.SetAsBox(size.X, size.Y);
            boxDef.Density = 1.0f;
            boxDef.Friction = 0.3f;

            var body = world.CreateBody(bodyDef);
            body.CreateShape(boxDef);
            body.SetMassFromShapes();
            return body;
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