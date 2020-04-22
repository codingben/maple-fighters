using Box2DX.Dynamics;
using Common.MathematicsHelper;

namespace Game.Physics
{
    public static class WorldUtils
    {
        public static void CreateGround(
            World world,
            Vector2 position,
            Vector2 size,
            short groupIndex,
            float density = 0.0f)
        {
            var bodyDefinition = new BodyDef();
            bodyDefinition.Position.Set(position.X, position.Y);

            var boxDefinition = new PolygonDef();
            boxDefinition.SetAsBox(size.X, size.Y);
            boxDefinition.Density = density;
            boxDefinition.Filter = new FilterData
            {
                GroupIndex = groupIndex
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