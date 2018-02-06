using Shared.Game.Common;
using UnityEngine;

namespace Scripts.Gameplay.Actors
{
    public class PlayerJumpingState : IPlayerStateBehaviour
    {
        public const KeyCode JUMP_KEY = KeyCode.Space;
        public const float JUMP_FORCE = 5;

        private IPlayerController playerController;
        private bool isJumping;

        public void OnStateEnter(IPlayerController playerController)
        {
            if (this.playerController == null)
            {
                this.playerController = playerController;
            }

            const float JUMP_DIRECTION_FORCE = 0.075f;

            var direction = playerController.Rigidbody.velocity.x;
            if (direction != 0)
            {
                direction = playerController.Rigidbody.velocity.x > 0 ? JUMP_DIRECTION_FORCE : -JUMP_DIRECTION_FORCE;
            }

            var forceDirection = new Vector2(direction, 1);
            playerController.Rigidbody.AddForce(forceDirection * JUMP_FORCE, ForceMode2D.Impulse);
        }

        public void OnStateUpdate()
        {
            if (playerController.IsOnGround() && !isJumping)
            {
                return;
            }

            if (playerController.IsOnGround())
            {
                playerController.PlayerState = PlayerState.Idle;
                return;
            }

            if (!isJumping)
            {
                isJumping = true;
            }
        }

        public void OnStateFixedUpdate()
        {
            // Left blank intentionally
        }

        public void OnStateExit()
        {
            isJumping = false;
        }
    }
}