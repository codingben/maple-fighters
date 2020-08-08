using System.Collections;
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
            if (playerController.GetPlayerState() != PlayerStates.Attacked)
            {
                playerController.SetPlayerState(PlayerStates.Attacked);

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