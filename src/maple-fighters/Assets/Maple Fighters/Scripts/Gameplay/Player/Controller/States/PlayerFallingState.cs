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
            FallDownWithDirection();
        }

        private void FallDownWithDirection()
        {
            var horizontal = Utils.GetAxis(Axes.Horizontal);
            var speed = playerController.GetProperties().Speed;

            rigidbody2D.velocity =
                new Vector2(horizontal * speed, rigidbody2D.velocity.y);

            if (horizontal != 0)
            {
                var direction =
                    horizontal > 0 ? Directions.Right : Directions.Left;
                playerController.ChangeDirection(direction);
            }
        }

        public void OnStateUpdate()
        {
            // Left blank intentionally
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

        private bool IsGrounded()
        {
            return playerController.IsGrounded();
        }
    }
}