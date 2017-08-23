using Game.InterestManagement;
using ServerApplication.Common.ComponentModel;

namespace Game.Entities
{
    internal class Entity : IEntity
    {
        public int Id { get; }
        public int PresenceSceneId { get; set; }

        public EntityType Type { get; }

        public IComponentsContainer Components { get; } = new ComponentsContainer();

        public IRegion GetRegion()
        {
            throw new System.NotImplementedException();
        }

        public Entity(int id, EntityType type)
        {
            Id = id;
            Type = type;
        }
    }
}