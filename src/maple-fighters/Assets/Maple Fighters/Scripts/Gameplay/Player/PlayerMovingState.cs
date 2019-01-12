using Game.Common;
using Scripts.UI.Controllers;
using UnityEngine;

namespace Scripts.Gameplay.Actors
{
    public class PlayerMovingState : IPlayerStateBehaviour
    {
        private readonly PlayerController playerController;
        private readonly Rigidbody2D rigidbody2D;

        private float direction;

        public PlayerMovingState(PlayerController playerController)
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
            if (playerController.IsGrounded())
            {
                if (FocusStateController.GetInstance().GetFocusState()
                    != FocusState.Game)
                {
                    playerController.ChangePlayerState(PlayerState.Idle);
                    return;
                }

                var jumpKey = playerController.Configuration.JumpKey;
                if (Input.GetKeyDown(jumpKey))
                {
                    playerController.ChangePlayerState(PlayerState.Jumping);
                    return;
                }

                // TODO: Move to the configuration
                var horizontal = Input.GetAxisRaw("Horizontal");
                if (Mathf.Abs(horizontal) == 0)
                {
                    playerController.ChangePlayerState(PlayerState.Idle);
                    return;
                }

                direction = horizontal;

                playerController.ChangeDirection(
                    direction < 0 ? Directions.Left : Directions.Right);
            }
            else
            {
                playerController.ChangePlayerState(PlayerState.Falling);
            }
        }

        public void OnStateFixedUpdate()
        {
            rigidbody2D.velocity = 
                new Vector2(
                    direction * playerController.Configuration.Speed,
                    rigidbody2D.velocity.y);
        }

        public void OnStateExit()
        {
            direction = 0;
        }
    }
}