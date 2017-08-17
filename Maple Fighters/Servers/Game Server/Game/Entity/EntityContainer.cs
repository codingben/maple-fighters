using System.Collections.Generic;
using CommonTools.Log;
using ServerApplication.Common.ComponentModel;
using ServerApplication.Common.Components;

namespace Game.Entities
{
    // Are this class and PeerContainer should using lockers?
    internal class EntityContainer : IComponent
    {
        private readonly Dictionary<int, Entity> entities = new Dictionary<int, Entity>();

        public Entity CreateEntity(int peerId, EntityType type)
        {
            var entity = new Entity(peerId, type);
            entities.Add(peerId, entity);

            ServerComponents.Container.GetComponent(out PeerContainer peerContainer);
            // ClientPeer<IClientPeer> -> peerContainer.AccessProvider.Components

            return entity;
        }

        public Entity CreateEntity(EntityType type)
        {
            ServerComponents.Container.GetComponent(out IdGenerator idGenerator);

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

        public void Dispose()
        {
            // TODO: Implement
        }
    }
}