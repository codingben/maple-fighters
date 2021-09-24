using System.Collections;
using UnityEngine;

namespace Scripts.Gameplay.Player.Behaviours
{
    [RequireComponent(typeof(PlayerController))]
    public class PlayerAttackedBehaviour : MonoBehaviour, IAttackPlayer
    {
        private const float ATTACK_DELAY = 1;

        private PlayerController playerController;
        private bool isAttacked;

        private void Awake()
        {
            playerController = GetComponent<PlayerController>();
        }

        public void OnPlayerAttacked(Vector3 direction)
        {
            if (isAttacked)
            {
                return;
            }

            isAttacked = true;

            StartCoroutine(WaitFrameAndBounce(direction));
        }

        private IEnumerator WaitFrameAndBounce(Vector3 direction)
        {
            // NOTE: One frame delay to prevent an exception in the game server (in World.Step)
            yield return null;

            playerController.Bounce(direction);

            StartCoroutine(WaitAfterAttacked());
        }

        private IEnumerator WaitAfterAttacked()
        {
            yield return new WaitForSeconds(ATTACK_DELAY);

            isAttacked = false;
        }
    }
}