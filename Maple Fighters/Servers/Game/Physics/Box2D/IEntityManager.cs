using Box2DX.Dynamics;
using ComponentModel.Common;

namespace Physics.Box2D
{
    public interface IEntityManager : IExposableComponent
    {
        void AddBody(BodyInfo bodyInfo);
        void RemoveBody(int id);

        Body GetBody(int id);
    }
}