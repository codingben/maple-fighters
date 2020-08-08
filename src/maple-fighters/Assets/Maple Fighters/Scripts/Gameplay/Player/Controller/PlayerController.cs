using System.Collections.Generic;
using ScriptableObjects.Configurations;
using Scripts.Editor;
using Scripts.Gameplay.Player.States;
using Scripts.UI.Focus;
using UnityEngine;

#pragma warning disable 0109

namespace Scripts.Gameplay.Player
{
    [RequireComponent(typeof(Collider2D))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Debug")]
        [ViewOnly, SerializeField]
        private PlayerStates playerState = PlayerStates.Falling;

        [Header("Ground")]
        [SerializeField]
        private float overlapCircleRadius;

        [SerializeField]
        private LayerMask groundLayerMask;

        [SerializeField]
        private Transform groundTransform;

        private Dictionary<PlayerStates, IPlayerStateBehaviour> playerStateBehaviours;

        private IPlayerStateBehaviour playerStateBehaviour;
        private IPlayerStateAnimator playerStateAnimator;

        private new Transform transform2D;
        private new Rigidbody2D rigidbody2D;

        private FocusStateController focusStateController;

        private void Awake()
        {
            playerStateBehaviours = new Dictionary<PlayerStates, IPlayerStateBehaviour>
            {
                { PlayerStates.Idle, new PlayerIdleState(this) },
                { PlayerStates.Moving, new PlayerMovingState(this) },
                { PlayerStates.Jumping, new PlayerJumpingState(this) },
                { PlayerStates.Falling, new PlayerFallingState(this) },
                { PlayerStates.Attacked, new PlayerAttackedState(this) },
                { PlayerStates.Rope, new PlayerRopeState(this) },
                { PlayerStates.Ladder, new PlayerLadderState(this) }
            };
            playerStateBehaviour = playerStateBehaviours[playerState];
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

        public PlayerProperties GetProperties()
        {
            return PlayerConfiguration.GetInstance().PlayerProperties;
        }

        public PlayerKeyboard GetKeyboardSettings()
        {
            return PlayerConfiguration.GetInstance().PlayerKeyboard;
        }

        public void SetPlayerStateAnimator(IPlayerStateAnimator playerStateAnimator)
        {
            this.playerStateAnimator = playerStateAnimator;
        }

        public IPlayerStateAnimator GetPlayerStateAnimator()
        {
            return playerStateAnimator;
        }

        public void SetPlayerState(PlayerStates newPlayerState)
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

        public PlayerStates GetPlayerState()
        {
            return playerState;
        }

        public void ChangeDirection(Directions direction)
        {
            if (transform2D == null)
            {
                transform2D = transform;
            }

            var x = Mathf.Abs(transform2D.localScale.x);
            var y = transform2D.localScale.y;
            var z = transform2D.localScale.z;

            if (direction == Directions.Left)
            {
                transform2D.localScale = new Vector3(x, y, z);
            }
            else if (direction == Directions.Right)
            {
                transform2D.localScale = new Vector3(-x, y, z);
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

            if (focusStateController?.GetFocusState() == UIFocusState.Game)
            {
                var horizontal = Utils.GetAxis(Axes.Horizontal, isRaw: true);
                isMoving = Mathf.Abs(horizontal) > 0;
            }

            return isMoving;
        }

        public bool CanJump()
        {
            return focusStateController?.GetFocusState() == UIFocusState.Game;
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