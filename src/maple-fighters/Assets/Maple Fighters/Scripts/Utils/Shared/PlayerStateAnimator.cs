using Game.Common;
using Scripts.Containers;
using Scripts.Gameplay.Actors;
using UnityEngine;

namespace Scripts.Utils.Shared
{
    [RequireComponent(typeof(Animator))]
    public class PlayerStateAnimator : MonoBehaviour
    {
        private Animator animator;
        private PlayerState playerState = PlayerState.Idle;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        private void Start()
        {
            SubscribeToGameServiceEvents();
        }

        private void OnDestroy()
        {
            UnsubscribeFromGameServiceEvents();
        }

        private void SubscribeToGameServiceEvents()
        {
            var gameSceneApi = ServiceContainer.GameService.GetGameSceneApi();
            if (gameSceneApi != null)
            {
                gameSceneApi.SceneObjectsAdded.AddListener(
                    OnSceneObjectsAdded);
                gameSceneApi.PlayerStateChanged.AddListener(
                    OnPlayerStateChanged);
            }
        }

        private void UnsubscribeFromGameServiceEvents()
        {
            var gameSceneApi = ServiceContainer.GameService.GetGameSceneApi();
            if (gameSceneApi != null)
            {
                gameSceneApi.SceneObjectsAdded.RemoveListener(
                    OnSceneObjectsAdded);
                gameSceneApi.PlayerStateChanged.RemoveListener(
                    OnPlayerStateChanged);
            }
        }

        public void OnPlayerStateChanged(PlayerState newPlayerState)
        {
            if (playerState != newPlayerState)
            {
                // TODO: Hack
                if (newPlayerState == PlayerState.Attacked)
                {
                    newPlayerState = PlayerState.Falling;
                }

                playerState = newPlayerState;

                UpdatePlayerAnimationState(animator, playerState);
                UpdatePlayerStateOperation();
            }
        }

        private void OnPlayerStateChanged(
            PlayerStateChangedEventParameters parameters)
        {
            var sceneObject = 
                SceneObjectsContainer.GetInstance()
                    .GetRemoteSceneObject(parameters.SceneObjectId)?.GameObject;
            if (sceneObject != null)
            {
                var playerAnimatorProvider = 
                    sceneObject.GetComponent<PlayerAnimatorProvider>();
                if (playerAnimatorProvider != null)
                {
                    var animator = playerAnimatorProvider.Provide();
                    if (animator != null)
                    {
                        UpdatePlayerAnimationState(animator, playerState);
                    }
                }
            }
        }

        /// <summary>
        /// When a new game objects added, so send them the last current state.
        /// </summary>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        private void OnSceneObjectsAdded(
            SceneObjectsAddedEventParameters parameters)
        {
            UpdatePlayerStateOperation();
        }

        private void UpdatePlayerStateOperation()
        {
            var gameSceneApi = ServiceContainer.GameService.GetGameSceneApi();
            if (gameSceneApi != null)
            {
                gameSceneApi.UpdatePlayerState(
                    new UpdatePlayerStateRequestParameters(playerState));
            }
        }

        private void UpdatePlayerAnimationState(
            Animator animator,
            PlayerState playerState)
        {
            animator.SetBool("WalkName", playerState == PlayerState.Moving);
            animator.SetBool("JumpName", playerState == PlayerState.Jumping || playerState == PlayerState.Falling);
            animator.SetBool("RopeName", playerState == PlayerState.Rope);
            animator.SetBool("LadderName", playerState == PlayerState.Ladder);
        }
    }
}