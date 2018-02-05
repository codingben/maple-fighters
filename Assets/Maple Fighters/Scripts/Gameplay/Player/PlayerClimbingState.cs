using Shared.Game.Common;
using UnityEngine;

namespace Scripts.Gameplay.Actors
{
    public class PlayerClimbingState : IPlayerStateBehaviour
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
            if (playerController.IsOnGround())
            {
                playerController.PlayerState = PlayerState.Idle;
                return;
            }

            direction = Input.GetAxisRaw("Vertical"); // TODO: Change
        }

        public void OnStateFixedUpdate()
        {
            const float SPEED = 10; // TODO: Remove this from here

            var rigidbody = playerController.Rigidbody;
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, direction * SPEED * Time.fixedDeltaTime);
        }

        public void OnStateExit()
        {
            direction = 0;
        }
    }
}