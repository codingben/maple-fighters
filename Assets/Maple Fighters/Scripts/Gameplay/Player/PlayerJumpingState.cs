using Game.Common;
using UnityEngine;

namespace Scripts.Gameplay.Actors
{
    public class PlayerJumpingState : IPlayerStateBehaviour
    {
        private readonly PlayerController playerController;
        private readonly Rigidbody2D rigidbody2D;

        private bool isJumping;

        public PlayerJumpingState(PlayerController playerController)
        {
            this.playerController = playerController;

            var collider = playerController.GetComponent<Collider2D>();
            rigidbody2D = collider.attachedRigidbody;
        }

        public void OnStateEnter()
        {
            Jump();
        }

        private void Jump()
        {
            // TODO: Move to the configuration
            const float JumpForce = 0.075f;

            var direction = rigidbody2D.velocity.x;
            if (direction != 0)
            {
                direction = 
                    rigidbody2D.velocity.x > 0 ? JumpForce : -JumpForce;
            }

            var force = new Vector2(direction, 1);
            rigidbody2D.AddForce(
                force * playerController.Configuration.JumpForce,
                ForceMode2D.Impulse);
        }

        public void OnStateUpdate()
        {
            if (playerController.IsGrounded() && !isJumping)
            {
                return;
            }

            if (playerController.IsGrounded())
            {
                playerController.ChangePlayerState(PlayerState.Idle);
                return;
            }

            if (!isJumping)
            {
                isJumping = true;
            }
        }

        public void OnStateFixedUpdate()
        {
            // Left blank intentionally
        }

        public void OnStateExit()
        {
            isJumping = false;
        }
    }
}