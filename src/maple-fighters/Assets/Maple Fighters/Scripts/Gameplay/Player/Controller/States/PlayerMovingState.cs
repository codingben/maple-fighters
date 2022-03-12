using System;
using UnityEngine;

namespace Scripts.Gameplay.Player.States
{
    public class PlayerMovingState : IPlayerStateBehaviour
    {
        private readonly PlayerController playerController;
        private readonly Rigidbody2D rigidbody2D;

        private Direction direction;
        private bool canMove;

        public PlayerMovingState(PlayerController playerController)
        {
            this.playerController = playerController;

            var collider = playerController.GetComponent<Collider2D>();
            rigidbody2D = collider.attachedRigidbody;
        }

        public void OnStateEnter()
        {
            canMove = true;
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

                    if (IsPrimaryAttackKeyClicked())
                    {
                        playerController.SetPlayerState(PlayerStates.PrimaryAttack);
                    }

                    if (IsSecondaryAttackKeyClicked())
                    {
                        playerController.SetPlayerState(PlayerStates.SecondaryAttack);
                    }

                    var horizontal = Utils.GetAxis(Axes.Horizontal, isRaw: true);

                    if (Math.Abs(horizontal) > 0)
                    {
                        direction =
                            horizontal < 0 ? Direction.Left : Direction.Right;
                    }

                    playerController.ChangeDirection(direction);
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
            if (canMove)
            {
                var speed = playerController.GetProperties().Speed;
                var horizontal = Utils.GetAxis(Axes.Horizontal);
                var y = rigidbody2D.velocity.y;

                rigidbody2D.velocity = new Vector3(horizontal * speed, y);
            }
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

        private bool IsJumpKeyClicked()
        {
            var key = playerController.GetKeyboardSettings().JumpKey;

            return Input.GetKeyDown(key);
        }

        private bool IsPrimaryAttackKeyClicked()
        {
            var key = playerController.GetKeyboardSettings().PrimaryAttackKey;

            return Input.GetKeyDown(key);
        }

        private bool IsSecondaryAttackKeyClicked()
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