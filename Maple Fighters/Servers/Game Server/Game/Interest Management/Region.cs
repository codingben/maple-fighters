using System.Collections.Generic;
using System.Linq;
using CommonTools.Log;
using Game.Entities;
using MathematicsHelper;

namespace Game.InterestManagement
{
    internal class Region : IRegion
    {
        public Vector2 Position { get; }
        public Vector2 Size { get; }

        private readonly Dictionary<int, IEntity> entities = new Dictionary<int, IEntity>();

        public Region(Vector2 position, Vector2 size)
        {
            Position = position;
            Size = size;
        }

        public void AddEntity(IEntity entity)
        {
            if (entities.ContainsKey(entity.Id))
            {
                LogUtils.Log($"Region::AddEntity() -> An entity with a id #{entity.Id} already exists in a region.", LogMessageType.Warning);
                return;
            }

            entities.Add(entity.Id, entity);
        }

        public void RemoveEntity(IEntity entity)
        {
            if (!entities.ContainsKey(entity.Id))
            {
                LogUtils.Log($"Region::RemoveEntity() -> An entity with a id #{entity.Id} does not exists in a region.", LogMessageType.Warning);
                return;
            }

            entities.Remove(entity.Id);
        }

        public List<IEntity> GetAllEntities()
        {
            return entities.Select(entity => entity.Value).ToList();
        }
    }
}