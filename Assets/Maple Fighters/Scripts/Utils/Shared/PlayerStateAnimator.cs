using CommonCommunicationInterfaces;
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
            // When a new game objects added, so send them the last current state.
            ServiceContainer.GameService.SceneObjectsAdded.AddListener((x) => 
            {
                SendPlayerStateChangedOperation(lastPlayerState);
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

            animator.SetState(playerState);

            SendPlayerStateChangedOperation(playerState);

            lastPlayerState = playerState;
        }

        private void SendPlayerStateChangedOperation(PlayerState playerState)
        {
            var parameters = new UpdatePlayerStateRequestParameters(playerState);
            var messageSendOptions = MessageSendOptions.DefaultUnreliable((byte)GameDataChannels.Animations);
            ServiceContainer.GameService.SendOperation((byte)GameOperations.PlayerStateChanged, parameters, messageSendOptions);
        }
    }
}