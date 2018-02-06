using Shared.Game.Common;
using UnityEngine;

namespace Scripts.Gameplay.Actors
{
    public class PlayerClimbingState : IPlayerStateBehaviour
    {
        private IPlayerController playerController;
        private float direction;

        public void OnStateEnter(IPlayerController playerController)
        {
            if (this.playerController == null)
            {
                this.playerController = playerController;
            }
        }

        public void OnStateUpdate()
        {
            if (Input.GetKeyDown(PlayerJumpingState.JUMP_KEY))
            {
                Jump();
                return;
            }

            var vertical = Input.GetAxisRaw("Vertical");
            direction = vertical;
        }

        public void OnStateFixedUpdate()
        {
            const float SPEED = 1.5f;

            var rigidbody = playerController.Rigidbody;
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, direction * SPEED);
        }

        public void OnStateExit()
        {
            direction = 0;
        }

        private void Jump()
        {
            var horizontal = Input.GetAxisRaw("Horizontal");
            if (Mathf.Abs(horizontal) > 0)
            {
                var forceDirection = new Vector2(horizontal, 1);
                playerController.Rigidbody.AddForce(forceDirection * (PlayerJumpingState.JUMP_FORCE / 2), ForceMode2D.Impulse);
                playerController.Direction = direction < 0 ? Directions.Left : Directions.Right;
            }

            playerController.PlayerState = PlayerState.Falling;
        }
    }
}