using CommonTools.Log;
using Game.Common;
using Scripts.Containers;
using Scripts.Gameplay.Actors;
using Scripts.Services;
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
            var gameScenePeerLogic = 
                ServiceContainer.GameService
                    .GetPeerLogic<IGameScenePeerLogicAPI>().AssertNotNull();
            gameScenePeerLogic.SceneObjectsAdded.AddListener(OnSceneObjectsAdded);
            gameScenePeerLogic.PlayerStateChanged.AddListener(OnPlayerStateChanged);
        }

        private void UnsubscribeFromGameServiceEvents()
        {
            var gameScenePeerLogic = 
                ServiceContainer.GameService
                    .GetPeerLogic<IGameScenePeerLogicAPI>().AssertNotNull();
            gameScenePeerLogic.SceneObjectsAdded.RemoveListener(OnSceneObjectsAdded);
            gameScenePeerLogic.PlayerStateChanged.RemoveListener(OnPlayerStateChanged);
        }

        public void OnPlayerStateChanged(PlayerState newPlayerState)
        {
            if (playerState == newPlayerState)
            {
                return;
            }

            animator.ChangePlayerAnimationState(newPlayerState);

            playerState = newPlayerState;

            UpdatePlayerStateOperation();
        }

        private void OnPlayerStateChanged(PlayerStateChangedEventParameters parameters)
        {
            var sceneObject = 
                SceneObjectsContainer.GetInstance()
                    .GetRemoteSceneObject(parameters.SceneObjectId)?.GameObject;
            if (sceneObject != null)
            {
                var playerStateSetter = sceneObject
                    .GetComponent<PlayerStateSetter>().AssertNotNull();
                playerStateSetter.SetState(parameters.PlayerState);
            }
        }

        /// <summary>
        /// When a new game objects added, so send them the last current state.
        /// </summary>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        private void OnSceneObjectsAdded(SceneObjectsAddedEventParameters parameters)
        {
            UpdatePlayerStateOperation();
        }

        private void UpdatePlayerStateOperation()
        {
            var gameScenePeerLogic = 
                ServiceContainer.GameService
                    .GetPeerLogic<IGameScenePeerLogicAPI>().AssertNotNull();
            gameScenePeerLogic.UpdatePlayerState(
                new UpdatePlayerStateRequestParameters(playerState));
        }
    }
}