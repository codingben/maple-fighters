using System;
using System.Linq;
using CommonTools.Log;
using Scripts.Utils.Shared;
using Shared.Game.Common;
using UnityEngine;

namespace Scripts.Gameplay.Actors
{
    public class PlayerController : MonoBehaviour
    {
        private PlayerState PlayerState
        {
            set
            {
                playerState = value;
                playerStateChanged?.Invoke(value);
            }
            get
            {
                return playerState;
            }
        }
        private Action<PlayerState> playerStateChanged;

        [SerializeField] private PlayerState playerState = PlayerState.Falling;

        [Header("General")]
        [SerializeField] private float speed;
        [SerializeField] private float jumpForce;

        [Header("Ground Detection")]
        [SerializeField] private LayerMask floorLayerMask;
        [SerializeField] private Transform[] floorDetectionPoints;

        [Header("Debug")]
        [SerializeField] private float direction;

        [Header("Other")]
        [SerializeField] private PlayerStateNetworkAnimator playerAnimator;

        private new Rigidbody2D rigidbody;

        private void Awake()
        {
            rigidbody = GetComponent<Rigidbody2D>().AssertNotNull();
            playerStateChanged = playerAnimator.AssertNotNull().OnPlayerStateChanged;
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

            SetPlayerState(directions);

            direction = GetDirecton(directions) * speed;

            FlipByDirection(directions);
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
        }

        private float GetDirecton(Directions direction)
        {
            var dir = 0.0f;

            switch (direction)
            {
                case Directions.None:
                {
                    dir = 0;
                    break;
                }
                case Directions.Left:
                {
                    dir = -1;
                    break;
                }
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
            return floorDetectionPoints.Any(ground => Physics2D.OverlapPoint(ground.position, floorLayerMask));
        }
    }
}