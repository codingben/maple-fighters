using UnityEngine;

namespace Scripts.Gameplay.GameEntity
{
    public interface IEntity
    {
        int Id { get; set; }

        GameObject GameObject { get; }
    }
}