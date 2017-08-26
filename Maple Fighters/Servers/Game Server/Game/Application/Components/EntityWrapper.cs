using CommonTools.Log;
using Game.Entities;
using ServerApplication.Common.ComponentModel;
using Shared.Game.Common;

namespace Game.Application.Components
{
    internal class EntityWrapper : Component
    {
        public IEntity Entity { get; private set; }

        private readonly EntityIdToPeerIdConverter entityIdToPeerIdConverter;

        public EntityWrapper(EntityType type, int peerId)
        {
            entityIdToPeerIdConverter = ServerComponents.Container.GetComponent<EntityIdToPeerIdConverter>().AssertNotNull() as EntityIdToPeerIdConverter;

            CreateEntity(type);
            AddEntityToPeerIdConverter(peerId);
        }

        public new void Dispose()
        {
            RemoveEntityIdFromConverter();
        }

        private void CreateEntity(EntityType type)
        {
            var entityContainer = ServerComponents.Container.GetComponent<EntityContainer>().AssertNotNull() as EntityContainer;
            Entity = entityContainer.CreateEntity(type);
        }

        private void AddEntityToPeerIdConverter(int peerId)
        {
            entityIdToPeerIdConverter.AddEntityIdToPeerId(Entity.Id, peerId);
        }

        private void RemoveEntityIdFromConverter()
        {
            entityIdToPeerIdConverter.RemoveEntityIdToPeerId(Entity.Id);
        }
    }
}