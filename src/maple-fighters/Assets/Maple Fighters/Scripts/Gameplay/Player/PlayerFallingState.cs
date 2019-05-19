using Game.Common;
using UnityEngine;

namespace Scripts.Gameplay.Actors
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
            // TODO: Move to the configuration
            var horizontal = Input.GetAxis("Horizontal");
            var speed = playerController.Configuration.Speed;
            var direction = 
                new Vector2(horizontal * speed, rigidbody2D.velocity.y);

            rigidbody2D.velocity = direction;
        }

        public void OnStateUpdate()
        {
            if (IsGrounded())
            {
                playerController.ChangePlayerState(PlayerState.Idle);
            }
        }

        public void OnStateFixedUpdate()
        {
            // Left blank intentionally
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