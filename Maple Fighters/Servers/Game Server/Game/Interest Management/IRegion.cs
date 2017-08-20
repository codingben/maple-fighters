using System.Collections.Generic;
using Game.Entities;
using MathematicsHelper;

namespace Game.InterestManagement
{
    internal interface IRegion // (Scene -> IRegion -> ServerComponents)
    {
        void SetPosition(Vector2 position);
        void SetSize(Vector2 size);

        Vector2 GetPosition();
        Vector2 GetSize();

        void AddEntity(IEntity entity);
        void RemoveEntity(IEntity entity);

        /*void AddSubscription(IEntity entity);
        void RemoveSubscription(IEntity entity);*/

        List<IEntity> GetEntities();
        // List<IEntity> GetSubscribers();
    }
}