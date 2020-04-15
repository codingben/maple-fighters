using Game.Common;
using UnityEngine;

namespace Scripts.Gameplay.Player.States
{
    public class PlayerLadderState : IPlayerStateBehaviour
    {
        private readonly PlayerController playerController;
        private readonly Rigidbody2D rigidbody2D;

        private float previousTime;
        private float previousGravityScale;

        public PlayerLadderState(PlayerController playerController)
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
                playerController.ChangePlayerState(PlayerState.Falling);
            }
            else
            {
                if (Time.time > previousTime + 1)
                {
                    var isMoving =
                        Mathf.Abs(Utils.GetAxis(Axes.Vertical, isRaw: true));
                    playerController.PlayerStateAnimator.Enabled = isMoving > 0;
                }
            }
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
            rigidbody2D.gravityScale = previousGravityScale;
            playerController.PlayerStateAnimator.Enabled = true;
        }

        private bool IsJumpKeyClicked()
        {
            var jumpKey = playerController.Properties.JumpKey;
            return Input.GetKeyDown(jumpKey);
        }
    }
}