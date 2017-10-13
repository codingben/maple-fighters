using UnityEngine;

namespace Scripts.Gameplay
{
    public interface IGameObject
    {
        int Id { get; set; }

        GameObject GetGameObject();
    }
}