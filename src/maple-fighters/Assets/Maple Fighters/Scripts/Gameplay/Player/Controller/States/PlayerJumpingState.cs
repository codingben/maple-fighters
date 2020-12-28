using Scripts.Gameplay.Graphics;
using UnityEngine;

namespace Scripts.Gameplay.Player.States
{
    public class PlayerJumpingState : IPlayerStateBehaviour
    {
        private readonly PlayerController playerController;
        private readonly Rigidbody2D rigidbody2D;

        private bool isJumping;
        private bool isRushing;

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

            if (isJumping)
            {
                var horizontal = Utils.GetAxis(Axes.Horizontal, isRaw: true);
                if (horizontal != 0)
                {
                    if (isRushing == false && IsRushKeyClicked())
                    {
                        var rushSpeed = playerController.GetProperties().RushSpeed;
                        var force = new Vector2(horizontal * rushSpeed, 0);

                        playerController.CreateRushEffect(horizontal);
                        playerController.Bounce(force);

                        isRushing = true;
                    }

                    var direction =
                        horizontal < 0 ? Directions.Left : Directions.Right;

                    playerController.ChangeDirection(direction);
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
            isRushing = false;
        }

        private bool IsGrounded()
        {
            return playerController.IsGrounded();
        }

        private bool IsRushKeyClicked()
        {
            var rushKey = playerController.GetKeyboardSettings().RushKey;

            return Input.GetKeyDown(rushKey);
        }
    }
}