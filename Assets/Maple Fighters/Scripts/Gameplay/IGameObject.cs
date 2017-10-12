using UnityEngine;

namespace Scripts.Gameplay
{
    public interface IGameObject
    {
        int Id { get; }

        GameObject GetGameObject();
    }
}