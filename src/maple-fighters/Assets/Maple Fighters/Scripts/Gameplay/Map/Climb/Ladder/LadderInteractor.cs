using Game.Common;
using Scripts.Constants;
using Scripts.Gameplay.Player;
using UnityEngine;

namespace Scripts.Gameplay.Map.Climb
{
    [RequireComponent(typeof(PlayerController), typeof(Collider2D))]
    public class LadderInteractor : ClimbInteractor
    {
        private PlayerController playerController;
        private ColliderInteraction colliderInteraction;

        private void Awake()
        {
            playerController = GetComponent<PlayerController>();

            var collider = GetComponent<Collider2D>();
            colliderInteraction = new ColliderInteraction(collider);
        }

        protected override void SetPlayerToClimbState()
        {
            playerController.SetPlayerState(GetClimbState());
        }

        protected override void UnsetPlayerFromClimbState()
        {
            var playerState = 
                playerController.IsGrounded()
                    ? PlayerState.Idle
                    : PlayerState.Falling;

            playerController.SetPlayerState(playerState);
        }

        protected override PlayerState GetPlayerState()
        {
            return playerController.GetPlayerState();
        }

        protected override KeyCode GetKey()
        {
            return playerController.GetKeyboardSettings().ClimbKey;
        }

        protected override ColliderInteraction GetColliderInteraction()
        {
            return colliderInteraction;
        }

        protected override string GetTagName()
        {
            return GameTags.LadderTag;
        }

        protected override PlayerState GetClimbState()
        {
            return PlayerState.Ladder;
        }
    }
}