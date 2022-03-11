using Game.Messages;
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
            gameApi.GameObjectsAdded.AddListener(OnGameObjectsAdded);
            gameApi.AnimationStateChanged.AddListener(OnPlayerStateChanged);
        }

        private void OnDisable()
        {
            gameApi?.GameObjectsAdded?.RemoveListener(OnGameObjectsAdded);
            gameApi?.AnimationStateChanged?.RemoveListener(OnPlayerStateChanged);
        }

        public void SetPlayerState(PlayerStates playerState)
        {
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
            // TODO: Send animation enabled (while in rope/ladder)
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
            var isPrimaryAttack = playerState == PlayerStates.PrimaryAttack;
            var isSecondaryAttack = playerState == PlayerStates.SecondaryAttack;

            animator.SetBool(PlayerStates.Moving.ToString(), isMoving);
            animator.SetBool(PlayerStates.Jumping.ToString(), isJumping || isFalling);
            animator.SetBool(PlayerStates.Rope.ToString(), isRope);
            animator.SetBool(PlayerStates.Ladder.ToString(), isLadder);
            animator.SetBool(PlayerStates.PrimaryAttack.ToString(), isPrimaryAttack);
            animator.SetBool(PlayerStates.SecondaryAttack.ToString(), isSecondaryAttack);
        }
    }
}