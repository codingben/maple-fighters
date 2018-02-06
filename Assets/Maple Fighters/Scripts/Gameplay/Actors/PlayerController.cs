using System;
using System.Linq;
using Scripts.World;
using Shared.Game.Common;
using UnityEngine;

#pragma warning disable 0109

namespace Scripts.Gameplay.Actors
{
    public class PlayerController : StateBehaviors, IPlayerController
    {
        public event Action<PlayerState> PlayerStateChanged;
        public event Action<Directions> DirectionChanged;

        public Rigidbody2D Rigidbody
        {
            get;
            private set;
        }

        public Directions Direction
        {
            set
            {
                const float SCALE = 1;

                transform.localScale = value == Directions.Left ? new Vector3(SCALE, transform.localScale.y, transform.localScale.z) 
                    : new Vector3(-SCALE, transform.localScale.y, transform.localScale.z);

                DirectionChanged?.Invoke(value);
            }
        }

        public PlayerState PlayerState
        {
            set
            {
                playerState = value;

                if (playerState != lastPlayerState)
                {
                    GetStateBehaviour(lastPlayerState)?.OnStateExit();
                    GetStateBehaviour(playerState)?.OnStateEnter(this);
                }

                lastPlayerState = playerState;

                PlayerStateChanged?.Invoke(playerState != PlayerState.Attacked ? playerState : PlayerState.Falling);
            }
            get
            {
                return playerState;
            }
        }

        [Header("State")]
        [SerializeField] private PlayerState playerState = PlayerState.Falling;
        private PlayerState lastPlayerState;

        [Header("Ground Detection")]
        [SerializeField] private LayerMask groundLayerMask;
        [SerializeField] private Transform[] groundDetectionPoints;

        private void Awake()
        {
            Rigidbody = GetComponent<Collider2D>().attachedRigidbody;

            CreatePlayerStates();
        }

        private void Start()
        {
            lastPlayerState = playerState;

            GetStateBehaviour(playerState)?.OnStateEnter(this);
        }

        private void Update()
        {
            GetStateBehaviour(playerState)?.OnStateUpdate();
        }

        private void FixedUpdate()
        {
            GetStateBehaviour(playerState)?.OnStateFixedUpdate();
        }

        public bool IsOnGround()
        {
            return groundDetectionPoints.Any(ground => Physics2D.OverlapPoint(ground.position, groundLayerMask));
        }
    }
}