using Game.Entities;
using ServerApplication.Common.ComponentModel;

namespace Game.Entity.Components
{
    public class EntityComponent : IComponent
    {
        public readonly IEntity OwnerEntity;

        protected EntityComponent(IEntity entity)
        {
            OwnerEntity = entity;
        }

        public void Dispose()
        {
            // Left blank intentionally
        }
    }
}