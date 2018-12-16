using Game.Common;
using Scripts.UI.Controllers;
using UnityEngine;

namespace Scripts.Gameplay.Actors
{
    public class PlayerMovingState : IPlayerStateBehaviour
    {
        private IPlayerController playerController;
        private float direction;

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

            if (FocusController.GetInstance().Focusable != Focusable.Game)
            {
                playerController.PlayerState = PlayerState.Idle;
                return;
            }

            var jumpKey = playerController.Config.JumpKey;
            if (Input.GetKeyDown(jumpKey))
            {
                playerController.PlayerState = PlayerState.Jumping;
                return;
            }

            var horizontal = Input.GetAxisRaw("Horizontal");
            if (Mathf.Abs(horizontal) == 0)
            {
                playerController.PlayerState = PlayerState.Idle;
                return;
            }

            direction = horizontal;
            playerController.Direction = direction < 0 ? Directions.Left : Directions.Right;
        }

        public void OnStateFixedUpdate()
        {
            var rigidbody = playerController.Rigidbody;
            rigidbody.velocity = new Vector2(direction * playerController.Config.Speed, rigidbody.velocity.y);
        }

        public void OnStateExit()
        {
            direction = 0;
        }
    }
}