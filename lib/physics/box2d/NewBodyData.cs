using Box2DX.Dynamics;

namespace Physics.Box2D
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