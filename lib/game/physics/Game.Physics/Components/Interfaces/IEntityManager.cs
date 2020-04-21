using Physics.Box2D.Core;

namespace Physics.Box2D.Components.Interfaces
{
    public interface IEntityManager
    {
        void Update();

        void AddBody(BodyData bodyData);

        void RemoveBody(int id);

        BodyData GetBody(int id);
    }
}