using System.Collections.Generic;
using Game.Common;
using Scripts.Editor;
using Scripts.UI.Controllers;
using Scripts.Utils.Shared;
using UnityEngine;

#pragma warning disable 0109

namespace Scripts.Gameplay.Actors
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

        [Header("Floor")]
        [SerializeField]
        private float overlapCircleRadius;

        [SerializeField]
        private LayerMask groundLayerMask;

        [SerializeField]
        private Transform groundTransform;

        private Vector2 localScale;

        private Dictionary<PlayerState, IPlayerStateBehaviour>
            playerStateBehaviours;

        private IPlayerStateAnimator playerStateAnimator;
        private IPlayerStateBehaviour playerStateBehaviour;

        private new Rigidbody2D rigidbody2D;

        private void Awake()
        {
            localScale = transform.localScale;

            var focusStateController = 
                FindObjectOfType<FocusStateController>();
            if (focusStateController == null)
            {
                Debug.LogError("Could not find FocusStateController!");
                Debug.Break();
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
        }

        private void Start()
        {
            playerStateBehaviour?.OnStateEnter();
        }

        private void Update()
        {
            playerStateBehaviour?.OnStateUpdate();
        }

        private void FixedUpdate()
        {
            playerStateBehaviour?.OnStateFixedUpdate();
        }

        public void SetPlayerStateAnimator(IPlayerStateAnimator playerStateAnimator)
        {
            this.playerStateAnimator = playerStateAnimator;
        }

        public void ChangePlayerState(PlayerState newPlayerState)
        {
            if (playerState != newPlayerState)
            {
                playerStateBehaviour?.OnStateExit();

                if (playerStateBehaviour != null)
                {
                    playerStateBehaviour = 
                        playerStateBehaviours[newPlayerState];
                }

                playerStateBehaviour?.OnStateEnter();

                playerState = newPlayerState;

                playerStateAnimator?.ChangePlayerState(playerState);
            }
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

        public bool IsGrounded()
        {
            return Physics2D.OverlapCircle(
                point: groundTransform.position, 
                radius: overlapCircleRadius, 
                layerMask: groundLayerMask);
        }

        public float GetAxis(Axes axis, bool isRaw = false)
        {
            var result = default(float);

            const string VerticalName = "Vertical";
            const string HorizontalName = "Horizontal";

            switch (axis)
            {
                case Axes.Vertical:
                {
                    result =
                        isRaw
                            ? Input.GetAxisRaw(VerticalName)
                            : Input.GetAxis(VerticalName);

                    break;
                }

                case Axes.Horizontal:
                {
                    result = 
                        isRaw
                            ? Input.GetAxisRaw(HorizontalName)
                            : Input.GetAxis(HorizontalName);

                    break;
                }
            }

            return result;
        }
    }
}