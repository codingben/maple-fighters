using UnityEngine;

namespace Scripts.Gameplay.Player.States
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
            var horizontal = Utils.GetAxis(Axes.Horizontal, isRaw: true);
            var jumpForce = playerController.GetProperties().JumpForce;
            var jumpHeight = playerController.GetProperties().JumpHeight;

            rigidbody2D.velocity =
                new Vector2(horizontal * jumpForce, jumpHeight);

            if (horizontal != 0)
            {
                var direction =
                    horizontal < 0 ? Directions.Left : Directions.Right;
                playerController.ChangeDirection(direction);
            }
        }

        public void OnStateUpdate()
        {
            if (isJumping && IsGrounded())
            {
                playerController.SetPlayerState(PlayerStates.Idle);
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