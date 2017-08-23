using ServerApplication.Common.ComponentModel;

namespace Game.Entities
{
    public interface IEntity
    {
        int Id { get; }
        int PresenceSceneId { get; } // TODO: Move to a entity component that will provide scene status (Has scene, scene id, etc..)

        EntityType Type { get; }

        IComponentsContainer Components { get; }
    }
}