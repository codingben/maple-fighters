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
        [SerializeField] private bool isLocal;

        [Header("Animator Parameters")]
        [SerializeField] private string walkName;
        [SerializeField] private string jumpName;
        [SerializeField] private string ropeName;
        [SerializeField] private string ladderName;

        private Animator animator;
        private PlayerState lastPlayerState = PlayerState.Idle;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        private void Start()
        {
            if (!isLocal)
            {
                return;
            }

            GameContainers.EntityContainer.EntityAdded += OnEntityAdded;
             
            ServiceContainer.GameService.PlayerStateChanged.AddListener(OnPlayerStateEventReceived);
        }

        /// <summary>
        /// When a new entity added, so send him the last current state.
        /// </summary>
        private void OnEntityAdded()
        {
            var parameters = new UpdatePlayerStateRequestParameters(lastPlayerState);
            ServiceContainer.GameService.UpdatePlayerState(parameters);
        }

        private void OnPlayerStateEventReceived(PlayerStateChangedEventParameters parameters)
        {
            var entityId = parameters.EntityId;
            var entity = GameContainers.EntityContainer.GetRemoteEntity(entityId);

            entity?.GameObject.GetComponent<PlayerStateSetter>().SetState(parameters.PlayerState);
        }

        public void OnPlayerStateChanged(PlayerState playerState)
        {
            if (lastPlayerState == playerState)
            {
                return;
            }

            var parameters = new UpdatePlayerStateRequestParameters(playerState);
            ServiceContainer.GameService.UpdatePlayerState(parameters);

            OnPlayerStateReceived(playerState);

            lastPlayerState = playerState;
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
                case PlayerState.Rope:
                {
                    RopeState();
                    break;
                }
                case PlayerState.Ladder:
                {
                    LadderState();
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
            animator.SetBool(ropeName, false);
            animator.SetBool(ladderName, false);
        }

        private void MovingState()
        {
            animator.SetBool(walkName, true);
            animator.SetBool(jumpName, false);
            animator.SetBool(ropeName, false);
            animator.SetBool(ladderName, false);
        }

        private void FallingState()
        {
            animator.SetBool(jumpName, true);
            animator.SetBool(walkName, false);
            animator.SetBool(ropeName, false);
            animator.SetBool(ladderName, false);
        }

        private void RopeState()
        {
            animator.SetBool(jumpName, false);
            animator.SetBool(walkName, false);
            animator.SetBool(ropeName, true);
            animator.SetBool(ladderName, false);
        }

        private void LadderState()
        {
            animator.SetBool(jumpName, false);
            animator.SetBool(walkName, false);
            animator.SetBool(ropeName, false);
            animator.SetBool(ladderName, true);
        }
    }
}