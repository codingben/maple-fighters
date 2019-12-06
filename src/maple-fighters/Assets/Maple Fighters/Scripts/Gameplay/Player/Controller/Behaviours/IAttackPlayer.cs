using UnityEngine;

namespace Scripts.Gameplay.Player.Behaviours
{
    public interface IAttackPlayer
    {
        void OnPlayerAttacked(Vector3 direction);
    }
}