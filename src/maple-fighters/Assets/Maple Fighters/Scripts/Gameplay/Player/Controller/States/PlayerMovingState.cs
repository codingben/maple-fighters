using System;
using UnityEngine;

namespace Scripts.Gameplay.Player.States
{
    public class PlayerMovingState : IPlayerStateBehaviour
    {
        private readonly PlayerController playerController;
        private readonly Rigidbody2D rigidbody2D;

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
            if (IsGrounded())
            {
                if (CanMove())
                {
                    if (IsMoveStopped())
                    {
                        playerController.SetPlayerState(PlayerStates.Idle);
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

                    SetDirection();
                }
                else
                {
                    playerController.SetPlayerState(PlayerStates.Idle);
                }
            }
            else
            {
                playerController.SetPlayerState(PlayerStates.Falling);
            }
        }

        public void OnStateFixedUpdate()
        {
            if (CanMove())
            {
                Move();
            }
        }

        public void OnStateExit()
        {
            // Left blank intentionally
        }

        private void Move()
        {
            var speed = playerController.GetProperties().Speed;
            var horizontal = Utils.GetAxis(Axes.Horizontal);
            var x = horizontal * speed;
            var y = rigidbody2D.velocity.y;

            rigidbody2D.velocity = new Vector3(x, y);
        }

        private void SetDirection()
        {
            var horizontal = Utils.GetAxis(Axes.Horizontal, isRaw: true);
            if (horizontal != 0)
            {
                var direction =
                    horizontal < 0 ? Direction.Left : Direction.Right;

                playerController.ChangeDirection(direction);
            }
        }

        private bool CanMove()
        {
            return playerController.CanMove();
        }

        private bool IsGrounded()
        {
            return playerController.IsGrounded();
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

        private bool IsMoveStopped()
        {
            return playerController.IsMoving() == false;
        }
    }
}