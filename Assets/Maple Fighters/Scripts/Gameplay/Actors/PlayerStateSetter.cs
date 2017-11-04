using Scripts.Utils.Shared;
using Shared.Game.Common;
using UnityEngine;

namespace Scripts.Gameplay.Actors
{
    public class PlayerStateSetter : MonoBehaviour
    {
        [HideInInspector] public PlayerStateAnimator PlayerAnimator;

        public void SetState(PlayerState playerState)
        {
            PlayerAnimator?.OnPlayerStateReceived(playerState);
        }
    }
}