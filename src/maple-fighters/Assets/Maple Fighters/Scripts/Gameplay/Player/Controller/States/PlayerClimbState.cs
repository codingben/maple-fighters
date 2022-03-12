using UnityEngine;

namespace Scripts.Gameplay.Player.States
{
    public class PlayerClimbState : IPlayerStateBehaviour
    {
        private readonly PlayerController playerController;
        private readonly Rigidbody2D rigidbody2D;

        private float previousTime;
        private float previousGravityScale;

        public PlayerClimbState(PlayerController playerController)
        {
            this.playerController = playerController;

            var collider = playerController.GetComponent<Collider2D>();
            rigidbody2D = collider.attachedRigidbody;
        }

        public void OnStateEnter()
        {
            previousTime = Time.time;
            previousGravityScale = rigidbody2D.gravityScale;

            rigidbody2D.gravityScale = default;
        }

        public void OnStateUpdate()
        {
            if (IsJumpKeyClicked())
            {
                playerController.SetPlayerState(PlayerStates.Falling);
            }
            else
            {
                if (Time.time > previousTime + 1)
                {
                    var isMoving =
                        Mathf.Abs(Utils.GetAxis(Axes.Vertical, isRaw: true));

                    SetAnimatorState(active: isMoving > 0);
                }
            }
        }

        public void OnStateFixedUpdate()
        {
            Move();
        }

        public void OnStateExit()
        {
            rigidbody2D.gravityScale = previousGravityScale;

            SetAnimatorState(active: true);
        }

        private void Move()
        {
            var direction = Utils.GetAxis(Axes.Vertical);
            var speed = playerController.GetProperties().ClimbSpeed;
            var x = rigidbody2D.velocity.x;
            var y = direction * speed;

            rigidbody2D.velocity = new Vector2(x, y);
        }

        private void SetAnimatorState(bool active)
        {
            var animator = playerController.GetPlayerStateAnimator();
            if (animator != null)
            {
                animator.Enabled = active;
            }
        }

        private bool IsJumpKeyClicked()
        {
            var key = playerController.GetKeyboardSettings().JumpKey;

            return Input.GetKeyDown(key);
        }
    }
}