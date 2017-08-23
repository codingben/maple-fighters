using System.Collections.Generic;
using CommonTools.Log;
using Game.Entities;
using Game.Systems;
using MathematicsHelper;

namespace Game.InterestManagement
{
    internal class Scene : IScene
    {
        private readonly Boundaries boundaries;
        private readonly Dictionary<int, IEntity> entities = new Dictionary<int, IEntity>();

        private readonly TransformSystem transformSystem;

        public Scene(Boundaries boundaries, Vector2 regions)
        {
            this.boundaries = boundaries;

            transformSystem = new TransformSystem();
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

            transformSystem.AddEntity(entity);
        }

        public void RemoveEntity(IEntity entity)
        {
            if (!entities.ContainsKey(entity.Id))
            {
                LogUtils.Log($"Scene::AddEntity() -> An entity with a id #{entity.Id} does not exists in a scene.", LogMessageType.Warning);
                return;
            }

            entities.Remove(entity.Id);

            transformSystem.RemoveEntity(entity);
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
            // Send an event for this entity which contains new entities.
        }

        public void RemoveEntitiesForEntity(IEntity entity, List<IEntity> entities)
        {
            // Send an event for this entity which will remove these entities.
        }
    }
}