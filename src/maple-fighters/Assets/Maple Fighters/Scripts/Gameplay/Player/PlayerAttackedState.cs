using Game.Common;
using UnityEngine;

namespace Scripts.Gameplay.Actors
{
    public class PlayerAttackedState : IPlayerStateBehaviour
    {
        private readonly PlayerController playerController;
        private readonly Rigidbody2D rigidbody2D;

        private bool isOnGround;

        public PlayerAttackedState(PlayerController playerController)
        {
            this.playerController = playerController;

            var collider = playerController.GetComponent<Collider2D>();
            rigidbody2D = collider.attachedRigidbody;
        }

        public void OnStateEnter()
        {
            rigidbody2D.velocity = Vector2.zero;
        }

        public void OnStateUpdate()
        {
            if (playerController.IsGrounded() && !isOnGround)
            {
                return;
            }

            if (playerController.IsGrounded())
            {
                playerController.ChangePlayerState(PlayerState.Idle);
                return;
            }

            if (!isOnGround)
            {
                isOnGround = true;
            }
        }

        public void OnStateFixedUpdate()
        {
            // Left blank intentionally
        }

        public void OnStateExit()
        {
            isOnGround = false;
        }
    }
}