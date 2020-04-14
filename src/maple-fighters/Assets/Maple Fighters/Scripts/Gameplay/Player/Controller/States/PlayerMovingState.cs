using System;
using Game.Common;
using UnityEngine;

namespace Scripts.Gameplay.Player.States
{
    public class PlayerMovingState : IPlayerStateBehaviour
    {
        private readonly PlayerController playerController;
        private readonly Rigidbody2D rigidbody2D;

        private Directions direction;

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
                if (IsMoveStopped())
                {
                    playerController.ChangePlayerState(PlayerState.Idle);
                }

                if (IsJumpKeyClicked() && CanJump())
                {
                    playerController.ChangePlayerState(PlayerState.Jumping);
                }

                var horizontal = Utils.GetAxis(Axes.Horizontal, isRaw: true);
                if (Math.Abs(horizontal) > 0)
                {
                    direction = 
                        horizontal < 0 ? Directions.Left : Directions.Right;
                }
                
                playerController.ChangeDirection(direction);
            }
            else
            {
                playerController.ChangePlayerState(PlayerState.Falling);
            }
        }

        public void OnStateFixedUpdate()
        {
            var speed = playerController.Properties.Speed;
            var horizontal = Utils.GetAxis(Axes.Horizontal);
            var y = rigidbody2D.velocity.y;

            rigidbody2D.velocity = new Vector3(horizontal * speed, y);
        }

        public void OnStateExit()
        {
            // Left blank intentionally
        }

        private bool IsGrounded()
        {
            return playerController.IsGrounded();
        }

        private bool IsJumpKeyClicked()
        {
            var jumpKey = playerController.Properties.JumpKey;
            return Input.GetKeyDown(jumpKey);
        }

        private bool IsMoveStopped()
        {
            return playerController.IsMoving() == false;
        }

        private bool CanJump()
        {
            return playerController.CanJump();
        }
    }
}