using Box2DX.Collision;
using Box2DX.Dynamics;

namespace Physics.Box2D
{
    public struct BodyDefinitionWrapper
    {
        public readonly BodyDef BodyDefiniton;
        public readonly PolygonDef FixtureDefinition;
        public readonly object UserData;

        public BodyDefinitionWrapper(BodyDef bodyDefinition, PolygonDef fixtureDefinition, object userData)
        {
            BodyDefiniton = bodyDefinition;
            FixtureDefinition = fixtureDefinition;
            UserData = userData;
        }
    }
}