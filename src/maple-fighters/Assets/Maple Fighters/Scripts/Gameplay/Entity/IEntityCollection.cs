using UnityEngine;

namespace Scripts.Gameplay.GameEntity
{
    public interface IEntityCollection
    {
        IEntity Add(int id, string name, Vector2 position);

        void Remove(int id);

        IEntity TryGet(int id);
    }
}