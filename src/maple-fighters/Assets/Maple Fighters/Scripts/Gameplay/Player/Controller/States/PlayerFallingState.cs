using UnityEngine;

namespace Scripts.Gameplay.Player.States
{
    public class PlayerFallingState : IPlayerStateBehaviour
    {
        private readonly PlayerController playerController;
        private readonly Rigidbody2D rigidbody2D;

        public PlayerFallingState(PlayerController playerController)
        {
            this.playerController = playerController;

            var collider = playerController.GetComponent<Collider2D>();
            rigidbody2D = collider.attachedRigidbody;
        }

        public void OnStateEnter()
        {
            SetVelocity();
            SetDirection();
        }

        public void OnStateUpdate()
        {
            SetDirection();
        }

        public void OnStateFixedUpdate()
        {
            if (IsGrounded())
            {
                playerController.SetPlayerState(PlayerStates.Idle);
            }
        }

        public void OnStateExit()
        {
            // Left blank intentionally
        }

        private void SetVelocity()
        {
            var horizontal = Utils.GetAxis(Axes.Horizontal);
            if (horizontal != 0)
            {
                var speed = playerController.GetProperties().Speed;
                var x = horizontal * speed;
                var y = rigidbody2D.velocity.y;

                rigidbody2D.velocity = new Vector2(x, y);
            }
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

        private bool IsGrounded()
        {
            return playerController.IsGrounded();
        }
    }
}