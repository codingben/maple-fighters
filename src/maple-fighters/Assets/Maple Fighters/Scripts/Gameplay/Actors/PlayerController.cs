using System;
using System.Collections.Generic;
using System.Linq;
using Game.Common;
using Scripts.Editor;
using Scripts.UI.Controllers;
using UnityEngine;

#pragma warning disable 0109

namespace Scripts.Gameplay.Actors
{
    [RequireComponent(typeof(Collider2D))]
    public class PlayerController : MonoBehaviour
    {
        public event Action<PlayerState> PlayerStateChanged;

        public PlayerControllerConfig Configuration => config;

        public PlayerState PlayerState => playerState;

        [Header("Debug")]
        [ViewOnly, SerializeField]
        private PlayerState playerState = PlayerState.Falling;

        [Header("Properties")]
        [SerializeField]
        private PlayerControllerConfig config;

        [Header("Ground")]
        [SerializeField]
        private LayerMask groundLayerMask;

        [SerializeField]
        private Transform[] groundDetectionPoints;

        private Dictionary<PlayerState, IPlayerStateBehaviour>
            playerStateBehaviours;

        private IPlayerStateBehaviour playerStateBehaviour;
        private new Rigidbody2D rigidbody2D;

        private void Awake()
        {
            var focusStateController = FindObjectOfType<FocusStateController>();
            if (focusStateController == null)
            {
                Debug.LogError(
                    "PlayerController::Awake() -> Could not find FocusStateController!");
            }

            playerStateBehaviours =
                new Dictionary<PlayerState, IPlayerStateBehaviour>
                {
                    {
                        PlayerState.Idle,
                        new PlayerIdleState(this, focusStateController)
                    },
                    {
                        PlayerState.Moving,
                        new PlayerMovingState(this, focusStateController)
                    },
                    { PlayerState.Jumping, new PlayerJumpingState(this) },
                    { PlayerState.Falling, new PlayerFallingState(this) },
                    { PlayerState.Attacked, new PlayerAttackedState(this) },
                    { PlayerState.Rope, new PlayerRopeState(this) },
                    { PlayerState.Ladder, new PlayerLadderState(this) },
                };

            playerStateBehaviour = playerStateBehaviours[PlayerState.Idle];
        }

        private void Start()
        {
            playerStateBehaviour.OnStateEnter();
        }

        private void Update()
        {
            playerStateBehaviour.OnStateUpdate();
        }

        private void FixedUpdate()
        {
            playerStateBehaviour.OnStateFixedUpdate();
        }

        public void ChangePlayerState(PlayerState newPlayerState)
        {
            if (playerState == newPlayerState)
            {
                return;
            }

            playerStateBehaviour.OnStateExit();
            playerStateBehaviour = playerStateBehaviours[newPlayerState];
            playerStateBehaviour.OnStateEnter();

            playerState = newPlayerState;

            PlayerStateChanged?.Invoke(playerState);
        }

        public void ChangeDirection(Directions direction)
        {
            const float Scale = 1;
            
            if (direction == Directions.Left)
            {
                transform.localScale = new Vector3(
                    Scale,
                    transform.localScale.y,
                    transform.localScale.z);
            }
            else
            {
                transform.localScale = new Vector3(
                    -Scale,
                    transform.localScale.y,
                    transform.localScale.z);
            }
        }

        public void Bounce(Vector2 force)
        {
            if (rigidbody2D == null)
            {
                var collider = GetComponent<Collider2D>();
                rigidbody2D = collider.attachedRigidbody;
            }

            rigidbody2D.AddForce(force, ForceMode2D.Impulse);
        }

        public bool IsGrounded()
        {
            return groundDetectionPoints.Any(
                x => Physics2D.OverlapPoint(x.position, groundLayerMask));
        }
    }
}