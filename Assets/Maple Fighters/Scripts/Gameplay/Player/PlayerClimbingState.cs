using Game.Common;
using UnityEngine;

namespace Scripts.Gameplay.Actors
{
    public class PlayerClimbingState : IPlayerStateBehaviour
    {
        private readonly PlayerController playerController;
        private readonly Rigidbody2D rigidbody2D;

        private float direction;

        public PlayerClimbingState(PlayerController playerController)
        {
            this.playerController = playerController;

            var collider = playerController.GetComponent<Collider2D>();
            rigidbody2D = collider.attachedRigidbody;
        }

        public void OnStateEnter()
        {
            // Left blank intentionally
        }

        public void OnStateUpdate()
        {
            var jumpKey = playerController.Configuration.JumpKey;
            if (Input.GetKeyDown(jumpKey))
            {
                Jump();
                return;
            }

            // TODO: Move to the configuration
            direction = Input.GetAxisRaw("Vertical");
        }

        public void OnStateFixedUpdate()
        {
            rigidbody2D.velocity =
                new Vector2(
                    rigidbody2D.velocity.x,
                    direction * playerController.Configuration.ClimbingSpeed);
        }

        public void OnStateExit()
        {
            direction = 0;
        }

        private void Jump()
        {
            // TODO: Move to the configuration
            var horizontal = Input.GetAxisRaw("Horizontal");
            if (Mathf.Abs(horizontal) > 0)
            {
                playerController.ChangeDirection(
                    horizontal < 0 ? Directions.Left : Directions.Right);

                var force = new Vector2(horizontal, 1);
                rigidbody2D.AddForce(
                    force * (playerController.Configuration.JumpForce / 2),
                    ForceMode2D.Impulse);
            }

            playerController.ChangePlayerState(PlayerState.Falling);
        }
    }
}