using Game.Common;
using Scripts.UI.Controllers;
using UnityEngine;

namespace Scripts.Gameplay.Actors
{
    public class PlayerIdleState : IPlayerStateBehaviour
    {
        private readonly PlayerController playerController;
        private readonly FocusStateController focusStateController;
        private readonly Rigidbody2D rigidbody2D;
 
        public PlayerIdleState(
            PlayerController playerController,
            FocusStateController focusStateController)
        {
            this.playerController = playerController;
            this.focusStateController = focusStateController;

            var collider = playerController.GetComponent<Collider2D>();
            rigidbody2D = collider.attachedRigidbody;
        }

        public void OnStateEnter()
        {
            rigidbody2D.Sleep();
        }

        public void OnStateUpdate()
        {
            if (IsGrounded())
            {
                if (IsGameFocused())
                {
                    if (IsMoved())
                    {
                        playerController.ChangePlayerState(PlayerState.Moving);
                    }

                    if (IsJumpKeyClicked())
                    {
                        playerController.ChangePlayerState(PlayerState.Jumping);
                    }
                }
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

        private bool IsGrounded()
        {
            return playerController.IsGrounded();
        }

        private bool IsGameFocused()
        {
            return focusStateController?.GetFocusState() == FocusState.Game;
        }

        private bool IsMoved()
        {
            // TODO: Move to the configuration
            var horizontal = Input.GetAxisRaw("Horizontal");
            return Mathf.Abs(horizontal) > 0;
        }

        private bool IsJumpKeyClicked()
        {
            var jumpKey = playerController.Configuration.JumpKey;
            return Input.GetKeyDown(jumpKey);
        }
    }
}