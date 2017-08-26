using Game.Entities;

namespace Game.InterestManagement
{
    internal interface IScene
    {
        int SceneId { get; }

        void AddEntity(IEntity entity);
        void RemoveEntity(IEntity entity);

        IEntity GetEntity(int entityId);

        void AddEntitiesForEntity(IEntity entity, IEntity[] entities);
        void RemoveEntitiesForEntity(IEntity entity, IEntity[] entities);
    }
}