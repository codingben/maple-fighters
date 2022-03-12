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
                if (CanMove())
                {
                    if (IsMoving())
                    {
                        playerController.SetPlayerState(PlayerStates.Moving);
                    }

                    if (IsJumpKeyClicked())
                    {
                        playerController.SetPlayerState(PlayerStates.Jumping);
                    }

                    if (IsPrimaryAttackingKeyClicked())
                    {
                        playerController.SetPlayerState(PlayerStates.PrimaryAttack);
                    }

                    if (IsSecondaryAttackingKeyClicked())
                    {
                        playerController.SetPlayerState(PlayerStates.SecondaryAttack);
                    }
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

        private bool CanMove()
        {
            return playerController.CanMove();
        }

        private bool IsGrounded()
        {
            return playerController.IsGrounded();
        }

        private bool IsMoving()
        {
            return playerController.IsMoving();
        }

        private bool IsJumpKeyClicked()
        {
            var key = playerController.GetKeyboardSettings().JumpKey;

            return Input.GetKeyDown(key);
        }

        private bool IsPrimaryAttackingKeyClicked()
        {
            var key = playerController.GetKeyboardSettings().PrimaryAttackKey;

            return Input.GetKeyDown(key);
        }

        private bool IsSecondaryAttackingKeyClicked()
        {
            var key = playerController.GetKeyboardSettings().SecondaryAttackKey;

            return Input.GetKeyDown(key);
        }
    }
}