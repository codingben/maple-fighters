using Game.Common;

namespace Scripts.Gameplay.Actors
{
    public class PlayerFallingState : IPlayerStateBehaviour
    {
        private IPlayerController playerController;

        public void OnStateEnter(IPlayerController playerController)
        {
            if (this.playerController == null)
            {
                this.playerController = playerController;
            }
        }

        public void OnStateUpdate()
        {
            if (playerController.IsOnGround())
            {
                playerController.PlayerState = PlayerState.Idle;
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
    }
}