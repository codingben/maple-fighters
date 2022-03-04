using UnityEngine;

namespace Scripts.Gameplay.Player.States
{
    public class PlayerJumpingState : IPlayerStateBehaviour
    {
        private readonly PlayerController playerController;
        private readonly Rigidbody2D rigidbody2D;

        private bool isRushing;
        private float previousTime;

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
                    horizontal < 0 ? Direction.Left : Direction.Right;

                playerController.ChangeDirection(direction);
            }

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
            else
            {
                if (isRushing == false && IsRushKeyClicked())
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