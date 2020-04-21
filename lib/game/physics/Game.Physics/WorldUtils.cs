using Box2DX.Dynamics;
using Common.MathematicsHelper;

namespace Physics.Box2D
{
    public static class WorldUtils
    {
        public static void CreateGround(
            World world,
            Vector2 position,
            Vector2 size,
            float density = 0.0f,
            LayerMask layerMask = LayerMask.Ground)
        {
            var bodyDefinition = new BodyDef();
            bodyDefinition.Position.Set(position.X, position.Y);

            var boxDefinition = new PolygonDef();
            boxDefinition.SetAsBox(size.X, size.Y);
            boxDefinition.Density = density;
            boxDefinition.Filter = new FilterData
            {
                GroupIndex = (short)layerMask
            };

            var body = world.CreateBody(bodyDefinition);
            body.CreateFixture(boxDefinition);
            body.SetMassFromShapes();
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
    }
}