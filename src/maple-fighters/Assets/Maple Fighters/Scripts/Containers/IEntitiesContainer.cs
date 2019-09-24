using Game.Common;
using Scripts.Gameplay;

namespace Scripts.Containers
{
    public interface IEntitiesContainer
    {
        void SetLocalEntity(IEntity entity);

        IEntity AddEntity(SceneObjectParameters parameters);

        void RemoveEntity(int id);
    }
}