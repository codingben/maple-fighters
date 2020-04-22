using Box2DX.Dynamics;

namespace Game.Physics
{
    public struct BodyData
    {
        public int Id { get; }

        public Body Body { get; }

        public BodyData(int id, Body body)
        {
            Id = id;
            Body = body;
        }
    }
}