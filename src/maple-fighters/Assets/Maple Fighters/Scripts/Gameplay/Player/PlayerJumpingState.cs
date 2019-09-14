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
            var horizontal = 
                playerController.GetAxis(Axes.Horizontal, isRaw: true);
            var jumpForce = playerController.Properties.JumpForce;
            var jumpHeight = playerController.Properties.JumpHeight;

            rigidbody2D.velocity = 
                new Vector2(horizontal * jumpForce, jumpHeight);

            var direction = 
                horizontal < 0 ? Directions.Left : Directions.Right;
            playerController.ChangeDirection(direction);
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