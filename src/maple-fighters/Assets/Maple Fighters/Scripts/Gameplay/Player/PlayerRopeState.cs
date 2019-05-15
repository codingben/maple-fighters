using Game.Common;
using UnityEngine;

namespace Scripts.Gameplay.Actors
{
    public class PlayerRopeState : IPlayerStateBehaviour
    {
        private readonly PlayerController playerController;
        private readonly Rigidbody2D rigidbody2D;

        private float direction;

        public PlayerRopeState(PlayerController playerController)
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
            if (IsJumpKeyClicked())
            {
                Jump();

                playerController.ChangePlayerState(PlayerState.Falling);
            }
            else
            {
                // TODO: Move to the configuration
                direction = Input.GetAxisRaw("Vertical");
            }
        }

        public void OnStateFixedUpdate()
        {
            var speed = playerController.Configuration.RopeClimbingSpeed;
            rigidbody2D.velocity = 
                new Vector2(rigidbody2D.velocity.x, direction * speed);
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
                var direction = 
                    horizontal < 0 ? Directions.Left : Directions.Right;
                playerController.ChangeDirection(direction);

                var jumpForce = playerController.Configuration.JumpForce;
                var force = new Vector2(horizontal, 1) * (jumpForce / 2);
                rigidbody2D.AddForce(force, ForceMode2D.Impulse);
            }
        }

        private bool IsJumpKeyClicked()
        {
            var jumpKey = playerController.Configuration.JumpKey;
            return Input.GetKeyDown(jumpKey);
        }
    }
}