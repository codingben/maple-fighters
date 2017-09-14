using System;
using Scripts.Containers;
using Scripts.Containers.Service;
using Scripts.Gameplay.Actors;
using Shared.Game.Common;
using UnityEngine;

namespace Scripts.Utils.Shared
{
    [RequireComponent(typeof(Animator))]
    public class PlayerStateNetworkAnimator : MonoBehaviour
    {
        [SerializeField] private bool IsLocal;

        [Header("Animator Parameters")]
        [SerializeField] private string walkName;
        [SerializeField] private string jumpName;

        private Animator animator;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        private void Start()
        {
            if (IsLocal)
            {
                ServiceContainer.GameService.PlayerStateChanged.AddListener(OnPlayerStateEventReceived);
            }
        }

        private void OnPlayerStateEventReceived(PlayerStateChangedEventParameters parameters)
        {
            var entityId = parameters.EntityId;
            var entity = GameContainers.EntityContainer.GetRemoteEntity(entityId);

            entity?.GameObject.GetComponent<PlayerStateSetter>().SetState(parameters.PlayerState);
        }

        public void OnPlayerStateChanged(PlayerState playerState)
        {
            var parameters = new UpdatePlayerStateRequestParameters(playerState);
            ServiceContainer.GameService.UpdatePlayerState(parameters);

            OnPlayerStateReceived(playerState);
        }

        public void OnPlayerStateReceived(PlayerState playerState)
        {
            switch (playerState)
            {
                case PlayerState.Idle:
                {
                    IdleState();
                    break;
                }
                case PlayerState.Moving:
                {
                    MovingState();
                    break;
                }
                case PlayerState.Falling:
                {
                    FallingState();
                    break;
                }
                default:
                {
                    throw new ArgumentOutOfRangeException(nameof(playerState), playerState, null);
                }
            }
        }

        private void IdleState()
        {
            animator.SetBool(walkName, false);
            animator.SetBool(jumpName, false);
        }

        private void MovingState()
        {
            animator.SetBool(walkName, true);
            animator.SetBool(jumpName, false);
        }

        private void FallingState()
        {
            animator.SetBool(jumpName, true);
            animator.SetBool(walkName, false);
        }
    }
}