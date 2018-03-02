using Game.Common;
using UnityEngine;

namespace Scripts.Gameplay.Actors
{
    public class PlayerAttackedState : IPlayerStateBehaviour
    {
        private IPlayerController playerController;
        private bool isOnGround;

        public void OnStateEnter(IPlayerController playerController)
        {
            if (this.playerController == null)
            {
                this.playerController = playerController;
            }

            playerController.Rigidbody.velocity = Vector2.zero;
        }

        public void OnStateUpdate()
        {
            if (playerController.IsOnGround() && !isOnGround)
            {
                return;
            }

            if (playerController.IsOnGround())
            {
                playerController.PlayerState = PlayerState.Idle;
                return;
            }

            if (!isOnGround)
            {
                isOnGround = true;
            }
        }

        public void OnStateFixedUpdate()
        {
            // Left blank intentionally
        }

        public void OnStateExit()
        {
            isOnGround = false;
        }
    }
}