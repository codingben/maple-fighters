using System;
using Scripts.Gameplay.Actors.Entity;

namespace Scripts.Containers.Entity
{
    public interface IEntityContainer
    {
        event Action EntityAdded;

        IEntity GetLocalEntity();
        IEntity GetRemoteEntity(int id);
    }
}