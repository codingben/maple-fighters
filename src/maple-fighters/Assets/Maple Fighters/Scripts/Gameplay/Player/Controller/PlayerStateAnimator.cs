using Game.Common;
using Scripts.Constants;
using Scripts.Gameplay.Entity;
using Scripts.Services.Game;
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

        private PlayerState playerState = PlayerState.Idle;
        private Animator animator;

        private GameService gameService;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        private void Start()
        {
            gameService = FindObjectOfType<GameService>();
            gameService?.GameSceneApi?.SceneObjectsAdded.AddListener(OnSceneObjectsAdded);
            gameService?.GameSceneApi?.PlayerStateChanged.AddListener(OnPlayerStateChanged);
        }

        private void OnDisable()
        {
            gameService?.GameSceneApi?.SceneObjectsAdded.RemoveListener(OnSceneObjectsAdded);
            gameService?.GameSceneApi?.PlayerStateChanged.RemoveListener(OnPlayerStateChanged);
        }

        public void SetPlayerState(PlayerState playerState)
        {
            // TODO: Hack
            if (playerState == PlayerState.Attacked)
            {
                playerState = PlayerState.Falling;
            }

            this.playerState = playerState;

            SetPlayerAnimationState(animator, playerState);
            SendUpdatePlayerStateOperation();
        }

        private void OnSceneObjectsAdded(SceneObjectsAddedEventParameters parameters)
        {
            // When a new game objects added, will send them the last current state
            SendUpdatePlayerStateOperation();
        }

        private void OnPlayerStateChanged(PlayerStateChangedEventParameters parameters)
        {
            var entity = 
                EntityContainer.GetInstance()
                    .GetRemoteEntity(parameters.SceneObjectId)?.GameObject;
            var playerAnimatorProvider =
                entity?.GetComponent<PlayerAnimatorProvider>();
            var animator = playerAnimatorProvider?.Provide();
            if (animator != null)
            {
                var playerState = parameters.PlayerState;
                SetPlayerAnimationState(animator, playerState);
            }
        }

        private void SendUpdatePlayerStateOperation()
        {
            // TODO: Send animation enabled
            var parameters =
                new UpdatePlayerStateRequestParameters(playerState);

            gameService?.GameSceneApi?.UpdatePlayerState(parameters);
        }

        private void SetPlayerAnimationState(Animator animator, PlayerState playerState)
        {
            var isMoving = playerState == PlayerState.Moving;
            var isJumping = playerState == PlayerState.Jumping;
            var isFalling = playerState == PlayerState.Falling;
            var isRope = playerState == PlayerState.Rope;
            var isLadder = playerState == PlayerState.Ladder;

            animator.SetBool(AnimationNames.Player.Walk, isMoving);
            animator.SetBool(AnimationNames.Player.Jump, isJumping || isFalling);
            animator.SetBool(AnimationNames.Player.Rope, isRope);
            animator.SetBool(AnimationNames.Player.Ladder, isLadder);
        }
    }
}