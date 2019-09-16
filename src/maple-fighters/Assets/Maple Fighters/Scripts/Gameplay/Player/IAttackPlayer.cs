using UnityEngine;

namespace Scripts.Gameplay.Player
{
    public interface IAttackPlayer
    {
        void OnPlayerAttacked(Vector3 direction);
    }
}