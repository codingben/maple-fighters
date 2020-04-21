using Box2DX.Dynamics;

namespace Physics.Box2D.Core
{
    public struct BodyData
    {
        public int Id { get; }

        public BodyDef BodyDefinition { get; }

        public PolygonDef FixtureDefinition { get; }

        public BodyData(
            int id,
            BodyDef bodyDefinition,
            PolygonDef fixtureDefinition)
        {
            Id = id;
            BodyDefinition = bodyDefinition;
            FixtureDefinition = fixtureDefinition;
        }
    }
}