using Game.Common;

namespace Scripts.Utils.Shared
{
    public interface IPlayerStateAnimator
    {
        void ChangePlayerState(PlayerState newPlayerState);
    }
}