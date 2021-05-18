using Box2DX.Dynamics;

namespace Game.Physics
{
    public struct NewBodyData
    {
        public int Id { get; }

        public BodyDef BodyDef { get; }

        public PolygonDef PolygonDef { get; }

        public NewBodyData(int id, BodyDef bodyDef, PolygonDef polygonDef)
        {
            Id = id;
            BodyDef = bodyDef;
            PolygonDef = polygonDef;
        }
    }
}