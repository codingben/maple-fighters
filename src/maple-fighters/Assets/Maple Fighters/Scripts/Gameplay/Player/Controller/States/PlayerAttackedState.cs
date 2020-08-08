using UnityEngine;

namespace Scripts.Gameplay.Player.States
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
            rigidbody2D.Sleep();
        }

        public void OnStateUpdate()
        {
            if (IsGrounded())
            {
                playerController.SetPlayerState(PlayerStates.Idle);
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

        private bool IsGrounded()
        {
            return playerController.IsGrounded();
        }
    }
}