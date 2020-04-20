using Box2DX.Dynamics;

namespace Physics.Box2D.Core
{
    public struct BodyInfo
    {
        public int Id { get; }

        public BodyDef BodyDefinition { get; }

        public PolygonDef FixtureDefinition { get; }

        public object UserData { get; }

        public BodyInfo(
            int id,
            BodyDef bodyDefinition,
            PolygonDef fixtureDefinition,
            object userData)
        {
            Id = id;
            BodyDefinition = bodyDefinition;
            FixtureDefinition = fixtureDefinition;
            UserData = userData;
        }
    }
}