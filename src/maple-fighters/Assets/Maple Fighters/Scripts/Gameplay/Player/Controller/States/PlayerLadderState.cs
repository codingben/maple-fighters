using UnityEngine;

namespace Scripts.Gameplay.Player.States
{
    public class PlayerLadderState : IPlayerStateBehaviour
    {
        private readonly PlayerController playerController;
        private readonly Rigidbody2D rigidbody2D;

        public PlayerLadderState(PlayerController playerController)
        {
            this.playerController = playerController;

            var collider = playerController.GetComponent<Collider2D>();
            rigidbody2D = collider.attachedRigidbody;
        }

        public void OnStateEnter()
        {
            // Left blank intentionally
        }

        public void OnStateUpdate()
        {
            // Left blank intentionally
        }

        public void OnStateFixedUpdate()
        {
            var direction = Utils.GetAxis(Axes.Vertical);
            var speed = playerController.Properties.LadderSpeed;
            var x = rigidbody2D.velocity.x;

            rigidbody2D.velocity = new Vector2(x, direction * speed);
        }

        public void OnStateExit()
        {
            // Left blank intentionally
        }
    }
}