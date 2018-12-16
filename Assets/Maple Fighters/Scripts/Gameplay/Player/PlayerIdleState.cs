using Game.Common;
using Scripts.UI.Controllers;
using UnityEngine;

namespace Scripts.Gameplay.Actors
{
    public class PlayerIdleState : IPlayerStateBehaviour
    {
        private IPlayerController playerController;

        public void OnStateEnter(IPlayerController playerController)
        {
            if (this.playerController == null)
            {
                this.playerController = playerController;
            }
        }

        public void OnStateUpdate()
        {
            if (!playerController.IsOnGround())
            {
                playerController.PlayerState = PlayerState.Falling;
                return;
            }

            if (FocusController.GetInstance().Focusable == Focusable.Game)
            {
                var jumpKey = playerController.Config.JumpKey;
                if (Input.GetKeyDown(jumpKey))
                {
                    playerController.PlayerState = PlayerState.Jumping;
                    return;
                }

                var horizontal = Input.GetAxisRaw("Horizontal");
                if (Mathf.Abs(horizontal) > 0)
                {
                    playerController.PlayerState = PlayerState.Moving;
                    return;
                }
            }

            if (playerController.Rigidbody.velocity != Vector2.zero)
            {
                playerController.Rigidbody.velocity = Vector2.zero;
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