using CommonTools.Log;
using Scripts.Containers;
using Scripts.Gameplay.Actors;
using Shared.Game.Common;
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
            LogUtils.Log(MessageBuilder.Trace());

            ServiceContainer.GameService.SceneObjectsAdded.AddListener((x) => 
            {
                // When a new game objects added, so send them the last current state.
                var parameters = new UpdatePlayerStateRequestParameters(lastPlayerState);
                ServiceContainer.GameService.UpdatePlayerState(parameters);
            });

            ServiceContainer.GameService.PlayerStateChanged.AddListener((x) => 
            {
                var sceneObject = SceneObjectsContainer.Instance.GetRemoteSceneObject(x.SceneObjectId)?.GetGameObject();
                if (sceneObject != null)
                {
                    sceneObject.GetComponent<PlayerStateSetter>()?.SetState(x.PlayerState);
                }
            });
        }

        public void OnPlayerStateChanged(PlayerState playerState)
        {
            if (lastPlayerState == playerState)
            {
                return;
            }

            var parameters = new UpdatePlayerStateRequestParameters(playerState);
            ServiceContainer.GameService.UpdatePlayerState(parameters);

            animator.SetState(playerState);

            lastPlayerState = playerState;
        }
    }
}