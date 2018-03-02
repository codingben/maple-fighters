using Game.Common;
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
            if (Input.GetKeyDown(playerController.Config.JumpKey))
            {
                Jump();
                return;
            }

            var vertical = Input.GetAxisRaw("Vertical");
            direction = vertical;
        }

        public void OnStateFixedUpdate()
        {
            var rigidbody = playerController.Rigidbody;
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, direction * playerController.Config.ClimbingSpeed);
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
                playerController.Direction = horizontal < 0 ? Directions.Left : Directions.Right;

                var forceDirection = new Vector2(horizontal, 1);
                playerController.Rigidbody.AddForce(forceDirection * (playerController.Config.JumpForce / 2), ForceMode2D.Impulse);
            }

            playerController.PlayerState = PlayerState.Falling;
        }
    }
}