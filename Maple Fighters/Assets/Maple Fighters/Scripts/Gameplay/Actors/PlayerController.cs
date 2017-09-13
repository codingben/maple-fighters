using System.Linq;
using CommonTools.Log;
using UnityEngine;

namespace Scripts.Gameplay.Actors
{
    public class PlayerController : MonoBehaviour
    {
        private enum PlayerState
        {
            Idle,
            Moving,
            Falling
        }

        [SerializeField] private PlayerState playerState = PlayerState.Falling;

        [Header("Properties")]
        [SerializeField] private float speed;
        [SerializeField] private float jumpForce;

        [Header("Ground Detection")]
        [SerializeField] private LayerMask floorLayerMask;
        [SerializeField] private Transform[] floorDetectionPoints;

        [Header("Debug")]
        [SerializeField] private float direction;

        private new Rigidbody2D rigidbody;

        private void Awake()
        {
            rigidbody = GetComponent<Rigidbody2D>().AssertNotNull();
        }

        private void Update()
        {
            switch (playerState)
            {
                case PlayerState.Idle:
                {
                    IdleState();
                    break;
                }
                case PlayerState.Moving:
                {
                    MovingState();
                    break;
                }
                case PlayerState.Falling:
                {
                    FallingState();
                    break;
                }
            }
        }

        private void FixedUpdate()
        {
            rigidbody.velocity = new Vector2(direction * speed * Time.fixedDeltaTime, rigidbody.velocity.y);
        }

        public void Move(Directions directions)
        {
            if (!IsOnFloor())
            {
                return;
            }

            switch (directions)
            {
                case Directions.None:
                {
                    playerState = PlayerState.Idle;
                    break;
                }
                case Directions.Left:
                case Directions.Right:
                {
                    playerState = PlayerState.Moving;
                    break;
                }
            }

            direction = (int)directions * speed;
        }

        public void Jump()
        {
            if (playerState != PlayerState.Idle && playerState != PlayerState.Moving)
            {
                return;
            }

            var forceDirection = new Vector2(direction, 1);
            rigidbody.AddForce(forceDirection * jumpForce, ForceMode2D.Impulse);

            playerState = PlayerState.Falling;
        }

        private void IdleState()
        {
            if (!IsOnFloor())
            {
                playerState = PlayerState.Falling;
            }

            direction = 0;
        }

        private void MovingState()
        {
            if (!IsOnFloor())
            {
                playerState = PlayerState.Falling;
            }
        }

        private void FallingState()
        {
            if (IsOnFloor())
            {
                playerState = PlayerState.Idle;
            }
        }

        private bool IsOnFloor()
        {
            return floorDetectionPoints.Any(ground => Physics2D.OverlapPoint(ground.position, floorLayerMask));
        }
    }
}