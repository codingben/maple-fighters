using Game.Common;
using Scripts.UI.Controllers;
using UnityEngine;

namespace Scripts.Gameplay.Actors
{
    public class PlayerIdleState : IPlayerStateBehaviour
    {
        private readonly PlayerController playerController;
        private readonly Rigidbody2D rigidbody2D;

        public PlayerIdleState(PlayerController playerController)
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
                if (FocusController.GetInstance().Focusable != Focusable.Game)
                {
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
                if (Mathf.Abs(horizontal) > 0)
                {
                    playerController.ChangePlayerState(PlayerState.Moving);
                    return;
                }

                rigidbody2D.velocity = Vector2.zero;
            }
            else
            {
                playerController.ChangePlayerState(PlayerState.Falling);
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
    }
}