using Game.Common;

namespace Scripts.Gameplay.Player
{
    public interface IPlayerStateAnimator
    {
        bool Enabled { get; set; }

        void SetPlayerState(PlayerState playerState);
    }
}