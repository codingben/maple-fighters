using System.Collections.Generic;
using Game.Entities;
using MathematicsHelper;

namespace Game.InterestManagement
{
    internal interface IScene
    {
        IRegion GetRegion(Vector2 position);

        void AddEntity(IEntity entity);
        void RemoveEntity(IEntity entity);

        IEntity GetEntity(int entityId); // TODO: Make a dictionary to convert an entity id to a peer id.

        void AddEntitiesForEntity(IEntity entity, List<IEntity> entities);
        void RemoveEntitiesForEntity(IEntity entity, List<IEntity> entities);
    }
}