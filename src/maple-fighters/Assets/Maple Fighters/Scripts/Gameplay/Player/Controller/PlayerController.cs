using System.Collections.Generic;
using ScriptableObjects.Configurations;
using Scripts.Editor;
using Scripts.Gameplay.Graphics;
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
        [ViewOnly, SerializeField]
        private bool canMove = true;

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
                { PlayerStates.Rope, new PlayerClimbState(this) },
                { PlayerStates.Ladder, new PlayerClimbState(this) }
            };
            playerStateBehaviour = playerStateBehaviours[playerState];
            focusStateController = FindObjectOfType<FocusStateController>();
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

        public void ChangeDirection(Direction direction)
        {
            if (transform2D == null)
            {
                transform2D = transform;
            }

            var x = Mathf.Abs(transform2D.localScale.x);
            var y = transform2D.localScale.y;
            var z = transform2D.localScale.z;

            if (direction == Direction.Left)
            {
                transform2D.localScale = new Vector3(x, y, z);
            }
            else if (direction == Direction.Right)
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

        public void CreateRushEffect(float direction)
        {
            var rushEffectPosition = transform.position;
            var rushEffectDirection = new Vector2(direction, 1);

            RushEffect.Create(rushEffectPosition, rushEffectDirection);
        }

        public void SetCanMove(bool canMove)
        {
            this.canMove = canMove;
        }

        public bool CanMove()
        {
            if (focusStateController?.GetFocusState() != UIFocusState.Game)
            {
                return false;
            }

            return canMove;
        }

        public bool IsMoving()
        {
            var horizontal = Utils.GetAxis(Axes.Horizontal, isRaw: true);

            return Mathf.Abs(horizontal) > 0;
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