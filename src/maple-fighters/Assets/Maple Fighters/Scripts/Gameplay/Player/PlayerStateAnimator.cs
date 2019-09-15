using Game.Common;
using Scripts.Containers;
using UnityEngine;

namespace Scripts.Gameplay.Player
{
    [RequireComponent(typeof(Animator))]
    public class PlayerStateAnimator : MonoBehaviour, IPlayerStateAnimator
    {
        [Header("Animations")]
        [SerializeField]
        private string walkName = "Walking";

        [SerializeField]
        private string jumpName = "Jump";

        [SerializeField]
        private string ropeName = "Rope";

        [SerializeField]
        private string ladderName = "Ladder";

        private Animator animator;
        private PlayerState playerState = PlayerState.Idle;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        private void Start()
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

        private void OnDestroy()
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

                UpdatePlayerAnimationState(animator, playerState);
                SendUpdatePlayerStateOperation();
            }
        }

        private void OnPlayerStateChanged(PlayerStateChangedEventParameters parameters)
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
        /// When a new game objects added, will send them the last current state
        /// </summary>
        private void OnSceneObjectsAdded(SceneObjectsAddedEventParameters parameters)
        {
            SendUpdatePlayerStateOperation();
        }

        private void SendUpdatePlayerStateOperation()
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
            animator.SetBool(walkName, playerState == PlayerState.Moving);
            animator.SetBool(jumpName, playerState == PlayerState.Jumping || playerState == PlayerState.Falling);
            animator.SetBool(ropeName, playerState == PlayerState.Rope);
            animator.SetBool(ladderName, playerState == PlayerState.Ladder);
        }
    }
}