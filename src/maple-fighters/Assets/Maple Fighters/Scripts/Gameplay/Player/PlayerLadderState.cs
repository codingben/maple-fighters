using UnityEngine;

namespace Scripts.Gameplay.Actors
{
    public class PlayerLadderState : IPlayerStateBehaviour
    {
        private readonly PlayerController playerController;
        private readonly Rigidbody2D rigidbody2D;

        private float direction;

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
            direction = Utils.GetAxis(Axes.Vertical, isRaw: true);
        }

        public void OnStateFixedUpdate()
        {
            var speed = playerController.Properties.LadderClimbingSpeed;
            rigidbody2D.velocity =
                new Vector2(rigidbody2D.velocity.x, direction * speed);
        }

        public void OnStateExit()
        {
            direction = 0;
        }
    }
}