using Box2DX.Dynamics;
using Physics.Box2D.Core;

namespace Physics.Box2D.Components.Interfaces
{
    public interface IEntityManager
    {
        void AddBody(BodyInfo bodyInfo);
        void RemoveBody(int id);

        Body GetBody(int id);
    }
}