using Game.Common;
using UnityEngine;

namespace Scripts.Gameplay.Actors
{
    public class PlayerAttackedState : IPlayerStateBehaviour
    {
        private readonly PlayerController playerController;
        private readonly Rigidbody2D rigidbody2D;

        public PlayerAttackedState(PlayerController playerController)
        {
            this.playerController = playerController;

            var collider = playerController.GetComponent<Collider2D>();
            rigidbody2D = collider.attachedRigidbody;
        }

        public void OnStateEnter()
        {
            // TODO: Make sure it won't cause a bug
            rigidbody2D.velocity = Vector2.zero;
        }

        public void OnStateUpdate()
        {
            if (playerController.IsGrounded())
            {
                playerController.ChangePlayerState(PlayerState.Idle);
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