using System.Collections.Generic;
using Game.Entities;
using MathematicsHelper;

namespace Game.InterestManagement
{
    public interface IRegion // (Scene -> IRegion -> ServerComponents)
    {
        Rectangle Rectangle { get; }

        void AddSubscription(IEntity entity);
        void RemoveSubscription(IEntity entity);

        List<IEntity> GetAllSubscribers();
    }
}