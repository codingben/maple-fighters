using UnityEngine;

namespace Scripts.Gameplay.Entity
{
    public interface IEntity
    {
        int Id { get; set; }

        GameObject GameObject { get; }
    }
}