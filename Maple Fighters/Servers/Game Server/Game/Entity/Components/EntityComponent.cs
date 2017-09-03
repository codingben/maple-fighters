using Game.Entities;
using ServerApplication.Common.ComponentModel;

namespace Game.Entity.Components
{
    public class EntityComponent : IComponent
    {
        protected IEntity OwnerEntity { get; }

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