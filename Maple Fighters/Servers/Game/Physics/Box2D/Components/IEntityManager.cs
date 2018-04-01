using Box2DX.Dynamics;

namespace Physics.Box2D
{
    public interface IEntityManager
    {
        void AddBody(BodyInfo bodyInfo);
        void RemoveBody(Body body, int id);

        Body GetBody(int id);
    }
}