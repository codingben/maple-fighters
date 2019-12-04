using Game.Common;
using Scripts.Constants;
using Scripts.Gameplay.Player;
using UnityEngine;

namespace Scripts.Gameplay.Map.Climb
{
    [RequireComponent(typeof(PlayerController), typeof(Collider2D))]
    public class LadderInteractor : ClimbInteractor
    {
        [SerializeField]
        private KeyCode key = KeyCode.LeftControl;

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
            playerController.ChangePlayerState(GetClimbState());
        }

        protected override void UnsetPlayerFromClimbState()
        {
            playerController.ResetPlayerState();
        }

        protected override PlayerState GetPlayerState()
        {
            return playerController.PlayerState;
        }

        protected override KeyCode GetKey()
        {
            return key;
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