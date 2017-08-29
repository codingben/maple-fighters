using System.Collections.Generic;
using Game.Entities;
using MathematicsHelper;

namespace Game.InterestManagement
{
    public interface IRegion
    {
        Rectangle Rectangle { get; }

        void AddSubscription(IEntity entity);
        void RemoveSubscription(IEntity entity);

        bool HasSubscription(int entityId);

        List<IEntity> GetAllSubscribers();
    }
}