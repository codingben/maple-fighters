using Game.Common;

namespace Scripts.Gameplay.Actors
{
    public class PlayerFallingState : IPlayerStateBehaviour
    {
        private readonly PlayerController playerController;

        public PlayerFallingState(PlayerController playerController)
        {
            this.playerController = playerController;
        }

        public void OnStateEnter()
        {
            // Left blank intentionally
        }

        public void OnStateUpdate()
        {
            if (IsGrounded())
            {
                playerController.ChangePlayerState(PlayerState.Idle);
            }
        }

        public void OnStateFixedUpdate()
        {
            // Left blank intentionally
        }

        public void OnStateExit()
        {
            // Left blank intentionally
        }

        private bool IsGrounded()
        {
            return playerController.IsGrounded();
        }
    }
}