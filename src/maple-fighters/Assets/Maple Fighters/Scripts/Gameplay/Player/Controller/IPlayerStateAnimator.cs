using Game.Common;

namespace Scripts.Gameplay.Player
{
    public interface IPlayerStateAnimator
    {
        void ChangePlayerState(PlayerState newPlayerState);
    }
}