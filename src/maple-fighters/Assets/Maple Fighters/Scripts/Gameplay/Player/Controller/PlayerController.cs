using System.Collections.Generic;
using Game.Common;
using Scripts.Editor;
using Scripts.UI.Focus;
using UnityEngine;

#pragma warning disable 0109

namespace Scripts.Gameplay.Player
{
    [RequireComponent(typeof(Collider2D))]
    public class PlayerController : MonoBehaviour
    {
        public PlayerProperties Properties => properties;

        public PlayerState PlayerState => playerState;

        [Header("Debug")]
        [ViewOnly, SerializeField]
        private PlayerState playerState = PlayerState.Falling;

        [Header("Configuration")]
        [SerializeField]
        private PlayerProperties properties;

        [Header("Ground")]
        [SerializeField]
        private float overlapCircleRadius;

        [SerializeField]
        private LayerMask groundLayerMask;

        [SerializeField]
        private Transform groundTransform;

        private Dictionary<PlayerState, IPlayerStateBehaviour> playerStateBehaviours;

        private IPlayerStateBehaviour playerStateBehaviour;
        private IPlayerStateAnimator playerStateAnimator;

        private Vector2 localScale;
        private new Rigidbody2D rigidbody2D;

        private FocusStateController focusStateController;

        private void Awake()
        {
            playerStateBehaviours =
                new Dictionary<PlayerState, IPlayerStateBehaviour>
                {
                    {
                        PlayerState.Idle,
                        new PlayerIdleState(this)
                    },
                    {
                        PlayerState.Moving,
                        new PlayerMovingState(this)
                    },
                    {
                        PlayerState.Jumping,
                        new PlayerJumpingState(this)
                    },
                    {
                        PlayerState.Falling,
                        new PlayerFallingState(this)
                    },
                    {
                        PlayerState.Attacked,
                        new PlayerAttackedState(this)
                    },
                    {
                        PlayerState.Rope,
                        new PlayerRopeState(this)
                    },
                    { 
                        PlayerState.Ladder,
                        new PlayerLadderState(this)
                    }
                };

            playerStateBehaviour = playerStateBehaviours[playerState];
            localScale = transform.localScale;
            focusStateController = FindObjectOfType<FocusStateController>();
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

        public void SetPlayerStateAnimator(IPlayerStateAnimator playerStateAnimator)
        {
            this.playerStateAnimator = playerStateAnimator;
        }

        public void ChangePlayerState(PlayerState newPlayerState)
        {
            if (playerState != newPlayerState)
            {
                playerStateBehaviour.OnStateExit();
                playerStateBehaviour = playerStateBehaviours[newPlayerState];
                playerStateBehaviour.OnStateEnter();

                playerStateAnimator?.SetPlayerState(newPlayerState);
                playerState = newPlayerState;
            }
        }

        public void ChangePlayerStateToIdleOrFalling()
        {
            var playerState =
                IsGrounded() ? PlayerState.Idle : PlayerState.Falling;
            ChangePlayerState(playerState);
        }

        public void ChangeDirection(Directions direction)
        {
            if (direction == Directions.Left)
            {
                transform.localScale = new Vector3(
                    localScale.x,
                    transform.localScale.y,
                    transform.localScale.z);
            }
            else
            {
                transform.localScale = new Vector3(
                    -localScale.x,
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

        public bool IsMoving()
        {
            var isMoving = false;

            if (focusStateController?.GetFocusState() == FocusState.Game)
            {
                var horizontal = Utils.GetAxis(Axes.Horizontal, isRaw: true);
                isMoving = Mathf.Abs(horizontal) > 0;
            }

            return isMoving;
        }

        public bool CanJump()
        {
            return focusStateController?.GetFocusState() == FocusState.Game;
        }

        public bool IsGrounded()
        {
            var isGrounded = false;

            if (groundTransform != null)
            {
                var position = groundTransform.position;
                var radius = overlapCircleRadius;
                var layerMask = groundLayerMask;

                isGrounded = 
                    Physics2D.OverlapCircle(position, radius, layerMask);
            }

            return isGrounded;
        }
    }
}