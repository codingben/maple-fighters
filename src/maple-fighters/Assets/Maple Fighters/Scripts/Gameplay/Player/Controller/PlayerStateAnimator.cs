using Game.Common;
using Scripts.Constants;
using Scripts.Gameplay.GameEntity;
using Scripts.Services.Game;
using UnityEngine;

namespace Scripts.Gameplay.Player
{
    [RequireComponent(typeof(Animator))]
    public class PlayerStateAnimator : MonoBehaviour, IPlayerStateAnimator
    {
        private PlayerState playerState = PlayerState.Idle;
        private Animator animator;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        private void Start()
        {
            var gameService = GameService.GetInstance();
            gameService?.GameSceneApi.SceneObjectsAdded.AddListener(OnSceneObjectsAdded);
            gameService?.GameSceneApi.PlayerStateChanged.AddListener(OnPlayerStateChanged);
        }

        private void OnDestroy()
        {
            var gameService = GameService.GetInstance();
            gameService?.GameSceneApi.SceneObjectsAdded.RemoveListener(OnSceneObjectsAdded);
            gameService?.GameSceneApi.PlayerStateChanged.RemoveListener(OnPlayerStateChanged);
        }

        public void ChangePlayerState(PlayerState newPlayerState)
        {
            if (playerState != newPlayerState)
            {
                // TODO: Hack
                if (newPlayerState == PlayerState.Attacked)
                {
                    newPlayerState = PlayerState.Falling;
                }

                playerState = newPlayerState;

                SetPlayerAnimationState(animator, playerState);

                SendUpdatePlayerStateOperation();
            }
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
                SetPlayerAnimationState(animator, playerState);
            }
        }

        private void SendUpdatePlayerStateOperation()
        {
            var gameService = GameService.GetInstance();
            gameService?.GameSceneApi.UpdatePlayerState(
                new UpdatePlayerStateRequestParameters(playerState));
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