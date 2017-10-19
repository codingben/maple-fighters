using System;
using System.Linq;
using Shared.Game.Common;
using UnityEngine;

#pragma warning disable 0109

namespace Scripts.Gameplay.Actors
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : MonoBehaviour
    {
        public bool DetectGround { get; set; } = true;

        public PlayerState PlayerState
        {
            private set
            {
                playerState = value;
                PlayerStateChanged?.Invoke(value);
            }
            get
            {
                return playerState;
            }
        }

        public Action<PlayerState> PlayerStateChanged;
        public event Action<Directions> ChangedDirection;

        [SerializeField] private PlayerState playerState = PlayerState.Falling;

        [Header("General")]
        [SerializeField] private float speed;
        [SerializeField] private float jumpForce;

        [Header("Rope Speed")]
        [SerializeField] private float ropeUpSpeed;
        [SerializeField] private float ropeDownSpeed;

        [Header("Ladder Speed")]
        [SerializeField] private float ladderUpSpeed;
        [SerializeField] private float ladderDownSpeed;

        [Header("Ground Detection")]
        [SerializeField] private LayerMask floorLayerMask;
        [SerializeField] private Transform[] floorDetectionPoints;

        [Header("Debug")]
        [SerializeField] private float direction;

        private new Rigidbody2D rigidbody;

        private void Awake()
        {
            rigidbody = GetComponent<Rigidbody2D>();
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
                case PlayerState.Rope:
                case PlayerState.Ladder:
                {
                    FallingState();
                    break;
                }
            }
        }

        private void FixedUpdate()
        {
            if (playerState == PlayerState.Rope || playerState == PlayerState.Ladder)
            {
                rigidbody.velocity = new Vector2(rigidbody.velocity.x, direction * speed * Time.fixedDeltaTime);
            }
            else
            {
                rigidbody.velocity = new Vector2(direction * speed * Time.fixedDeltaTime, rigidbody.velocity.y);
            }
        }

        public void Move(Directions directions)
        {
            if (!IsOnFloor())
            {
                return;
            }

            SetPlayerState(directions);

            direction = GetDirecton(directions) * speed;

            FlipByDirection(directions);
        }

        public void MoveOnRope(Directions directions)
        {
            var speedTemp = 0f;

            switch (directions)
            {
                case Directions.Up:
                {
                    speedTemp = ropeUpSpeed;
                    break;
                }
                case Directions.Down:
                {
                    speedTemp = ropeDownSpeed;
                    break;
                }
            }

            direction = GetDirecton(directions) * speedTemp;
        }

        public void MoveOnLadder(Directions directions)
        {
            var speedTemp = 0f;

            switch (directions)
            {
                case Directions.Up:
                {
                    speedTemp = ladderUpSpeed;
                    break;
                }
                case Directions.Down:
                {
                    speedTemp = ladderDownSpeed;
                    break;
                }
            }

            direction = GetDirecton(directions) * speedTemp;
        }

        public void Jump()
        {
            if (PlayerState != PlayerState.Idle && PlayerState != PlayerState.Moving)
            {
                return;
            }

            var forceDirection = new Vector2(direction, 1);
            rigidbody.AddForce(forceDirection * jumpForce, ForceMode2D.Impulse);

            PlayerState = PlayerState.Falling;
        }

        public void SetStateFromRopeOrLadderInteraction(PlayerState state)
        {
            playerState = state;
            PlayerStateChanged?.Invoke(state);

            rigidbody.velocity = new Vector2(0, rigidbody.velocity.y);
        }

        private void IdleState()
        {
            if (!IsOnFloor())
            {
                PlayerState = PlayerState.Falling;
            }

            direction = 0;
        }

        private void MovingState()
        {
            if (!IsOnFloor())
            {
                PlayerState = PlayerState.Falling;
            }
        }

        private void FallingState()
        {
            if (IsOnFloor())
            {
                PlayerState = PlayerState.Idle;
            }
        }

        private void SetPlayerState(Directions directions)
        {
            switch (directions)
            {
                case Directions.None:
                {
                    PlayerState = PlayerState.Idle;
                    break;
                }
                case Directions.Left:
                case Directions.Right:
                {
                    PlayerState = PlayerState.Moving;
                    break;
                }
            }
        }

        private void FlipByDirection(Directions direction)
        {
            switch (direction)
            {
                case Directions.Left:
                {
                    transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
                    break;
                }
                case Directions.Right:
                {
                    transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
                    break;
                }
            }

            ChangedDirection?.Invoke(direction);
        }

        private float GetDirecton(Directions direction)
        {
            var dir = 0f;

            switch (direction)
            {
                case Directions.None:
                {
                    dir = 0;
                    break;
                }
                case Directions.Down:
                case Directions.Left:
                {
                    dir = -1;
                    break;
                }
                case Directions.Up:
                case Directions.Right:
                {
                    dir = 1;
                    break;
                }
            }
            return dir;
        }

        private bool IsOnFloor()
        {
            return DetectGround && floorDetectionPoints.Any(ground => Physics2D.OverlapPoint(ground.position, floorLayerMask));
        }
    }
}