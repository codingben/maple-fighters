using Scripts.Containers;
using Scripts.Gameplay.Actors;
using Game.Common;
using UnityEngine;

namespace Scripts.Utils.Shared
{
    [RequireComponent(typeof(Animator))]
    public class PlayerStateAnimator : MonoBehaviour
    {
        private Animator animator;
        private PlayerState lastPlayerState = PlayerState.Idle;

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
            ServiceContainer.GameService.SceneObjectsAdded.AddListener(OnSceneObjectsAdded); // When a new game objects added, so send them the last current state.
            ServiceContainer.GameService.PlayerStateChanged.AddListener(OnPlayerStateChanged);
        }

        private void UnsubscribeFromGameServiceEvents()
        {
            ServiceContainer.GameService.SceneObjectsAdded.RemoveListener(OnSceneObjectsAdded);
            ServiceContainer.GameService.PlayerStateChanged.RemoveListener(OnPlayerStateChanged);
        }

        public void OnPlayerStateChanged(PlayerState playerState)
        {
            if (lastPlayerState == playerState)
            {
                return;
            }

            animator.SetState(playerState);
            lastPlayerState = playerState;

            UpdatePlayerStateOperation();
        }

        private void OnPlayerStateChanged(PlayerStateChangedEventParameters parameters)
        {
            var sceneObject = SceneObjectsContainer.Instance.GetRemoteSceneObject(parameters.SceneObjectId)?.GetGameObject();
            if (sceneObject != null)
            {
                sceneObject.GetComponent<PlayerStateSetter>()?.SetState(parameters.PlayerState);
            }
        }

        private void OnSceneObjectsAdded(SceneObjectsAddedEventParameters parameters)
        {
            UpdatePlayerStateOperation();
        }

        private void UpdatePlayerStateOperation()
        {
            var parameters = new UpdatePlayerStateRequestParameters(lastPlayerState);
            ServiceContainer.GameService.UpdatePlayerState(parameters);
        }
    }
}