using Box2DX.Dynamics;

namespace Physics.Box2D
{
    public struct BodyData
    {
        public int Id { get; }

        public Body Body { get; }

        public BodyData(
            int id,
            Body body)
        {
            Id = id;
            Body = body;
        }
    }
}