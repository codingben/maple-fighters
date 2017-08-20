using System.Collections.Generic;
using CommonTools.Log;
using Game.Entities;
using MathematicsHelper;

namespace Game.InterestManagement
{
    internal class Scene : IScene
    {
        private readonly Boundaries boundaries;
        private readonly Dictionary<int, IEntity> entities = new Dictionary<int, IEntity>();

        public Scene(Boundaries boundaries, int regions)
        {
            this.boundaries = boundaries;
        }

        public IRegion GetRegion(Vector2 position)
        {
            throw new System.NotImplementedException();
        }

        public void AddEntity(IEntity entity)
        {
            if (entities.ContainsKey(entity.Id))
            {
                LogUtils.Log($"Scene::AddEntity() -> An entity with a id #{entity.Id} already exists in a scene.", LogMessageType.Warning);
                return;
            }

            entities.Add(entity.Id, entity);
        }

        public void RemoveEntity(IEntity entity)
        {
            if (!entities.ContainsKey(entity.Id))
            {
                LogUtils.Log($"Scene::AddEntity() -> An entity with a id #{entity.Id} does not exists in a scene.", LogMessageType.Warning);
                return;
            }

            entities.Remove(entity.Id);
        }

        public IEntity GetEntity(int entityId)
        {
            if (entities.TryGetValue(entityId, out var entity))
            {
                return entity;
            }

            LogUtils.Log($"Scene::GetEntity() - Could not found an entity id #{entityId}", LogMessageType.Error);

            return null;
        }

        public void AddEntitiesForEntity(IEntity entity, List<IEntity> entities)
        {
            throw new System.NotImplementedException();
        }

        public void RemoveEntitiesForEntity(IEntity entity, List<IEntity> entities)
        {
            throw new System.NotImplementedException();
        }
    }
}