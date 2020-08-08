using UnityEngine;

namespace Scripts.Gameplay.Player.States
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
            rigidbody2D.velocity = Vector2.zero;
        }

        public void OnStateUpdate()
        {
            if (IsGrounded())
            {
                if (IsMoving())
                {
                    playerController.SetPlayerState(PlayerStates.Moving);
                }

                if (IsJumpKeyClicked() && CanJump())
                {
                    playerController.SetPlayerState(PlayerStates.Jumping);
                }
            }
            else
            {
                playerController.SetPlayerState(PlayerStates.Falling);
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

        private bool IsMoving()
        {
            return playerController.IsMoving();
        }

        private bool CanJump()
        {
            return playerController.CanJump();
        }

        private bool IsJumpKeyClicked()
        {
            var jumpKey = playerController.GetKeyboardSettings().JumpKey;
            return Input.GetKeyDown(jumpKey);
        }
    }
}