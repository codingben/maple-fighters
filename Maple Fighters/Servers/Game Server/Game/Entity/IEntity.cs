using ServerApplication.Common.ComponentModel;

namespace Game.Entities
{
    internal interface IEntity
    {
        int Id { get; }
        int PresenceSceneId { get; }

        EntityType Type { get; }

        IComponentsContainer Components { get; }
    }
}