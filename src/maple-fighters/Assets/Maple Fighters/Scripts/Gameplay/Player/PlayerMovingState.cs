using System;
using Game.Common;
using Scripts.UI.Controllers;
using UnityEngine;

namespace Scripts.Gameplay.Actors
{
    public class PlayerMovingState : IPlayerStateBehaviour
    {
        private readonly PlayerController playerController;
        private readonly FocusStateController focusStateController;
        private readonly Rigidbody2D rigidbody2D;

        private Directions direction;

        public PlayerMovingState(
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
            // Left blank intentionally
        }

        public void OnStateUpdate()
        {
            if (IsGrounded())
            {
                if (IsUnFocused())
                {
                    playerController.ChangePlayerState(PlayerState.Idle);
                }

                if (IsMoveStopped())
                {
                    playerController.ChangePlayerState(PlayerState.Idle);
                }

                if (IsJumpKeyClicked())
                {
                    playerController.ChangePlayerState(PlayerState.Jumping);
                }

                var horizontal = playerController.GetHorizontalRaw();
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
            var horizontal = playerController.GetHorizontalRaw();
            var speed = playerController.Properties.Speed;
            var position = rigidbody2D.transform.position;
            var direction = new Vector3(horizontal, 0, 0).normalized;
            var newPosition = position + (direction * speed * Time.deltaTime);
            
            rigidbody2D.MovePosition(newPosition);
        }

        public void OnStateExit()
        {
            // Left blank intentionally
        }

        private bool IsGrounded()
        {
            return playerController.IsGrounded();
        }

        private bool IsUnFocused()
        {
            return focusStateController?.GetFocusState() != FocusState.Game;
        }

        private bool IsJumpKeyClicked()
        {
            var jumpKey = playerController.Properties.JumpKey;
            return Input.GetKeyDown(jumpKey);
        }

        private bool IsMoveStopped()
        {
            var horizontal = playerController.GetHorizontalRaw();
            return Mathf.Abs(horizontal) == 0;
        }
    }
}