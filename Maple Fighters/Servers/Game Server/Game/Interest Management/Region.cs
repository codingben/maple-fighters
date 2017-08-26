using System.Collections.Generic;
using System.Linq;
using CommonTools.Log;
using Game.Entities;
using MathematicsHelper;

namespace Game.InterestManagement
{
    internal class Region : IRegion
    {
        public Rectangle Rectangle { get; }

        private readonly Dictionary<int, IEntity> entities = new Dictionary<int, IEntity>();

        public Region(Rectangle rectangle)
        {
            Rectangle = rectangle;
        }

        public void AddSubscription(IEntity entity)
        {
            if (entities.ContainsKey(entity.Id))
            {
                LogUtils.Log($"Region::AddEntity() -> An entity with a id #{entity.Id} already exists in a region.", LogMessageType.Warning);
                return;
            }

            entities.Add(entity.Id, entity);
        }

        public void RemoveSubscription(IEntity entity)
        {
            if (!entities.ContainsKey(entity.Id))
            {
                LogUtils.Log($"Region::RemoveEntity() -> An entity with a id #{entity.Id} does not exists in a region.", LogMessageType.Warning);
                return;
            }

            entities.Remove(entity.Id);
        }

        public List<IEntity> GetAllSubscribers()
        {
            return entities.Select(entity => entity.Value).ToList();
        }
    }
}