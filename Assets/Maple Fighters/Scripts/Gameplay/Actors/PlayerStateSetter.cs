using Scripts.Utils.Shared;
using Shared.Game.Common;
using UnityEngine;

namespace Scripts.Gameplay.Actors
{
    public class PlayerStateSetter : MonoBehaviour
    {
        [SerializeField] private PlayerStateNetworkAnimator playerAnimator;

        public void SetState(PlayerState playerState)
        {
            playerAnimator.OnPlayerStateReceived(playerState);
        }
    }
}