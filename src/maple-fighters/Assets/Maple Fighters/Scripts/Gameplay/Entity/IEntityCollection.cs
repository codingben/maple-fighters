using Scripts.Gameplay;
using UnityEngine;

namespace Scripts.Gameplay.Entity
{
    public interface IEntityCollection
    {
        IEntity Add(int id, string name, Vector2 position);

        void Remove(int id);

        IEntity TryGet(int id);
    }
}