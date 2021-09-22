using System.Collections;
using UnityEngine;

namespace Scripts.Gameplay.Player.Behaviours
{
    [RequireComponent(typeof(PlayerController))]
    public class PlayerAttackedBehaviour : MonoBehaviour, IAttackPlayer
    {
        [SerializeField]
        private float attackDelay = 2;

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

            // NOTE: Waiting one frame prevent an exception in the game server.
            StartCoroutine(WaitFrameAndBounce(direction));
        }

        private IEnumerator WaitFrameAndBounce(Vector3 direction)
        {
            yield return new WaitForSeconds(0.1f);

            playerController.Bounce(direction);

            StartCoroutine(WaitAfterAttacked());
        }

        private IEnumerator WaitAfterAttacked()
        {
            isAttacked = true;

            yield return new WaitForSeconds(attackDelay);

            isAttacked = false;
        }
    }
}