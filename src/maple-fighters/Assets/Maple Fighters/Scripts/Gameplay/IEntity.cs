using UnityEngine;

namespace Scripts.Gameplay
{
    public interface IEntity
    {
        int Id { get; set; }

        GameObject GameObject { get; }
    }
}