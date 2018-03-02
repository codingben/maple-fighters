using Game.Common;

namespace Scripts.Gameplay.Actors
{
    public struct PlayerStateDetails
    {
        public PlayerState PlayerState { get; }
        public IPlayerStateBehaviour PlayerStateBehaviour { get; }

        public PlayerStateDetails(PlayerState playerState, IPlayerStateBehaviour playerStateBehaviour)
        {
            PlayerState = playerState;
            PlayerStateBehaviour = playerStateBehaviour;
        }
    }
}