using Shared.Game.Common;
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

            playerController.Rigidbody.velocity = Vector2.zero;
        }

        public void OnStateUpdate()
        {
            if (!playerController.IsOnGround())
            {
                playerController.PlayerState = PlayerState.Falling;
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