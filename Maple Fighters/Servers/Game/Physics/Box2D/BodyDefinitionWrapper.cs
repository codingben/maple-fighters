using Box2DX.Collision;
using Box2DX.Dynamics;

namespace Physics.Box2D
{
    public struct BodyDefinitionWrapper
    {
        public readonly BodyDef BodyDef;
        public readonly PolygonDef FixtureDefinition;
        public readonly object UserData;

        public BodyDefinitionWrapper(BodyDef bodyDef, PolygonDef fixtureDefinition, object userData)
        {
            BodyDef = bodyDef;
            FixtureDefinition = fixtureDefinition;
            UserData = userData;
        }
    }
}