using UnityEngine;

namespace Scripts.Gameplay.Player.States
{
    public class PlayerSecondaryAttackState : IPlayerStateBehaviour
    {
        private readonly PlayerController playerController;
        private readonly Rigidbody2D rigidbody2D;

        private float previousTime;

        public PlayerSecondaryAttackState(PlayerController playerController)
        {
            this.playerController = playerController;

            var collider = playerController.GetComponent<Collider2D>();
            rigidbody2D = collider.attachedRigidbody;
        }

        public void OnStateEnter()
        {
            previousTime = Time.time;

            rigidbody2D.velocity = Vector2.zero;
        }

        public void OnStateUpdate()
        {
            if (IsGrounded())
            {
                if (Time.time > previousTime + 0.5f)
                {
                    playerController.SetPlayerState(PlayerStates.Idle);
                }
            }
            else
            {
                playerController.SetPlayerState(PlayerStates.Falling);
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