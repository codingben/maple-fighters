using UnityEngine;

namespace Scripts.Gameplay.Entity
{
    public interface IEntityCollection
    {
        IEntity Create(int id, string name, Vector2 position);

        void Destroy(int id);

        IEntity TryGet(int id);
    }
}