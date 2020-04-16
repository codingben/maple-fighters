using System.Collections;
using Game.Common;
using UnityEngine;

namespace Scripts.Gameplay.Player.Behaviours
{
    [RequireComponent(typeof(PlayerController))]
    public class PlayerAttackedBehaviour : MonoBehaviour, IAttackPlayer
    {
        private PlayerController playerController;

        private void Awake()
        {
            playerController = GetComponent<PlayerController>();
        }

        public void OnPlayerAttacked(Vector3 direction)
        {
            if (playerController.GetPlayerState() != PlayerState.Attacked)
            {
                playerController.SetPlayerState(PlayerState.Attacked);

                StartCoroutine(WaitFrameAndBounce(direction));
            }
        }

        private IEnumerator WaitFrameAndBounce(Vector3 direction)
        {
            yield return null;

            playerController.Bounce(direction);
        }
    }
}