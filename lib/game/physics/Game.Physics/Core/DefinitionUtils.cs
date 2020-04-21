using Box2DX.Dynamics;
using Common.MathematicsHelper;

namespace Physics.Box2D.Core
{
    public static class DefinitionUtils
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

        public static PolygonDef CreateFixtureDefinition(
            Vector2 size,
            LayerMask layerMask,
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
                    GroupIndex = (short)layerMask
                }
            };
            polygonDefinition.SetAsBox(size.X, size.Y);

            return polygonDefinition;
        }
    }
}