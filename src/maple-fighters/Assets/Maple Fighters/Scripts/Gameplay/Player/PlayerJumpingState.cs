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

            var jumpForce = playerController.Configuration.JumpForce;
            var force = new Vector2(direction, 1) * jumpForce;
            rigidbody2D.AddForce(force, ForceMode2D.Impulse);
        }

        public void OnStateUpdate()
        {
            if (isJumping && playerController.IsGrounded())
            {
                playerController.ChangePlayerState(PlayerState.Idle);
            }
            else
            {
                if (playerController.IsGrounded() == false)
                {
                    isJumping = true;
                }
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