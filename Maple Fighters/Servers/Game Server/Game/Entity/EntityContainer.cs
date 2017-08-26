using System.Collections.Generic;
using CommonTools.Log;
using ServerApplication.Common.ComponentModel;
using ServerApplication.Common.Components;
using Shared.Game.Common;

namespace Game.Entities
{
    // TODO: Are this class and PeerContainer should using lockers?
    internal class EntityContainer : Component
    {
        private readonly Dictionary<int, Entity> entities = new Dictionary<int, Entity>();
        private readonly IdGenerator idGenerator;

        public EntityContainer()
        {
            idGenerator = ServerComponents.Container.GetComponent<IdGenerator>().AssertNotNull() as IdGenerator;
        }

        public Entity CreateEntity(EntityType type)
        {
            var entityId = idGenerator.GenerateId();
            var entity = new Entity(entityId, type);

            entities.Add(entityId, entity);

            return entity;
        }

        public Entity GetEntity(int entityId)
        {
            var value = entities.TryGetValue(entityId, out var entity);
            LogUtils.Assert(value, $"Could not find an entity with id {entity}");

            return entity;
        }

        public Entity[] GetAllEntities()
        {
            var entitiesTemp = new Entity[entities.Count];

            for (var i = 0; i < entities.Count; i++)
            {
                entitiesTemp[i] = entities[i];
            }

            return entitiesTemp;
        }
    }
}