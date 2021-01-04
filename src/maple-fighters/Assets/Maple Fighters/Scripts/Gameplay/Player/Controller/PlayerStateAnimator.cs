using Game.Messages;
using Scripts.Constants;
using Scripts.Gameplay.Entity;
using Scripts.Services;
using Scripts.Services.GameApi;
using UnityEngine;

namespace Scripts.Gameplay.Player
{
    [RequireComponent(typeof(Animator))]
    public class PlayerStateAnimator : MonoBehaviour, IPlayerStateAnimator
    {
        public bool Enabled
        {
            get => animator.enabled;
            set => animator.enabled = value;
        }

        private IGameApi gameApi;

        private PlayerStates playerState = PlayerStates.Idle;
        private Animator animator;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        private void Start()
        {
            gameApi = ApiProvider.ProvideGameApi();
            gameApi.GameObjectsAdded += OnGameObjectsAdded;
            gameApi.AnimationStateChanged += OnPlayerStateChanged;
        }

        private void OnDisable()
        {
            gameApi.GameObjectsAdded -= OnGameObjectsAdded;
            gameApi.AnimationStateChanged -= OnPlayerStateChanged;
        }

        public void SetPlayerState(PlayerStates playerState)
        {
            // TODO: Hack
            if (playerState == PlayerStates.Attacked)
            {
                playerState = PlayerStates.Falling;
            }

            this.playerState = playerState;

            SetPlayerAnimationState(animator, playerState);
            SendUpdatePlayerStateMessage();
        }

        private void OnGameObjectsAdded(GameObjectsAddedMessage _)
        {
            // When a new game objects added, will send them the last current state
            SendUpdatePlayerStateMessage();
        }

        private void OnPlayerStateChanged(AnimationStateChangedMessage message)
        {
            var id = message.GameObjectId;

            if (EntityContainer.GetInstance().GetRemoteEntity(id, out var entity))
            {
                var playerAnimatorProvider =
                    entity?.GameObject.GetComponent<PlayerAnimatorProvider>();
                var animator = playerAnimatorProvider?.Provide();
                if (animator != null)
                {
                    var playerState = (PlayerStates)message.AnimationState;

                    SetPlayerAnimationState(animator, playerState);
                }
            }
        }

        private void SendUpdatePlayerStateMessage()
        {
            // TODO: Send animation enabled
            var message = new ChangeAnimationStateMessage()
            {
                AnimationState = (byte)playerState
            };

            gameApi?.SendMessage(MessageCodes.ChangeAnimationState, message);
        }

        private void SetPlayerAnimationState(Animator animator, PlayerStates playerState)
        {
            var isMoving = playerState == PlayerStates.Moving;
            var isJumping = playerState == PlayerStates.Jumping;
            var isFalling = playerState == PlayerStates.Falling;
            var isRope = playerState == PlayerStates.Rope;
            var isLadder = playerState == PlayerStates.Ladder;

            animator.SetBool(AnimationNames.Player.Walk, isMoving);
            animator.SetBool(AnimationNames.Player.Jump, isJumping || isFalling);
            animator.SetBool(AnimationNames.Player.Rope, isRope);
            animator.SetBool(AnimationNames.Player.Ladder, isLadder);
        }
    }
}