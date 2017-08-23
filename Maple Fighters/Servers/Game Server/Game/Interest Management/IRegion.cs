using System.Collections.Generic;
using Game.Entities;
using MathematicsHelper;

namespace Game.InterestManagement
{
    public interface IRegion // (Scene -> IRegion -> ServerComponents)
    {
        Vector2 Position { get; }
        Vector2 Size { get; }

        void AddEntity(IEntity entity);
        void RemoveEntity(IEntity entity);

        /*void AddSubscription(IEntity entity);
        void RemoveSubscription(IEntity entity);*/

        List<IEntity> GetAllEntities();
        // List<IEntity> GetSubscribers();
    }
}