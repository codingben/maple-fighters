using UnityEngine;

namespace Scripts.Gameplay.Actors
{
    public class PlayerAttackedState : IPlayerStateBehaviour
    {
        public void OnStateEnter(IPlayerController playerController)
        {
            playerController.Rigidbody.velocity = Vector2.zero;
        }

        public void OnStateUpdate()
        {
            // Left blank intentionally
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