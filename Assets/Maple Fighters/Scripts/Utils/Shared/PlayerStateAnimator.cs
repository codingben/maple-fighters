using CommonTools.Log;
using Scripts.Containers;
using Scripts.Gameplay.Actors;
using Game.Common;
using Scripts.Services;
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
            var gameScenePeerLogic = ServiceContainer.GameService.GetPeerLogic<IGameScenePeerLogicAPI>().AssertNotNull();
            gameScenePeerLogic.SceneObjectsAdded.AddListener(OnSceneObjectsAdded);
            gameScenePeerLogic.PlayerStateChanged.AddListener(OnPlayerStateChanged);
        }

        private void UnsubscribeFromGameServiceEvents()
        {
            var gameScenePeerLogic = ServiceContainer.GameService.GetPeerLogic<IGameScenePeerLogicAPI>().AssertNotNull();
            gameScenePeerLogic.SceneObjectsAdded.RemoveListener(OnSceneObjectsAdded);
            gameScenePeerLogic.PlayerStateChanged.RemoveListener(OnPlayerStateChanged);
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

        /// <summary>
        /// When a new game objects added, so send them the last current state.
        /// </summary>
        private void OnSceneObjectsAdded(SceneObjectsAddedEventParameters parameters)
        {
            UpdatePlayerStateOperation();
        }

        private void UpdatePlayerStateOperation()
        {
            var gameScenePeerLogic = ServiceContainer.GameService.GetPeerLogic<IGameScenePeerLogicAPI>().AssertNotNull();
            gameScenePeerLogic.UpdatePlayerState(new UpdatePlayerStateRequestParameters(lastPlayerState));
        }
    }
}