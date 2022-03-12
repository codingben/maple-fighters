using UnityEngine;

namespace Scripts.Gameplay.Player.States
{
    public class PlayerJumpingState : IPlayerStateBehaviour
    {
        private readonly PlayerController playerController;

        private bool isRushing;
        private float previousTime;

        public PlayerJumpingState(PlayerController playerController)
        {
            this.playerController = playerController;
        }

        public void OnStateEnter()
        {
            SetDirection();
            Jump();

            previousTime = Time.time;
        }

        private void Jump()
        {
            var horizontal = Utils.GetAxis(Axes.Horizontal, isRaw: true);
            var jumpForce = playerController.GetProperties().JumpForce;
            var jumpHeight = playerController.GetProperties().JumpHeight;

            playerController.Bounce(new Vector2(horizontal * jumpForce, jumpHeight));
        }

        public void OnStateUpdate()
        {
            if (IsGrounded())
            {
                if (Time.time > previousTime + 0.1f)
                {
                    playerController.SetPlayerState(PlayerStates.Idle);
                }
            }
            else if (CanMove())
            {
                if (IsRushKeyClicked())
                {
                    CreateRushEffect();
                }

                SetDirection();
            }
        }

        public void OnStateFixedUpdate()
        {
            // Left blank intentionally
        }

        public void OnStateExit()
        {
            isRushing = false;
        }

        private void CreateRushEffect()
        {
            if (isRushing)
            {
                return;
            }

            var rushSpeed = playerController.GetProperties().RushSpeed;
            var direction = playerController.transform.localScale.x * -1;
            var force = new Vector2(direction * rushSpeed, 0);

            playerController.CreateRushEffect(direction);
            playerController.Bounce(force);

            isRushing = true;
        }

        private void SetDirection()
        {
            var horizontal = Utils.GetAxis(Axes.Horizontal, isRaw: true);
            if (horizontal != 0)
            {
                var direction =
                    horizontal < 0 ? Direction.Left : Direction.Right;

                playerController.ChangeDirection(direction);
            }
        }

        private bool CanMove()
        {
            return playerController.CanMove();
        }

        private bool IsGrounded()
        {
            return playerController.IsGrounded();
        }

        private bool IsRushKeyClicked()
        {
            var key = playerController.GetKeyboardSettings().RushKey;

            return Input.GetKeyDown(key);
        }
    }
}