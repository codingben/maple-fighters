using Scripts.Gameplay.Actors.Entity;

namespace Scripts.Containers.Entity
{
    public interface IEntityContainer
    {
        IEntity GetLocalEntity();
        IEntity GetRemoteEntity(int id);
    }
}