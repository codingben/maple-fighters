using UnityEngine;

namespace Scripts.Gameplay.Player.States
{
    public class PlayerRopeState : IPlayerStateBehaviour
    {
        private readonly PlayerController playerController;
        private readonly Rigidbody2D rigidbody2D;

        private float previousTime;
        private float previousGravityScale;

        public PlayerRopeState(PlayerController playerController)
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
                    playerController.GetPlayerStateAnimator().Enabled = isMoving > 0;
                }
            }
        }

        public void OnStateFixedUpdate()
        {
            var direction = Utils.GetAxis(Axes.Vertical);
            var speed = playerController.GetProperties().ClimbSpeed;
            var x = rigidbody2D.velocity.x;

            rigidbody2D.velocity = new Vector2(x, direction * speed);
        }

        public void OnStateExit()
        {
            rigidbody2D.gravityScale = previousGravityScale;
            playerController.GetPlayerStateAnimator().Enabled = true;
        }

        private bool IsJumpKeyClicked()
        {
            var jumpKey = playerController.GetKeyboardSettings().JumpKey;
            return Input.GetKeyDown(jumpKey);
        }
    }
}