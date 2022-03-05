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
            Jump();
        }

        private void Jump()
        {
            var horizontal = Utils.GetAxis(Axes.Horizontal, isRaw: true);
            if (horizontal != 0)
            {
                var direction =
                    horizontal < 0 ? Direction.Left : Direction.Right;

                playerController.ChangeDirection(direction);
            }

            var jumpForce = playerController.GetProperties().JumpForce;
            var jumpHeight = playerController.GetProperties().JumpHeight;

            playerController.Bounce(new Vector2(horizontal * jumpForce, jumpHeight));

            previousTime = Time.time;
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
                if (IsRushKeyClicked() && isRushing == false)
                {
                    var rushSpeed = playerController.GetProperties().RushSpeed;
                    var direction = playerController.transform.localScale.x * -1;
                    var force = new Vector2(direction * rushSpeed, 0);

                    playerController.CreateRushEffect(direction);
                    playerController.Bounce(force);

                    isRushing = true;
                }

                var horizontal = Utils.GetAxis(Axes.Horizontal, isRaw: true);
                if (horizontal != 0)
                {
                    var direction =
                        horizontal < 0 ? Direction.Left : Direction.Right;

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
            isRushing = false;
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
            var rushKey = playerController.GetKeyboardSettings().RushKey;

            return Input.GetKeyDown(rushKey);
        }
    }
}