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
            var horizontal = Input.GetAxisRaw("Horizontal");
            var jumpForce = playerController.Configuration.JumpForce;
            var jumpHeight = playerController.Configuration.JumpHeight;
            var direction = new Vector2(horizontal * jumpForce, jumpHeight);

            rigidbody2D.velocity = direction;
        }

        public void OnStateUpdate()
        {
            if (isJumping && IsGrounded())
            {
                playerController.ChangePlayerState(PlayerState.Idle);
            }
            else
            {
                if (IsGrounded() == false)
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

        private bool IsGrounded()
        {
            return playerController.IsGrounded();
        }
    }
}