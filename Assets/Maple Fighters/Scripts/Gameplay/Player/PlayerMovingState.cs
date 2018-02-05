using Shared.Game.Common;
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

            direction = Input.GetAxisRaw("Horizontal"); // TODO: Change
        }

        public void OnStateFixedUpdate()
        {
            const float SPEED = 10; // TODO: Remove this from here

            var rigidbody = playerController.Rigidbody;
            rigidbody.velocity = new Vector2(direction * SPEED * Time.fixedDeltaTime, rigidbody.velocity.y);
        }

        public void OnStateExit()
        {
            direction = 0;
        }
    }
}